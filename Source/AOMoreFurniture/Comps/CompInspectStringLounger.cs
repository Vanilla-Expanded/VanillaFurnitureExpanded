using System.Text;
using RimWorld;
using Verse;

namespace VanillaFurnitureEC;

public class CompInspectStringLounger : CompInspectString
{
    public override string CompInspectStringExtra()
    {
        var stringBuilder = new StringBuilder();

        if (RoofUtility.IsAnyCellUnderRoof(parent))
        {
            AppendFailReason("VFE.SunbathingNotPossible.UnderRoof".Translate());
            return stringBuilder.ToString();
        }

        if (parent.Map.weatherManager.curWeather.rainRate > 0.1f)
            AppendFailReason("VFE.SunbathingNotPossible.BadWeather".Translate());
        if (GenCelestial.CurCelestialSunGlow(parent.Map) < 0.65f)
            AppendFailReason("not sunny enough");

        var conditions = parent.Map.GameConditionManager.ActiveConditions;
        for (int index = 0; index < conditions.Count; ++index)
        {
            var activeCondition = conditions[index];

            if ((ModsConfig.AnomalyActive && activeCondition.def == GameConditionDefOf.UnnaturalDarkness) || !activeCondition.AllowEnjoyableOutsideNow(parent.Map))
                AppendFailReason(activeCondition.def.label);
        }

        var assignable = parent.GetComp<CompAssignableToPawn>();
        if (assignable != null && assignable.AssignedPawnsForReading.Count > 0)
        {
            foreach (var pawn in assignable.AssignedPawnsForReading)
            {
                if (!pawn.needs.EnjoysOutdoors())
                    AppendFailReason("VFE.SunbathingNotPossible.PawnOutdoors".Translate(pawn.Named("PAWN")));
                if (!pawn.ComfortableTemperatureRange().Includes(parent.Map.mapTemperature.OutdoorTemp))
                    AppendFailReason("VFE.SunbathingNotPossible.PawnComfortableTemperature".Translate(pawn.Named("PAWN")));
            }

            if (stringBuilder.Length == 0)
                stringBuilder.Append("VFE.SunbathingPossibleForPawn".Translate(assignable.AssignedPawnsForReading.ToStringSafeEnumerable().Named("PAWNLIST")));
        }

        if (stringBuilder.Length == 0)
            stringBuilder.Append("VFE.SunbathingPossibleGeneric".Translate());

        return stringBuilder.ToString();

        void AppendFailReason(string reason)
        {
            if (stringBuilder.Length == 0)
            {
                stringBuilder.Append("VFE.SunbathingNotPossible".Translate());
                stringBuilder.Append(' ');
                stringBuilder.Append(reason);
            }
            else
            {
                stringBuilder.AppendWithComma(reason);
            }
        }
    }
}