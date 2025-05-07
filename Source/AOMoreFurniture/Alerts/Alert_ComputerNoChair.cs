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

            for (var defIndex = 0; defIndex < VFE_DefOf.VFE_Play_Computer.thingDefs.Count; defIndex++)
            {
                var def = VFE_DefOf.VFE_Play_Computer.thingDefs[defIndex];
                for (var mapIndex = 0; mapIndex < maps.Count; mapIndex++)
                {
                    var map = maps[mapIndex];
                    var list = map.listerThings.ThingsOfDef(def);
                    for (var buildingIndex = 0; buildingIndex < list.Count; buildingIndex++)
                    {
                        var building = list[buildingIndex];
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