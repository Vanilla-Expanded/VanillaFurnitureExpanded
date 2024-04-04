using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VanillaFurnitureEC
{
    [StaticConstructorOnStartup]
    public static class StaticCollections
    {
        public static List<ThingDef> compBinCleanDefs = DefDatabase<ThingDef>.AllDefsListForReading
            .Where(def => def.comps.Any(comp => comp is CompProperties_BinClean)).ToList();
    }
}