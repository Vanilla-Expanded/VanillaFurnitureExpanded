using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VanillaFurnitureEC
{
    public class CompBinClean : ThingComp, IThingHolder
    {
        public CompProperties_BinClean Props => (CompProperties_BinClean)props;

        private int ticksCounted = 0;

        public ThingOwner innerContainer;

        public float cleanupTarget = 0.9f;

        public CompBinClean()
        {
            innerContainer = new ThingOwner<Thing>(this);
        }

        public float AmountStored => AllFilth().Sum(x => x.thickness);

        private IEnumerable<Filth> AllFilth()
        {
            return innerContainer.ToList().Cast<Filth>();
        }

        public float AmountStoredPct => AmountStored / Props.capacity;

        public bool ShouldClean => AmountStoredPct >= cleanupTarget;

        public override void CompTick()
        {
            if (ticksCounted == Props.timerInTicks)
            {
                var filthInHomeArea = parent.Map.listerFilthInHomeArea.FilthInHomeArea;
                for (int i = 0; i < filthInHomeArea.Count; i++)
                {
                    var filth = filthInHomeArea[i] as Filth;
                    if (filth != null && CanAccept(filth))
                    {
                        if (filth.Position.InHorDistOf(parent.Position, Props.radius))
                        {
                            filth.DeSpawn();
                            innerContainer.TryAdd(filth);
                            break;
                        }
                    }
                }
                ticksCounted = 0;
            }
            ticksCounted++;
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

        private bool CanAccept(Filth filth)
        {
            if (AmountStoredPct < 1f)
            {
                return Props.capacity - AmountStored >= filth.thickness;
            }
            return false;
        }

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
            Scribe_Values.Look(ref cleanupTarget, "cleanupTarget", 0.9f);
        }
    }
}