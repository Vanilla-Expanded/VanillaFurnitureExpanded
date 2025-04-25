using System;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace VanillaFurnitureEC;

public class JoyGiver_UseLounger : JoyGiver_InteractBuilding
{
    public override Job TryGiveJob(Pawn pawn)
    {
        if (IsWeatherGood(pawn))
            return base.TryGiveJob(pawn);
        return null;
    }

    public override Job TryGiveJobWhileInBed(Pawn pawn)
    {
        var bed = pawn.CurrentBed();
        if (!def.thingDefs.Contains(bed.def))
            return null;
        if (RoofUtility.IsAnyCellUnderRoof(bed))
            return null;
        if (IsWeatherGood(pawn))
            return TryGivePlayJob(pawn, bed);

        return null;
    }

    protected override Job TryGivePlayJob(Pawn pawn, Thing bestThing) => JobMaker.MakeJob(def.jobDef, bestThing);

    private static bool IsWeatherGood(Pawn pawn)
    {
        // Make sure there's no rain
        if (pawn.Map.weatherManager.curWeather.rainRate > 0.1f)
            return false;
        // Make sure it's sunny
        if (GenCelestial.CurCelestialSunGlow(pawn.Map) < 0.65f)
            return false;
        // Ensure the weather and temperature are good
        if (!JoyUtility.EnjoyableOutsideNow(pawn))
            return false;
        // Make sure there's no unnatural darkness
        if (ModsConfig.AnomalyActive && pawn.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.UnnaturalDarkness))
            return false;

        // The weather is good, go ahead
        return true;
    }

    protected override bool CanInteractWith(Pawn pawn, Thing t, bool inBed)
    {
        if (!base.CanInteractWith(pawn, t, inBed))
            return false;

        var assignable = t.TryGetComp<CompAssignableToPawn>();
        // If there's any assigned pawn and this one isn't, can't use for recreation
        if (assignable != null && assignable.AssignedPawns.Any() && !assignable.AssignedPawns.Contains(pawn))
            return false;

        if (t is not Building_Bed bed)
            return true;

        // Can't use medical bed for recreation
        if (bed.Medical)
            return false;

        // Check if bed's ownership type matches pawn type
        switch (bed.ForOwnerType)
        {
            case BedOwnerType.Prisoner:
                if (!pawn.IsPrisoner)
                    return false;
                break;
            case BedOwnerType.Slave:
                if (!pawn.IsSlave)
                    return false;
                break;
            case BedOwnerType.Colonist:
            default:
                break;
        }

        // Make sure the lounger there's no roof over any of the lounger tiles
        return !RoofUtility.IsAnyCellUnderRoof(t);
    }
}