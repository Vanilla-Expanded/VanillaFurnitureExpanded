using RimWorld;
using System;
using Verse;
using Verse.Sound;

namespace AOMoreFurniture
{
    public class JobDriver_PlayComputerModern : JobDriver_WatchBuilding
    {
        protected override void WatchTickAction()
        {
            Random random = new Random();
            bool flag = this.pawn.IsHashIntervalTick(400 + random.Next(0, 100));
            if (flag)
            {
                Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Computer_LearningBoost, false);
                bool flag2 = firstHediffOfDef == null;
                if (flag2)
                {
                    this.pawn.health.AddHediff(HediffDefOf.Computer_LearningBoost, null, null, null);
                }
                else
                {
                    firstHediffOfDef.Severity += 0.08f;
                }
                SoundDefOf.Computer_SFXOne.PlayOneShot(new TargetInfo(this.pawn.Position, this.pawn.Map, false));
            }
            base.WatchTickAction();
        }
    }
}