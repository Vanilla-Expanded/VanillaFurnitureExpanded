using RimWorld;
using Verse;

namespace VanillaFurnitureEC
{
    [DefOf]
    public static class SoundDefOf
    {
        public static SoundDef VFE_Arcade_SFX;

        public static SoundDef VFE_Computer_SFX;

        static SoundDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(SoundDefOf));
        }
    }
}