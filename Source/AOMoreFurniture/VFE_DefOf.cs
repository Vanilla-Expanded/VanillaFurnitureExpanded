using RimWorld;
using Verse;

namespace VanillaFurnitureEC
{
    [DefOf]
    public static class VFE_DefOf
    {
        // Buildings
        public static ThingDef Joy_RouletteTable;
        public static ThingDef Stone_Campfire;
        [MayRequireIdeology]
        public static ThingDef Stone_DarklightCampfire;

        // Motes
        public static ThingDef Mote_FlyingDart;

        // Jobs
        public static JobDef VFE_CleanBin;

        // JoyGivers
        public static JoyGiverDef Play_ComputerModern;
        public static JoyGiverDef Play_ComputerIndustrial;

        static VFE_DefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(VFE_DefOf));
        }
    }
}