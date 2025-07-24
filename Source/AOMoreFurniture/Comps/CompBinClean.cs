using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace VanillaFurnitureEC
{
    public class CompBinClean : ThingComp, IThingHolder
    {
        public CompProperties_BinClean Props => (CompProperties_BinClean)props;

        public ThingOwner<Filth> innerContainer;

        public float cleanupTarget = 0.9f;

        public float lastBeautyValue;

        public CompBinClean()
        {
            innerContainer = new ThingOwner<Filth>(this);
        }

        public int AmountStored
        {
            get
            {
                var sum = 0;
                var filth = AllFilth();
                for (var j = 0; j < filth.Count; j++)
                    sum += filth[j].thickness;
                return sum;
            }
        }

        private List<Filth> AllFilth()
        {
            innerContainer ??= new ThingOwner<Filth>(this);
            return innerContainer.InnerListForReading;
        }

        public float AmountStoredPct => AmountStored / Props.capacity;

        public bool ShouldClean => AmountStoredPct >= cleanupTarget;

        public override void CompTickInterval(int delta) => TickInterval(delta);

        public override void CompTickRare() => TickInterval(GenTicks.TickRareInterval);

        public override void CompTickLong() => TickInterval(GenTicks.TickLongInterval);

        protected virtual void TickInterval(int delta)
        {
            if (!parent.Spawned)
                return;
            
            if (parent.IsHashIntervalTick(Props.timerInTicks, delta))
            {
                var amountStored = AmountStored;
                if (amountStored < Props.capacity)
                {
                    var filthInHomeArea = parent.Map.listerFilthInHomeArea.FilthInHomeArea;
                    for (int i = 0; i < filthInHomeArea.Count; i++)
                    {
                        if (filthInHomeArea[i] is Filth filth && CanAccept(filth, amountStored))
                        {
                            if (filth.Position.InHorDistOf(parent.Position, Props.radius))
                            {
                                filth.DeSpawn();
                                innerContainer.TryAdd(filth);
                                break;
                            }
                        }
                    }
                }
            }

            // 3 times per day, recalculate the beauty of the room due to building beauty changes
            if (parent.IsHashIntervalTick(GenDate.TicksPerDay / 3))
            {
                // Only do something if the beauty actually changed
                var beauty = parent.GetStatValue(StatDefOf.Beauty);
                if (!Mathf.Approximately(lastBeautyValue, beauty))
                {
                    lastBeautyValue = beauty;
                    parent.GetRoom()?.Notify_BedTypeChanged();
                }
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            lastBeautyValue = parent.GetStatValue(StatDefOf.Beauty);
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            base.PostDestroy(mode, previousMap);
            var filthList = AllFilth();
            for (var i = 0; i < filthList.Count; i++)
            {
                var filth = filthList[i];
                if (Rand.Chance(0.1f))
                {
                    FilthMaker.TryMakeFilth(parent.Position, previousMap, filth.def, filth.thickness);
                }
            }
        }

        public override string CompInspectStringExtra()
        {
            return "VFE.FilthStored".Translate(AmountStored, AmountStoredPct.ToStringPercent());
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (parent.Faction == Faction.OfPlayer)
            {
                yield return new Command_CleanupTarget
                {
                    comp = this
                };
            }
        }

        private bool CanAccept(Filth filth, int amountStored) => Props.capacity - amountStored >= filth.thickness;

        public void GetChildHolders(List<IThingHolder> outChildren)
        {
            ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
        }

        public ThingOwner GetDirectlyHeldThings()
        {
            return innerContainer;
        }

        public override void PostExposeData()
        {
            base.PostExposeData();

            Scribe_Deep.Look(ref innerContainer, "container", this);
            innerContainer ??= new ThingOwner<Filth>(this);
            Scribe_Values.Look(ref cleanupTarget, "cleanupTarget", 0.9f);
        }
    }
}