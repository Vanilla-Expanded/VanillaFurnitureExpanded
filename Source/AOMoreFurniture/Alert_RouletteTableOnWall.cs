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

            foreach (var map in maps)
            {
                var tables = map.listerThings.ThingsOfDef(VFE_DefOf.Joy_RouletteTable);
                foreach (var table in tables)
                {
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