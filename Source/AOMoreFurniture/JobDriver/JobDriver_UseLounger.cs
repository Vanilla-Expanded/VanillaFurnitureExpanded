using System;
using RimWorld;
using Verse;
using Verse.AI;

namespace VanillaFurnitureEC;

public class JobDriver_UseLounger : JobDriver_LayDown
{
    public override bool CanSleep => true;

    public override bool CanRest => true;

    public override bool LookForOtherJobs => false;

    public override bool TryMakePreToilReservations(bool errorOnFailed)
        => pawn.Reserve(TargetA, job, job.def.joyMaxParticipants, 0, errorOnFailed: errorOnFailed);

    public override Toil LayDownToil(bool hasBed)
    {
        var toil = base.LayDownToil(hasBed);

        toil.tickAction += () => JoyUtility.JoyTickCheckEnd(pawn, joySource: TargetThingA as Building);
        toil.defaultDuration = job.def.joyDuration;
        toil.FailOn(() => pawn.Position.Roofed(pawn.Map));
        toil.FailOn(() => !JoyUtility.EnjoyableOutsideNow(pawn));
        toil.FailOn(() => GenCelestial.CurCelestialSunGlow(pawn.Map) < 0.65f);

        return toil;
    }

    public override string GetReport()
    {
        throw new NotImplementedException("Lounger is not fully implemented yet");
    }
}