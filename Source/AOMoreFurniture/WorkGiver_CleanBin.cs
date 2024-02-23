using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace VanillaFurnitureEC
{

    public class WorkGiver_CleanBin : WorkGiver_Scanner
    {
        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            return pawn.Map.listerThings.ThingsOfDef(VFE_DefOf.Bin_Small).Concat(pawn.Map.listerThings.ThingsOfDef(VFE_DefOf.Bin_Large)); ;
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            var compBin = t.TryGetComp<CompBinClean>();
            if (compBin == null || compBin.ShouldClean is false)
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