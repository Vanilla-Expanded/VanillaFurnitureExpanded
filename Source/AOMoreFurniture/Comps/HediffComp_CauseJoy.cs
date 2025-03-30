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

        var amount = joyGainRate * JoyTunings.BaseJoyGainPerHour / GenDate.TicksPerHour;
        if (amount <= 0f)
            return;

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