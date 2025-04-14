using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VanillaFurnitureEC
{
    public class CompBinClean : ThingComp, IThingHolder
    {
        public CompProperties_BinClean Props => (CompProperties_BinClean)props;

        public ThingOwner innerContainer;

        public float cleanupTarget = 0.9f;

        public CompBinClean()
        {
            innerContainer = new ThingOwner<Thing>(this);
        }

        public float AmountStored => AllFilth().Sum(x => x.thickness);

        private IEnumerable<Filth> AllFilth()
        {
            if (innerContainer is null)
            {
                innerContainer = new ThingOwner<Thing>(this);
            }
            return innerContainer.ToList().Cast<Filth>();
        }

        public float AmountStoredPct => AmountStored / Props.capacity;

        public bool ShouldClean => AmountStoredPct >= cleanupTarget;

        public override void CompTick()
        {
            if (AmountStoredPct < 1f && parent.IsHashIntervalTick(Props.timerInTicks))
            {
                var filthInHomeArea = parent.Map.listerFilthInHomeArea.FilthInHomeArea;
                for (int i = 0; i < filthInHomeArea.Count; i++)
                {
                    if (filthInHomeArea[i] is Filth filth && CanAccept(filth))
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

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            base.PostDestroy(mode, previousMap);
            foreach (var filth in AllFilth())
            {
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

        private bool CanAccept(Filth filth) => Props.capacity - AmountStored >= filth.thickness;

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
            Scribe_Deep.Look(ref innerContainer, "innerContainer", this);
            if (innerContainer is null)
            {
                innerContainer = new ThingOwner<Thing>(this);
            }
            Scribe_Values.Look(ref cleanupTarget, "cleanupTarget", 0.9f);
        }
    }
}