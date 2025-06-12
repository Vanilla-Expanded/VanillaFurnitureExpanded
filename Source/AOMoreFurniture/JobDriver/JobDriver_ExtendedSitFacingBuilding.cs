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
        // If no joy from cell, replace the original tickAction.
        // We could always replace it, but I feel that doing it this
        // way is more compatible in case the vanilla JobDriver is
        // updated, and we forget to update this method.
        if (joyData is { allowComfortFromCell: false })
        {
            toil.tickAction = () =>
            {
                // Same tickAction as original, but missing comfort gain.
                // Perhaps a reverse harmony patch would be better to copy the original method?
                // Honestly, feels like too much work for such a simple thing.
                pawn.rotationTracker.FaceTarget(TargetA);
                JoyUtility.JoyTickCheckEnd(pawn, 1,job.doUntilGatheringEnded ? JoyTickFullJoyAction.None : JoyTickFullJoyAction.EndJob, 1f, (Building)TargetThingA);
            };
        }

        // Play a sound every couple of ticks, both sound and delay specified by the 
        toil.tickAction += () =>
        {
            if (joyData == null)
                return;

            if (joyData.sound != null && Find.TickManager.TicksGame >= nextSoundTick)
            {
                nextSoundTick = Find.TickManager.TicksGame + joyData.soundRefireDelay.RandomInRange;
                joyData.sound.PlayOneShot(new TargetInfo(TargetThingA));
            }

            if (joyData.extraJoySkill != null)
                pawn.skills.GetSkill(joyData.extraJoySkill).Learn(joyData.extraJoyXpPerTick);
        };

        // Add research points when finished the job.
        toil.AddFinishAction(() =>
        {
            if (joyData == null || joyData.researchOnFinished <= 0)
                return;

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
        });

        // Display effecter for stuff like the chips from roulette table
        toil.WithEffect(() => joyData?.effecter, () => TargetA.Thing.OccupiedRect().ClosestCellTo(pawn.Position));
        // Fail if the target has a power trader and no power
        toil.FailOn(() => powerTrader is { PowerOn: false });
    }

    public override void ExposeData()
    {
        if (Scribe.mode == LoadSaveMode.PostLoadInit)
        {
            joyData = TargetA.Thing.def.GetModExtension<ExtendedSitFacingJoyDataExtension>();
            powerTrader = TargetA.Thing.TryGetComp<CompPowerTrader>();
        }

        // Call to base needs to be done after we've done our PostLoadInit
        base.ExposeData();

        Scribe_Values.Look(ref nextSoundTick, nameof(nextSoundTick));
    }
}