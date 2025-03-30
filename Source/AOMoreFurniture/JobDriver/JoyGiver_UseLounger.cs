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
}