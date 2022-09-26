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
                SoundDefOf.Arcade_SFX.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map, false));
            }
            base.WatchTickAction();
        }
    }
}