using Verse;

namespace VanillaFurnitureEC
{
    public class CompProperties_BinClean : CompProperties
    {
        public CompProperties_BinClean()
        {
            compClass = typeof(CompBinClean);
        }

        public float radius;
        public int timerInTicks;
    }
}