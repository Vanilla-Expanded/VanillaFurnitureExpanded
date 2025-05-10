using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace VanillaFurnitureEC;

public class JobDriver_ComputerLearning : JobDriver
{
    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        return pawn.Reserve(TargetA, job, errorOnFailed: errorOnFailed);
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
        this.FailOnDespawnedOrNull(TargetIndex.A);
        this.FailOnSomeonePhysicallyInteracting(TargetIndex.A);
        this.FailOnChildLearningConditions();
        this.FailOn(() => !LearningGiver_ComputerUsing.CanUseComputerNow(TargetThingA, pawn));

        yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.InteractionCell)
            .FailOn(() => !LearningGiver_ComputerUsing.CanUseComputerNow(TargetThingA, pawn));

        var computerUseToil = ToilMaker.MakeToil();
        computerUseToil.tickAction = () =>
        {
            pawn.GainComfortFromCellIfPossible();
            pawn.rotationTracker.FaceTarget(TargetA);
            LearningUtility.LearningTickCheckEnd(pawn);
            pawn.skills.Learn(SkillDefOf.Intellectual, VFE_DefOf.VFE_ComputerLearning.xpPerTick);
        };
        computerUseToil.handlingFacing = true;
        computerUseToil.defaultCompleteMode = ToilCompleteMode.Never;
        yield return computerUseToil;
    }
}