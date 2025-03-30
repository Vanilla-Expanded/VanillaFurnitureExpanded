using RimWorld;
using Verse;

namespace VanillaFurnitureEC;

public class CompProperties_CauseJoyHediff_Aoe : CompProperties
{
    public HediffDef hediff;
    public float range;
    // How frequently to update the hediff. Hediff duration is set to this value + 5.
    public int checkInterval = 10;
    // TODO: Remove if not used in the end
    // The amount and type of joy
    // public float joyGainRate;
    // public JoyKindDef joyKind;

    public CompProperties_CauseJoyHediff_Aoe() => compClass = typeof(CompCauseJoyHediff_Aoe);
}