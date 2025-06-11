using RimWorld;
using Verse;
using VFECore;

namespace VanillaFurnitureEC;

public class CompCauseJoyHediff_Aoe : CompCustomCauseHediff_AoE
{
    protected override Hediff GiveOrUpdateHediff(Pawn target)
    {
        var hediff = base.GiveOrUpdateHediff(target);

        if (hediff is HediffWithComps hediffWithComps)
        {
            var causeJoy = hediffWithComps.GetComp<HediffComp_CauseJoy>();
            if (causeJoy != null)
            {
                var joyGainRate = parent.GetStatValue(StatDefOf.JoyGainFactor);
                if (joyGainRate >= causeJoy.joyGainRate)
                {
                    causeJoy.joyGainRate = joyGainRate;
                    causeJoy.joyKind = parent.def.building.joyKind;
                }
            }
            else
                Log.ErrorOnce($"{parent.def.defName} has {nameof(CompCauseJoyHediff_Aoe)} with a hediff in props which does not have a {nameof(HediffComp_CauseJoy)}", Gen.HashCombineInt(1723408196, hediff.def.shortHash));
        }
        else
            Log.ErrorOnce($"{parent.def.defName} has {nameof(CompCauseJoyHediff_Aoe)} with a hediff which is not {nameof(HediffWithComps)}", Gen.HashCombineInt(-871064605, hediff.def.shortHash));

        return hediff;
    }

    public override bool IsPawnAffected(Pawn target) => target.needs.joy != null && base.IsPawnAffected(target);
}