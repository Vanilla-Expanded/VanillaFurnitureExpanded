using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace VanillaFurnitureEC
{
    public class JobDriver_CleanBin : JobDriver
    {
        private float workDone;

        private float totalWork;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(job.GetTarget(TargetIndex.A), job);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            var clean = ToilMaker.MakeToil();
            clean.initAction = () =>
            {
                workDone = 0f;
                totalWork = 150f;
            };
            clean.tickIntervalAction = delta =>
            {
                workDone += delta;
                if (workDone >= totalWork)
                {
                    var bin = job.GetTarget(TargetIndex.A).Thing;
                    var comp = bin.TryGetComp<CompBinClean>();
                    clean.actor.records.Increment(RecordDefOf.MessesCleaned);
                    comp.innerContainer.ClearAndDestroyContents();
                    ReadyForNextToil();
                }
            };
            clean.defaultCompleteMode = ToilCompleteMode.Never;
            clean.WithEffect(EffecterDefOf.Clean, TargetIndex.A);
            clean.WithProgressBar(TargetIndex.A, () => workDone / totalWork, interpolateBetweenActorAndTarget: true);
            clean.PlaySustainerOrSound(() => SoundDefOf.Interact_CleanFilth);
            yield return clean;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref workDone, "workDone", 0f);
            Scribe_Values.Look(ref totalWork, "totalWork", 150f);
        }
    }
}