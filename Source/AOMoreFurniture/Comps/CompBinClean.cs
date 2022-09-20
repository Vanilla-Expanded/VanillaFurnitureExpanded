using Verse;

namespace VanillaFurnitureEC
{
    public class CompBinClean : ThingComp
    {
        public CompProperties_BinClean Props => (CompProperties_BinClean)props;

        private int ticksCounted = 0;

        public override void CompTick()
        {
            if (ticksCounted == Props.timerInTicks)
            {
                var filthInHomeArea = parent.Map.listerFilthInHomeArea.FilthInHomeArea;
                for (int i = 0; i < filthInHomeArea.Count; i++)
                {
                    var filth = filthInHomeArea[i];
                    if (filth.Position.InHorDistOf(parent.Position, Props.radius))
                    {
                        filth.Destroy(DestroyMode.Vanish);
                        break;
                    }
                }

                ticksCounted = 0;
            }

            ticksCounted++;
        }
    }
}