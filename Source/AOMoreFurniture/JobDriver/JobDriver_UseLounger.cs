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

    public override Toil LayDownToil(bool hasBed)
    {
        var toil = Toils_LayDown.LayDown(TargetIndex.A, hasBed, LookForOtherJobs, CanSleep, CanRest, PawnPosture.LayingInBedFaceUp);

        toil.tickAction += () => JoyUtility.JoyTickCheckEnd(pawn, joySource: TargetThingA as Building);
        toil.defaultDuration = job.def.joyDuration;
        toil.FailOn(() => pawn.Position.Roofed(pawn.Map));
        toil.FailOn(() => !JoyUtility.EnjoyableOutsideNow(pawn));
        toil.FailOn(() => GenCelestial.CurCelestialSunGlow(pawn.Map) < 0.65f);

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
}