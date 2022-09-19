using RimWorld;
using Verse;
using Verse.Sound;

namespace VanillaFurnitureEC
{
    public class JobDriver_PlayArcadeSounds : JobDriver_WatchBuilding
    {
        protected override void WatchTickAction()
        {
            if (pawn.IsHashIntervalTick(400 + Rand.RangeInclusive(0, 100)))
            {
                switch (Rand.Range(0, 4))
                {
                    case 1:
                        SoundDefOf.Arcade_SFXTwo.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map, false));
                        break;

                    case 2:
                        SoundDefOf.Arcade_SFXThree.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map, false));
                        break;

                    case 3:
                        SoundDefOf.Arcade_SFXFour.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map, false));
                        break;

                    default:
                        SoundDefOf.Arcade_SFXOne.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map, false));
                        break;
                }
            }
            base.WatchTickAction();
        }
    }
}