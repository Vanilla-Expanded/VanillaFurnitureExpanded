using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace VanillaFurnitureEC;

[StaticConstructorOnStartup]
public class Alert_ComputerNoChair : Alert
{
    // Just in case, remove null JoyGivers
    private static readonly List<JoyGiverDef> JoyGivers = new[] { VFE_DefOf.Play_ComputerModern, VFE_DefOf.Play_ComputerIndustrial }.Where(x => x != null).ToList();

    private readonly List<Thing> badBuildingsResult = [];

    private List<Thing> BadBuildings
    {
        get
        {
            badBuildingsResult.Clear();
            var maps = Find.Maps;

            foreach (var joyGiver in JoyGivers)
            {
                foreach (var def in joyGiver.thingDefs)
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