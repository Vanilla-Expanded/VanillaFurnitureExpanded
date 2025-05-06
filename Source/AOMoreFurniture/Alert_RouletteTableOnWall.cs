using System.Collections.Generic;
using RimWorld;
using Verse;

namespace VanillaFurnitureEC;

public class Alert_RouletteTableOnWall : Alert
{
    private readonly List<Thing> badTableResults = [];

    private List<Thing> BadTables
    {
        get
        {
            badTableResults.Clear();

            var maps = Find.Maps;
            var faction = Faction.OfPlayer;

            for (var mapIndex = 0; mapIndex < maps.Count; mapIndex++)
            {
                var map = maps[mapIndex];
                var tables = map.listerThings.ThingsOfDef(VFE_DefOf.Joy_RouletteTable);
                for (var buildingIndex = 0; buildingIndex < tables.Count; buildingIndex++)
                {
                    var table = tables[buildingIndex];
                    if (table.Faction == faction && JoyGiver_PlayBilliards.ThingHasStandableSpaceOnAllSides(table))
                        badTableResults.Add(table);
                }
            }

            return badTableResults;
        }
    }

    public Alert_RouletteTableOnWall()
    {
        defaultLabel = "VFE.RouletteTableOnWall".Translate();
        defaultExplanation = "VFE.RouletteTableOnWallDesc".Translate();
    }

    public override AlertReport GetReport() => AlertReport.CulpritsAre(BadTables);
}