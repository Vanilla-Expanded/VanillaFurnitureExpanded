using RimWorld;
using Verse;

namespace VanillaFurnitureEC
{
    [DefOf]
    public static class EffecterDefOf
    {
        public static EffecterDef Joy_HoldChips;

        public static EffecterDef Joy_PlayPiano;

        static EffecterDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(EffecterDefOf));
        }
    }
}