using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaFurnitureEC;

public class HediffComp_CauseJoy : HediffComp
{
    public float joyGainRate;
    public JoyKindDef joyKind;

    protected HediffCompProperties_CauseJoy Props => (HediffCompProperties_CauseJoy)props;

    public override void CompPostTickInterval(ref float severityAdjustment, int delta)
    {
        var joy = Pawn.needs.joy;
        if (joy == null)
            return;

        // If actively listening to the radio, the JobDriver will handle the recreation gain.
        var job = Pawn.CurJobDef;
        if (job != null && Props.jobDefsDisablingRecreation != null && Props.jobDefsDisablingRecreation.Contains(job))
            return;

        var amount = joyGainRate * delta * (JoyTunings.BaseJoyGainPerHour / GenDate.TicksPerHour);
        if (amount <= 0f)
            return;

        // If not actively listening to radio allow for passive joy loss.
        if (joyKind != null)
            amount *= joy.tolerances.JoyFactorFromTolerance(joyKind);
        amount = Mathf.Min(amount, 1f - joy.CurLevel);
        joy.CurLevel += amount;
        if (joyKind != null)
            joy.tolerances.Notify_JoyGained(amount, joyKind);
    }

    public override void CompExposeData()
    {
        Scribe_Values.Look(ref joyGainRate, nameof(joyGainRate));
        Scribe_Defs.Look(ref joyKind, nameof(joyKind));
    }
}