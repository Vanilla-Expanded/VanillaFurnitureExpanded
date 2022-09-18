using Verse;

namespace AOMoreFurniture
{
    public class CompProperties_BinClean : CompProperties
    {
        public float Radius;

        public int TimerInTicks;

        public CompProperties_BinClean()
        {
            this.compClass = typeof(CompBinClean);
        }
    }
}