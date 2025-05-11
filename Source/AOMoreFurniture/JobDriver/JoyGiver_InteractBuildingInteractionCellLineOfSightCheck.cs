using RimWorld;
using Verse;

namespace VanillaFurnitureEC;

public class JoyGiver_InteractBuildingInteractionCellLineOfSightCheck : JoyGiver_InteractBuildingInteractionCell
{
    protected override bool CanInteractWith(Pawn pawn, Thing t, bool inBed)
    {
        if (!NonPublicMethods.WatchBuildingUtility_EverPossibleToWatchFrom(t.InteractionCell, t.Position, t.Map, false, t.def))
            return false;

        return base.CanInteractWith(pawn, t, inBed);
    }
}