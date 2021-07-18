using RimWorld;
using Verse;
using Verse.AI;

namespace AOMoreFurniture
{
    internal class JobDriver_PlayRoulette : JobDriver_SitFacingBuilding
    {
        protected override void ModifyPlayToil(Toil toil)
        {
            base.ModifyPlayToil(toil);
            ToilEffects.WithEffect(toil, () => EffecterDefOf.Joy_HoldChips, () => base.TargetA.Thing.OccupiedRect().ClosestCellTo(this.pawn.Position));
        }
    }
}