using RimWorld;
using Verse;
using Verse.AI;

namespace VanillaFurnitureEC
{
    internal class JobDriver_PlayPiano : JobDriver_SitFacingBuilding
    {
        protected override void ModifyPlayToil(Toil toil)
        {
            base.ModifyPlayToil(toil);
            ToilEffects.WithEffect(toil, () => EffecterDefOf.Joy_PlayPiano, () => TargetA.Thing.OccupiedRect().ClosestCellTo(pawn.Position));
        }
    }
}