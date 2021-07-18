using RimWorld;
using System.Collections.Generic;
using Verse;

namespace AOMoreFurniture
{
    internal class ThoughtWorker_RadioBase : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            bool flag = false;
            bool flag2 = !p.Spawned;
            ThoughtState result;
            if (flag2)
            {
                result = false;
            }
            else
            {
                List<Thing> list = p.Map.listerThings.ThingsOfDef(ThingDefOf.Radio_Spacer);
                for (int i = 0; i < list.Count; i++)
                {
                    CompPowerTrader compPowerTrader = list[i].TryGetComp<CompPowerTrader>();
                    bool flag3 = compPowerTrader == null || compPowerTrader.PowerOn;
                    if (flag3)
                    {
                        bool flag4 = p.Position.InHorDistOf(list[i].Position, 8f);
                        if (flag4)
                        {
                            return ThoughtState.ActiveAtStage(1);
                        }
                    }
                }
                List<Thing> list2 = p.Map.listerThings.ThingsOfDef(ThingDefOf.Radio_Industrial);
                for (int j = 0; j < list2.Count; j++)
                {
                    CompPowerTrader compPowerTrader2 = list2[j].TryGetComp<CompPowerTrader>();
                    bool flag5 = compPowerTrader2 == null || (compPowerTrader2.PowerOn && !flag);
                    if (flag5)
                    {
                        bool flag6 = p.Position.InHorDistOf(list2[j].Position, 5f);
                        if (flag6)
                        {
                            return ThoughtState.ActiveAtStage(0);
                        }
                    }
                }
                result = false;
            }
            return result;
        }
    }
}