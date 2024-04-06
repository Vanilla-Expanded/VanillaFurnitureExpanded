using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace VanillaFurnitureEC
{

    public class WorkGiver_CleanBin : WorkGiver_Scanner
    {
        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            var list = new List<Thing>();

            foreach (var binDef in StaticCollections.compBinCleanDefs)
                list.AddRange(pawn.Map.listerThings.ThingsOfDef(binDef));

            return list;
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            var compBin = t.TryGetComp<CompBinClean>();
            if (compBin == null || (forced is false && compBin.ShouldClean is false || forced && compBin.AmountStored <= 0))
            {
                return false;
            }
            if (!pawn.CanReserve(t, 1, -1, null, forced))
            {
                return false;
            }
            if (t.IsForbidden(pawn))
            {
                return false;
            }
            return true;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return JobMaker.MakeJob(VFE_DefOf.VFE_CleanBin, t);
        }
    }
}