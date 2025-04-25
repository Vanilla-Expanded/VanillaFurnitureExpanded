using Verse;

namespace VanillaFurnitureEC;

public class ExtendedSitFacingJoyDataExtension : DefModExtension
{
    public SoundDef sound;
    public IntRange soundRefireDelay = new(400, 500);
    public EffecterDef effecter;
    public float researchOnFinished;
    public bool allowComfortFromCell = true;
}