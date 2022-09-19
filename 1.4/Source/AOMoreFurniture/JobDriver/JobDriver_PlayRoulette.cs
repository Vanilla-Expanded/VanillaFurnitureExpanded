using RimWorld;
using Verse;
using Verse.AI;

namespace VanillaFurnitureEC
{
    internal class JobDriver_PlayRoulette : JobDriver_SitFacingBuilding
    {
        protected override void ModifyPlayToil(Toil toil)
        {
            base.ModifyPlayToil(toil);
            ToilEffects.WithEffect(toil, () => EffecterDefOf.Joy_HoldChips, () => TargetA.Thing.OccupiedRect().ClosestCellTo(pawn.Position));
        }
    }
}