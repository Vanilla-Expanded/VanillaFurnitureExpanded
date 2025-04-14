using UnityEngine;
using Verse;

namespace VanillaFurnitureEC;

public class PlaceWorker_SameRoomRadius : PlaceWorker
{
    public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
    {
        var props = def.GetCompProperties<CompProperties_CauseJoyHediff_Aoe>();
        if (props == null)
            return;

        var map = Find.CurrentMap;
        var room = RegionAndRoomQuery.RoomAt(center, map);

        // If radio is in a room or adjacent to a room
        if (room is { AnyPassable: true })
            GenDraw.DrawRadiusRing(center, props.range, Color.white, cell => room == cell.GetRoom(map));
        else
            GenDraw.DrawRadiusRing(center, props.range);
    }
}