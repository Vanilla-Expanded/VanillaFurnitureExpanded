using RimWorld;
using Verse;
using Verse.AI;

namespace VanillaFurnitureEC;

public class JoyGiver_InteractBuildingInteractionCellSit : JoyGiver_InteractBuildingInteractionCell
{
    protected override Job TryGivePlayJob(Pawn pawn, Thing thing)
    {
        // If the JoyGiver requires a chair, include an interaction cell chair check
        if (def.requireChair && thing.InteractionCell.GetEdifice(pawn.Map)?.def.building.isSittable != true)
            return null;

        return base.TryGivePlayJob(pawn, thing);
    }
}