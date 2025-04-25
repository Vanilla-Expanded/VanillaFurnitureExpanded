using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace VanillaFurnitureEC;

public class JobDriver_UseLounger : JobDriver_LayDown
{
    public bool canSleep = false;

    public override bool CanSleep => canSleep;

    public override bool CanRest => canSleep;

    public override bool LookForOtherJobs => false;

    public override void Notify_Starting()
    {
        base.Notify_Starting();

        canSleep = pawn.needs?.rest?.CurLevelPercentage <= 0.3f;
    }

    public override bool TryMakePreToilReservations(bool errorOnFailed)
        => pawn.Reserve(TargetA, job, job.def.joyMaxParticipants, 0, errorOnFailed: errorOnFailed);

    // Return all the original toils besides "ClaimBedIfNonMedical", as we don't want to claim bed for recreation.
    protected override IEnumerable<Toil> MakeNewToils() => base.MakeNewToils().Where(toil => toil.debugName != nameof(Toils_Bed.ClaimBedIfNonMedical));

    public override Toil LayDownToil(bool hasBed)
    {
        var toil = ToilMaker.MakeToil();

        toil.defaultCompleteMode = ToilCompleteMode.Never;
        toil.FailOn(() => !CanUseBedNow(Bed, pawn));
        toil.FailOn(() => RoofUtility.IsAnyCellUnderRoof(Bed));
        toil.FailOn(() => !JoyUtility.EnjoyableOutsideNow(pawn));
        toil.FailOn(() => GenCelestial.CurCelestialSunGlow(pawn.Map) < 0.65f);

        toil.initAction = () =>
        {
            pawn.pather?.StopDead();
            if (!Bed.OccupiedRect().Contains(pawn.Position))
            {
                Log.Error("Can't start LayDown toil because pawn is not in the bed. pawn=" + pawn);
                pawn.jobs.EndCurrentJob(JobCondition.Errored);
                return;
            }

            // Setup posture
            pawn.jobs.posture = PawnPosture.LayingInBedFaceUp;
            PortraitsCache.SetDirty(pawn);
        };
        toil.tickAction = () =>
        {
            if (!asleep)
            {
                if (CanSleep && RestUtility.CanFallAsleep(pawn) && pawn.ageTracker.CurLifeStage.canVoluntarilySleep)
                {
                    asleep = true;
                    job.startInvoluntarySleep = false;
                }
            }
            else if (!CanSleep || RestUtility.ShouldWakeUp(pawn))
            {
                asleep = false;
            }

            // Our slightly modified ApplyBedRelatedEffects. No bed related thoughts.
            var bed = Bed;
            JoyUtility.JoyTickCheckEnd(pawn, joySource: bed);
            pawn.GainComfortFromCellIfPossible();
            if (asleep && CanRest && pawn.needs.rest != null)
                pawn.needs.rest.TickResting(bed.GetStatValue(StatDefOf.BedRestEffectiveness));

            Thing pawnOrParent;
            if (pawn.IsHashIntervalTick(100) && (pawnOrParent = pawn.SpawnedParentOrMe) != null)
            {
                var map = pawnOrParent.Map;

                if (!pawnOrParent.Position.Fogged(map) && CanSleep)
                {
                    var (fleckDef, velocitySpeed) = pawn.ageTracker.CurLifeStage.developmentalStage switch
                    {
                        DevelopmentalStage.Baby or DevelopmentalStage.Newborn => (FleckDefOf.SleepZ_Tiny, 0.25f),
                        DevelopmentalStage.Child => (FleckDefOf.SleepZ_Small, 0.33f),
                        _ => (FleckDefOf.SleepZ, 0.42f),
                    };

                    FleckMaker.ThrowMetaIcon(pawnOrParent.Position, map, fleckDef, velocitySpeed);
                }
            }

            if (LookForOtherJobs && pawn.IsHashIntervalTick(211))
                pawn.jobs.CheckForJobOverride();
        };
        toil.AddFinishAction(() =>
        {
            var bed = Bed;
            if (pawn.GetPosture().InBed() && !CanUseBedNow(bed, pawn))
                RestUtility.KickOutOfBed(pawn, bed);
        });

        return toil;
    }

    public override string GetReport()
    {
        var reportStringOverride = GetReportStringOverride();
        if (!reportStringOverride.NullOrEmpty())
            return reportStringOverride;
        if (asleep)
            "VFE_SleepingWhileSunbathing".Translate();
        return job.def.reportString;
    }

    public override void ExposeData()
    {
        base.ExposeData();

        Scribe_Values.Look(ref canSleep, nameof(canSleep));
    }

    public static bool CanUseBedNow(Thing bedThing, Pawn sleeper)
    {
        // Modified RestUtility.CanUseBedNow to work with lounger use
        
        if (bedThing is not Building_Bed bed)
            return false;

        if (!bed.Spawned)
            return false;

        if (bed.Map != sleeper.MapHeld)
            return false;

        if (bed.IsBurning())
            return false;

        if (!RestUtility.CanUseBedEver(sleeper, bed.def))
            return false;

        if (bed.CompAssignableToPawn.IdeoligionForbids(sleeper))
            return false;

        var isOwner = bed.IsOwner(sleeper, out var ownedBedSlot);
        var isSameBed = sleeper.CurrentBed(out var currentBedSlot) == bed;
        if (!bed.AnyUnoccupiedSleepingSlot && !isOwner && !isSameBed)
            return false;

        switch (sleeper.GuestStatus)
        {
            case GuestStatus.Prisoner when !bed.ForPrisoners:
            case GuestStatus.Slave when !bed.ForSlaves:
                return false;
        }

        if (bed.Medical)
            return false;

        if (!isOwner && !RestUtility.BedOwnerWillShare(bed, sleeper, sleeper.GuestStatus))
            return false;

        // If bed is owned by this pawn, only allow using it if they're using their assigned bed slot
        if (isSameBed && ownedBedSlot != null && ownedBedSlot != currentBedSlot)
            return false;

        if (sleeper.IsColonist && sleeper.CurJob is not { ignoreForbidden: true } && !sleeper.Downed && bed.IsForbidden(sleeper))
            return false;

        return true;
    }
}