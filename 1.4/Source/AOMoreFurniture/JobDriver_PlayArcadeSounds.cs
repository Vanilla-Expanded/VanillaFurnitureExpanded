using RimWorld;
using System;
using Verse;
using Verse.Sound;

namespace AOMoreFurniture
{
    public class JobDriver_PlayArcadeSounds : JobDriver_WatchBuilding
    {
        private const int ArcadeSoundInterval = 200;

        protected override void WatchTickAction()
        {
            Random random = new Random();
            bool flag = this.pawn.IsHashIntervalTick(400 + random.Next(0, 100));
            if (flag)
            {
                switch (random.Next(0, 4))
                {
                    case 1:
                        SoundDefOf.Arcade_SFXTwo.PlayOneShot(new TargetInfo(this.pawn.Position, this.pawn.Map, false));
                        break;

                    case 2:
                        SoundDefOf.Arcade_SFXThree.PlayOneShot(new TargetInfo(this.pawn.Position, this.pawn.Map, false));
                        break;

                    case 3:
                        SoundDefOf.Arcade_SFXFour.PlayOneShot(new TargetInfo(this.pawn.Position, this.pawn.Map, false));
                        break;

                    default:
                        SoundDefOf.Arcade_SFXOne.PlayOneShot(new TargetInfo(this.pawn.Position, this.pawn.Map, false));
                        break;
                }
            }
            base.WatchTickAction();
        }
    }
}