using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VanillaFurnitureEC
{
    public class CompBinClean : ThingComp, IThingHolder
    {
        public CompProperties_BinClean Props => (CompProperties_BinClean)props;

        public ThingOwner<Filth> innerContainer;

        public float cleanupTarget = 0.9f;

        public CompBinClean()
        {
            innerContainer = new ThingOwner<Filth>(this);
        }

        public int AmountStored => AllFilth().Sum(x => x.thickness);

        private IEnumerable<Filth> AllFilth() => innerContainer ??= new ThingOwner<Filth>(this);

        public float AmountStoredPct => AmountStored / Props.capacity;

        public bool ShouldClean => AmountStoredPct >= cleanupTarget;

        public override void CompTick()
        {
            if (parent.IsHashIntervalTick(Props.timerInTicks))
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

            // Backwards compatibility to support change from ThingOwner to ThingOwner<Filth>.
            // Just keep a single "Scribe_Deep.Look(ref innerContainer, "innerContainer", this)"
            // once removing backwards compatibility in 1.6.
            if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                ThingOwner container = null;
                Scribe_Deep.Look(ref container, "innerContainer", this);

                switch (container)
                {
                    case ThingOwner<Filth> filth:
                        innerContainer = filth;
                        break;
                    // Handle backwards compatibility by transferring all
                    // filth from ThingOwner<Thing> to ThingOwner<Filth>
                    case ThingOwner<Thing> thingOwner:
                    {
                        innerContainer = new ThingOwner<Filth>(this);
                        thingOwner.TryTransferAllToContainer(innerContainer);
                        break;
                    }
                }
            }
            else
            {
                Scribe_Deep.Look(ref innerContainer, "innerContainer", this);
            }

            innerContainer ??= new ThingOwner<Filth>(this);
            Scribe_Values.Look(ref cleanupTarget, "cleanupTarget", 0.9f);
        }
    }
}