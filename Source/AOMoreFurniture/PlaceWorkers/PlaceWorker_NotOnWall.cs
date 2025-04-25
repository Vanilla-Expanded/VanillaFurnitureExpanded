using RimWorld;
using Verse;

namespace VanillaFurnitureEC;

public class PlaceWorker_NotOnWall : PlaceWorker
{
    public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
    {
        var list = loc.GetThingList(map);
        for (var i = 0; i < list.Count; i++)
        {
            if (GenConstruct.BuiltDefOf(list[i].def) is ThingDef { Fillage: FillCategory.Full })
                return "VFE.CannotPlaceOnWall".Translate();
        }

        return true;
    }
}