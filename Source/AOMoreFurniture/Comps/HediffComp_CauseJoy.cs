using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaFurnitureEC;

public class HediffComp_CauseJoy : HediffComp
{
    public float joyGainRate;
    public JoyKindDef joyKind;

    public override void CompPostTick(ref float severityAdjustment)
    {
        var joy = Pawn.needs.joy;
        if (joy == null)
            return;

        // If actively listening to the radio, the JobDriver will handle the recreation gain.
        if (Pawn.CurJobDef == VFE_DefOf.VFE_ListenToMusic)
            return;

        var amount = joyGainRate * JoyTunings.BaseJoyGainPerHour / GenDate.TicksPerHour;
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