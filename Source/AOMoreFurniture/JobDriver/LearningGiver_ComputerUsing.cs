using RimWorld;
using Verse;
using Verse.AI;

namespace VanillaFurnitureEC;

public class LearningGiver_ComputerUsing : LearningGiver
{
    public override bool CanGiveDesire
    {
        get
        {
            // Reverse order since, presumably - the lowest learning rate computer will be at the end.
            for (var index = VFE_DefOf.VFE_Play_Computer.thingDefs.Count - 1; index >= 0; index--)
            {
                if (VFE_DefOf.VFE_Play_Computer.thingDefs[index].IsResearchFinished)
                    return true;
            }

            return false;
        }
    }

    public override bool CanDo(Pawn pawn) => base.CanDo(pawn) && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight) && TryFindComputer(pawn, out _);

    public override Job TryGiveJob(Pawn pawn)
    {
        if (TryFindComputer(pawn, out var computer))
            return JobMaker.MakeJob(def.jobDef, computer);
        return null;
    }

    private static bool TryFindComputer(Pawn pawn, out Thing computer)
    {
        for (var index = 0; index < VFE_DefOf.VFE_Play_Computer.thingDefs.Count; index++)
        {
            computer = GenClosest.ClosestThingReachable(
                pawn.Position,
                pawn.Map,
                ThingRequest.ForDef(VFE_DefOf.VFE_Play_Computer.thingDefs[index]),
                PathEndMode.InteractionCell,
                TraverseParms.For(pawn),
                validator: thing => CanUseComputerNow(thing, pawn));

            if (computer != null)
                return true;
        }

        computer = null;
        return false;
    }

    public static bool CanUseComputerNow(Thing thing, Pawn pawn)
    {
        // Only spawned things allowed
        if (!thing.Spawned)
            return false;

        // Only allow if the computer is not forbidden
        if (thing.IsForbidden(pawn))
            return false;

        // Make sure it's not reserved right now, etc.
        if (!pawn.CanReserve(thing))
            return false;

        // If comp has power trader, ensure it's on and electricity isn't off
        var comp = thing.TryGetComp<CompPowerTrader>();
        if (comp != null)
        {
            if (!comp.PowerOn)
                return false;
            if (thing.Map.GameConditionManager.ElectricityDisabled(thing.Map))
                return false;
        }

        // Only check if the computer usage joy type requires chair,
        // just in case some mod removes the requirement from the joy type.
        if (VFE_DefOf.VFE_Play_Computer.requireChair)
        {
            // Make sure there's a sittable edifice
            if (thing.InteractionCell.GetEdifice(thing.Map)?.def.building.isSittable != true)
                return false;
            // Also make sure that we can reserve it
            if (!pawn.CanReserveSittableOrSpot(thing.InteractionCell))
                return false;
        }

        return true;
    }
}