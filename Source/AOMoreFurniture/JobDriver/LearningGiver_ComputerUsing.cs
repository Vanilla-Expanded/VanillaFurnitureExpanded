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
        if (!thing.Spawned)
            return false;

        if (!pawn.CanReserve(thing))
            return false;

        var comp = thing.TryGetComp<CompPowerTrader>();
        if (comp != null)
        {
            if (!comp.PowerOn)
                return false;
            if (thing.Map.GameConditionManager.ElectricityDisabled(thing.Map))
                return false;
        }

        if (VFE_DefOf.VFE_Play_Computer.requireChair && thing.InteractionCell.GetEdifice(thing.Map)?.def.building.isSittable != true)
            return false;

        if (thing.IsForbidden(pawn))
            return false;

        return true;
    }
}