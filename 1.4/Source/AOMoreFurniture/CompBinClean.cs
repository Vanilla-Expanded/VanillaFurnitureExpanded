using System.Collections.Generic;
using Verse;

namespace AOMoreFurniture
{
    public class CompBinClean : ThingComp
    {
        private int TicksCounted = 0;

        public CompProperties_BinClean Cleanprops
        {
            get
            {
                return this.props as CompProperties_BinClean;
            }
        }

        public override void CompTick()
        {
            this.TicksCounted++;
            bool flag = this.TicksCounted == this.Cleanprops.TimerInTicks;
            if (flag)
            {
                this.TicksCounted = 0;
                Map map = this.parent.Map;
                List<Thing> filthInHomeArea = map.listerFilthInHomeArea.FilthInHomeArea;
                for (int i = 0; i < filthInHomeArea.Count; i++)
                {
                    bool flag2 = filthInHomeArea[i].Position.InHorDistOf(this.parent.Position, this.Cleanprops.Radius);
                    if (flag2)
                    {
                        filthInHomeArea[i].Destroy(DestroyMode.Vanish);
                        break;
                    }
                }
            }
            base.CompTick();
        }
    }
}