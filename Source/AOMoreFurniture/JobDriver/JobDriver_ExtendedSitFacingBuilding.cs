using RimWorld;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace VanillaFurnitureEC;

public class JobDriver_ExtendedSitFacingBuilding : JobDriver_SitFacingBuilding
{
    protected ExtendedSitFacingJoyDataExtension joyData;
    protected CompPowerTrader powerTrader;
    protected int nextSoundTick;

    public override void Notify_Starting()
    {
        base.Notify_Starting();

        joyData = TargetA.Thing.def.GetModExtension<ExtendedSitFacingJoyDataExtension>();
        powerTrader = TargetA.Thing.TryGetComp<CompPowerTrader>();
        // Initial sound delay, this one is between 0 and max
        if (joyData?.sound != null)
            nextSoundTick = Find.TickManager.TicksGame + Rand.RangeInclusive(0, joyData.soundRefireDelay.max);
    }

    protected override void ModifyPlayToil(Toil toil)
    {
        // Play a sound every couple of ticks, both sound and delay specified by the 
        toil.tickAction += () =>
        {
            if (joyData?.sound == null || Find.TickManager.TicksGame < nextSoundTick)
                return;

            nextSoundTick = Find.TickManager.TicksGame + joyData.soundRefireDelay.RandomInRange;
            joyData.sound.PlayOneShot(new TargetInfo(TargetThingA));
        };

        // Add research points when finished the job.
        toil.AddFinishAction(() =>
        {
            // Make sure that we're supposed to add research and that the job was finished.
            // I've tried adding extra toil, but it did not trigger if added after the original last one.
            if (joyData != null && (pawn.jobs.curDriver.ticksLeftThisToil <= 0 || pawn.needs.joy.CurLevelPercentage >= 0.99f))
            {
                var project = Find.ResearchManager?.GetProject();
                if (project != null)
                {
                    // Total, multiplied by the percentage of the job that was finished
                    var amount = joyData.researchOnFinished * (job.def.joyDuration - pawn.jobs.curDriver.ticksLeftThisToil) / job.def.joyDuration;
                    if (amount > 0)
                    {
                        if (pawn != null)
                        {
                            if (pawn.Faction != null)
                                amount /= project.CostFactor(pawn.Faction.def.techLevel);
                            pawn.records.AddTo(RecordDefOf.ResearchPointsResearched, amount);
                        }
                        Find.ResearchManager.AddProgress(project, amount, pawn);
                    }
                }
            }
        });

        // Display effecter for stuff like the chips from roulette table
        toil.WithEffect(() => joyData?.effecter, () => TargetA.Thing.OccupiedRect().ClosestCellTo(pawn.Position));
        // Fail if the target has a power trader and no power
        toil.FailOn(() => powerTrader is { PowerOn: false });
    }

    public override void ExposeData()
    {
        base.ExposeData();

        Scribe_Values.Look(ref nextSoundTick, nameof(nextSoundTick));

        if (Scribe.mode != LoadSaveMode.PostLoadInit)
            return;

        joyData = TargetA.Thing.def.GetModExtension<ExtendedSitFacingJoyDataExtension>();
        powerTrader = TargetA.Thing.TryGetComp<CompPowerTrader>();
    }
}