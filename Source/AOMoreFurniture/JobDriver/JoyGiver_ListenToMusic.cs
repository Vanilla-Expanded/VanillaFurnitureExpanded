using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace VanillaFurnitureEC;

public class JoyGiver_ListenToMusic : JoyGiver_InteractBuilding
{
    protected override bool CanInteractWith(Pawn pawn, Thing t, bool inBed)
        => base.CanInteractWith(pawn, t, inBed) && t.TryGetComp<CompCauseJoyHediff_Aoe>()?.IsPawnAffected(pawn) == true;

    protected override Job TryGivePlayJob(Pawn pawn, Thing bestGame)
    {
        if (!TryFindBestListenCell(bestGame, pawn, def.desireSit, out var position, out var chair))
            return null;

        return JobMaker.MakeJob(def.jobDef, bestGame, position, chair);
    }

    // Modified WatchBuildingUtility.TryFindBestWatchCell
    private static bool TryFindBestListenCell(Thing toWatch, Pawn pawn, bool desireSit, out IntVec3 result, out Building chair)
    {
        result = IntVec3.Invalid;
        chair = null;

        var comp = toWatch.TryGetComp<CompCauseJoyHediff_Aoe>();
        if (comp != null)
        {
            try
            {
                comp.CacheCurrentRoom();

                foreach (var curPos in GenRadial.RadialCellsAround(toWatch.Position, Mathf.Min(GenRadial.MaxRadialPatternRadius - 1f, comp.Props.range), false))
                {
                    // Make sure position is in bounds
                    if (!curPos.InBounds(toWatch.Map))
                        continue;
                    // Make sure we can reserve the position/spot
                    if (!pawn.CanReserveSittableOrSpot(curPos) || !pawn.Map.pawnDestinationReservationManager.CanReserve(curPos, pawn))
                        continue;
                    // Make sure the position is actually in range of the radio (same room, etc.)
                    if (!comp.IsPositionInRange(curPos))
                        break;

                    var edifice = curPos.GetEdifice(toWatch.Map);
                    if (edifice != null && edifice.def.building.isSittable)
                    {
                        result = curPos;
                        chair = edifice;
                        break;
                    }

                    // TODO: Fix this part at some point
                    if (!curPos.IsValid)
                    {
                        result = curPos;
                        if (!desireSit)
                            break;
                    }
                }
            }
            finally
            {
                comp.ClearCurrentRoom();
            }
        }

        return result.IsValid;
    }
}