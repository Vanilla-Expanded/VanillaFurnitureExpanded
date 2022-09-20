using RimWorld;
using Verse;
using Verse.Sound;

namespace VanillaFurnitureEC
{
    public class JobDriver_PlayPunchingBag : JobDriver_WatchBuilding
    {
        private const int PunchSoundInterval = 400;

        protected override void WatchTickAction()
        {
            if (pawn.IsHashIntervalTick(400 + Rand.Range(0, 100)))
            {
                if (Rand.Bool)
                {
                    RimWorld.SoundDefOf.Pawn_Melee_Punch_HitPawn.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map, false));
                }
                else
                {
                    RimWorld.SoundDefOf.Pawn_Melee_Punch_Miss.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map, false));
                }
            }
            base.WatchTickAction();
        }
    }
}