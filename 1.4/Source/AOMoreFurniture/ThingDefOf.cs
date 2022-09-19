using RimWorld;
using Verse;

namespace VanillaFurnitureEC
{
    [DefOf]
    public static class ThingDefOf
    {
        public static ThingDef Mote_FlyingDart;

        public static ThingDef Radio_Industrial;

        public static ThingDef Radio_Spacer;

        static ThingDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf));
        }
    }
}