using RimWorld;
using Verse;

namespace VanillaFurnitureEC
{
    [DefOf]
    public static class SoundDefOf
    {
        public static SoundDef Arcade_SFX;

        public static SoundDef Computer_SFX;

        static SoundDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(SoundDefOf));
        }
    }
}