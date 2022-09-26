using RimWorld;
using Verse;
using Verse.Sound;

namespace VanillaFurnitureEC
{
    public class JobDriver_PlayComputerModern : JobDriver_WatchBuilding
    {
        protected override void WatchTickAction()
        {
            if (pawn.IsHashIntervalTick(400 + Rand.RangeInclusive(0, 100)))
            {
                Hediff learningBoost = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Computer_LearningBoost, false);
                if (learningBoost == null)
                {
                    pawn.health.AddHediff(HediffDefOf.Computer_LearningBoost);
                }
                else
                {
                    learningBoost.Severity += 0.08f;
                }

                SoundDefOf.Computer_SFX.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map, false));
            }
            base.WatchTickAction();
        }
    }
}