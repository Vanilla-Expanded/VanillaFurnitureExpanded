using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaFurnitureEC;

public class PlaceWorker_LinkToFirepits : PlaceWorker
{
    private static readonly ThingDef[] Firepits = new[] { VFE_DefOf.Stone_Campfire, VFE_DefOf.Stone_DarklightCampfire }.Where(x => x != null).ToArray();
    public const float Range = 11.9f;

    public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
    {
        for (var buildingIndex = 0; buildingIndex < Firepits.Length; buildingIndex++)
        {
            var firepit = Firepits[buildingIndex];
            var placeWorker = firepit.PlaceWorkers.OfType<PlaceWorker_LinkToCampfire>().FirstOrDefault();
            float range;
            if (placeWorker == null)
            {
                range = Range;
                GenDraw.DrawRadiusRing(center, range);
            }
            else
            {
                range = placeWorker.range;
            }

            var forCell = Find.CurrentMap.listerBuldingOfDefInProximity.GetForCell(center, range, firepit);
            for (var i = 0; i < forCell.Count; i++)
                GenDraw.DrawLineBetween(GenThing.TrueCenter(center, Rot4.North, def.size, def.Altitude), forCell[i].TrueCenter(), SimpleColor.Green);
        }
    }
}