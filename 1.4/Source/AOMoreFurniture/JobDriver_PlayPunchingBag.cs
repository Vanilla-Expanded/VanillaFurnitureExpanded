using RimWorld;
using System;
using Verse;
using Verse.Sound;

namespace AOMoreFurniture
{
    public class JobDriver_PlayPunchingBag : JobDriver_WatchBuilding
    {
        private const int PunchSoundInterval = 400;

        protected override void WatchTickAction()
        {
            Random random = new Random();
            bool flag = this.pawn.IsHashIntervalTick(400 + random.Next(0, 100) - Convert.ToInt32((float)this.pawn.skills.GetSkill(SkillDefOf.Melee).Level) * 10);
            if (flag)
            {
                int num = random.Next(0, 2);
                int num2 = num;
                if (num2 != 1)
                {
                    RimWorld.SoundDefOf.Pawn_Melee_Punch_HitPawn.PlayOneShot(new TargetInfo(this.pawn.Position, this.pawn.Map, false));
                }
                else
                {
                    RimWorld.SoundDefOf.Pawn_Melee_Punch_Miss.PlayOneShot(new TargetInfo(this.pawn.Position, this.pawn.Map, false));
                }
            }
            base.WatchTickAction();
        }
    }
}