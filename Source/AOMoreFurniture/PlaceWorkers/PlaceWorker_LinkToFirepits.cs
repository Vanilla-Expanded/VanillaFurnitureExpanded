using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaFurnitureEC;

public class PlaceWorker_LinkToFirepits : PlaceWorker
{
    private static readonly ThingDef[] Firepits = new[] {VFE_DefOf.Stone_Campfire, VFE_DefOf.Stone_DarklightCampfire}.Where(x => x != null).ToArray();
    public float range = 11.9f;

    public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
    {
        GenDraw.DrawRadiusRing(center, range);
        foreach (var firepit in Firepits)
        {
            var forCell = Find.CurrentMap.listerBuldingOfDefInProximity.GetForCell(center, range, firepit);
            for (var i = 0; i < forCell.Count; i++)
                GenDraw.DrawLineBetween(GenThing.TrueCenter(center, Rot4.North, def.size, def.Altitude), forCell[i].TrueCenter(), SimpleColor.Green);
        }
    }
}