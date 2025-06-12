using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace VanillaFurnitureEC;

public class JobDriver_ComputerLearning : JobDriver
{
    protected ExtendedSitFacingJoyDataExtension joyData;
    protected int nextSoundTick;
    
    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        return pawn.Reserve(TargetA, job, errorOnFailed: errorOnFailed) && pawn.ReserveSittableOrSpot(TargetThingA.InteractionCell, job, errorOnFailed: errorOnFailed);
    }

    public override void Notify_Starting()
    {
        base.Notify_Starting();

        joyData = TargetA.Thing.def.GetModExtension<ExtendedSitFacingJoyDataExtension>();
        // Initial sound delay, this one is between 0 and max
        if (joyData?.sound != null)
            nextSoundTick = Find.TickManager.TicksGame + Rand.RangeInclusive(0, joyData.soundRefireDelay.max);
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
            pawn.GainComfortFromCellIfPossible(1);
            pawn.rotationTracker.FaceTarget(TargetA);
            LearningUtility.LearningTickCheckEnd(pawn,1);
            pawn.skills.Learn(SkillDefOf.Intellectual, VFE_DefOf.VFE_ComputerLearning.xpPerTick);

            if (joyData is { sound: not null } && Find.TickManager.TicksGame >= nextSoundTick)
            {
                nextSoundTick = Find.TickManager.TicksGame + joyData.soundRefireDelay.RandomInRange;
                joyData.sound.PlayOneShot(new TargetInfo(TargetThingA));
            }
        };
        computerUseToil.handlingFacing = true;
        computerUseToil.defaultCompleteMode = ToilCompleteMode.Never;
        yield return computerUseToil;
    }

    public override void ExposeData()
    {
        if (Scribe.mode == LoadSaveMode.PostLoadInit)
            joyData = TargetA.Thing.def.GetModExtension<ExtendedSitFacingJoyDataExtension>();

        // Call to base needs to be done after we've done our PostLoadInit
        base.ExposeData();

        Scribe_Values.Look(ref nextSoundTick, nameof(nextSoundTick));
    }
}