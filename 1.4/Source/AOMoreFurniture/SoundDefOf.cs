using RimWorld;
using Verse;

namespace VanillaFurnitureEC
{
    [DefOf]
    public static class SoundDefOf
    {
        public static SoundDef Arcade_SFXFour;

        public static SoundDef Arcade_SFXOne;

        public static SoundDef Arcade_SFXThree;

        public static SoundDef Arcade_SFXTwo;

        public static SoundDef Computer_SFXOne;

        public static SoundDef Computer_SFXTwo;

        static SoundDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(SoundDefOf));
        }
    }
}