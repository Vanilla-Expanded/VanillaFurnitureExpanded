using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace AOMoreFurniture
{
    internal class PlaceWorker_ACUnit : PlaceWorker
    {
        public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
        {
            IntVec3 intVec = center + IntVec3.South.RotatedBy(rot), c = intVec;
            if (rot == Rot4.East)
            {
                c = new IntVec3(intVec.x, intVec.y, intVec.z - 1);
            }
            else if (rot == Rot4.West)
            {
                c = new IntVec3(intVec.x, intVec.y, intVec.z + 1);
            }
            else if (rot == Rot4.North)
            {
                c = new IntVec3(intVec.x + 1, intVec.y, intVec.z);
            }
            else if (rot == Rot4.South)
            {
                c = new IntVec3(intVec.x - 1, intVec.y, intVec.z);
            }

            IntVec3 intVec2 = center + IntVec3.North.RotatedBy(rot), c2 = intVec2;
            if (rot == Rot4.East)
            {
                c2 = new IntVec3(intVec2.x, intVec2.y, intVec2.z - 1);
            }
            else if (rot == Rot4.West)
            {
                c2 = new IntVec3(intVec2.x, intVec2.y, intVec2.z + 1);
            }
            else if (rot == Rot4.North)
            {
                c2 = new IntVec3(intVec2.x + 1, intVec2.y, intVec2.z);
            }
            else if (rot == Rot4.South)
            {
                c2 = new IntVec3(intVec2.x - 1, intVec2.y, intVec2.z);
            }

            AcceptanceReport result;
            if (intVec.Impassable(map) || intVec2.Impassable(map) || c.Impassable(map) || c2.Impassable(map))
            {
                result = "ACUnit".Translate();
            }
            else
            {
                Frame f1 = this.GetFirst(intVec, map) as Frame;
                Frame f2 = this.GetFirst(intVec2, map) as Frame;
                Frame f3 = this.GetFirst(c, map) as Frame;
                Frame f4 = this.GetFirst(c2, map) as Frame;

                if ((f1 != null && f1.def.entityDefToBuild != null && f1.def.entityDefToBuild.passability == Traversability.Impassable) ||
                    (f2 != null && f2.def.entityDefToBuild != null && f2.def.entityDefToBuild.passability == Traversability.Impassable) ||
                    (f3 != null && f3.def.entityDefToBuild != null && f3.def.entityDefToBuild.passability == Traversability.Impassable) ||
                    (f4 != null && f4.def.entityDefToBuild != null && f4.def.entityDefToBuild.passability == Traversability.Impassable))
                {
                    result = "ACUnit".Translate();
                }
                else
                {
                    Blueprint b1 = this.GetFirst(intVec, map) as Blueprint;
                    Blueprint b2 = this.GetFirst(intVec2, map) as Blueprint;
                    Blueprint b3 = this.GetFirst(c, map) as Blueprint;
                    Blueprint b4 = this.GetFirst(c2, map) as Blueprint;

                    if ((b1 != null && b1.def.entityDefToBuild != null && b1.def.entityDefToBuild.passability == Traversability.Impassable) ||
                        (b2 != null && b2.def.entityDefToBuild != null && b2.def.entityDefToBuild.passability == Traversability.Impassable) ||
                        (b3 != null && b3.def.entityDefToBuild != null && b3.def.entityDefToBuild.passability == Traversability.Impassable) ||
                        (b4 != null && b4.def.entityDefToBuild != null && b4.def.entityDefToBuild.passability == Traversability.Impassable))
                    {
                        result = "ACUnit".Translate();
                    }
                    else
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
        {
            Map currentMap = Find.CurrentMap;

            IntVec3 cold1 = center + IntVec3.South.RotatedBy(rot), cold2 = cold1;
            if (rot == Rot4.East)
                cold2 = new IntVec3(cold1.x, cold1.y, cold1.z - 1);
            else if (rot == Rot4.West)
                cold2 = new IntVec3(cold1.x, cold1.y, cold1.z + 1);
            else if (rot == Rot4.North)
                cold2 = new IntVec3(cold1.x + 1, cold1.y, cold1.z);
            else if (rot == Rot4.South)
                cold2 = new IntVec3(cold1.x - 1, cold1.y, cold1.z);

            IntVec3 hot1 = center + IntVec3.North.RotatedBy(rot), hot2 = center;
            if (rot == Rot4.East)
                hot2 = new IntVec3(hot1.x, hot1.y, hot1.z - 1);
            else if (rot == Rot4.West)
                hot2 = new IntVec3(hot1.x, hot1.y, hot1.z + 1);
            else if (rot == Rot4.North)
                hot2 = new IntVec3(hot1.x + 1, hot1.y, hot1.z);
            else if (rot == Rot4.South)
                hot2 = new IntVec3(hot1.x - 1, hot1.y, hot1.z);

            GenDraw.DrawFieldEdges(new List<IntVec3>
            {
                cold1,
                cold2
            }, GenTemperature.ColorSpotCold);

            GenDraw.DrawFieldEdges(new List<IntVec3>
            {
                hot1,
                hot2
            }, new Color(0.9f, 0.9f, 0.9f, 0.5f));

            Room room1 = hot1.GetRoom(currentMap);
            Room room2 = cold1.GetRoom(currentMap);
            if (room1 != null && room2 != null)
            {
                if (room1 == room2 && !room1.UsesOutdoorTemperature)
                {
                    GenDraw.DrawFieldEdges(room1.Cells.ToList(), GenTemperature.ColorRoomCold);
                }
                else if (!room1.UsesOutdoorTemperature)
                {
                    GenDraw.DrawFieldEdges(room1.Cells.ToList(), new Color(0.9f, 0.9f, 0.9f, 0.5f));
                }
                else if (!room2.UsesOutdoorTemperature)
                {
                    GenDraw.DrawFieldEdges(room2.Cells.ToList(), GenTemperature.ColorRoomCold);
                }
            }
        }

        private Thing GetFirst(IntVec3 c, Map map)
        {
            List<Thing> thingList = c.GetThingList(map);
            for (int i = 0; i < thingList.Count; i++)
            {
                Thing thing = thingList[i];
                bool flag = thing != null && (thing is Blueprint || thing is Building);
                if (flag)
                {
                    return thing;
                }
            }
            return null;
        }
    }
}