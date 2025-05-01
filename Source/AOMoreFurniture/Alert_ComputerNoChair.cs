using System.Collections.Generic;
using RimWorld;
using Verse;

namespace VanillaFurnitureEC;

[StaticConstructorOnStartup]
public class Alert_ComputerNoChair : Alert
{
    private readonly List<Thing> badBuildingsResult = [];

    private List<Thing> BadBuildings
    {
        get
        {
            badBuildingsResult.Clear();
            var maps = Find.Maps;

            foreach (var def in VFE_DefOf.VFE_Play_Computer.thingDefs)
            {
                foreach (var map in maps)
                {
                    foreach (var building in map.listerThings.ThingsOfDef(def))
                    {
                        if (building.Faction == Faction.OfPlayer && !JoyBuildingUsable(building))
                            badBuildingsResult.Add(building);
                    }
                }
            }

            return badBuildingsResult;
        }
    }

    public Alert_ComputerNoChair()
    {
        defaultLabel = "VFE.ComputerNeedChairs".Translate();
        defaultExplanation = "VFE.ComputerNeedChairsDesc".Translate();
    }

    public override AlertReport GetReport() => AlertReport.CulpritsAre(BadBuildings);

    private bool JoyBuildingUsable(Thing building) => building.InteractionCell.GetEdifice(building.Map)?.def.building.isSittable == true;
}