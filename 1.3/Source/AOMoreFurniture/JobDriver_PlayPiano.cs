using RimWorld;
using Verse;
using Verse.AI;

namespace AOMoreFurniture
{
    internal class JobDriver_PlayPiano : JobDriver_SitFacingBuilding
    {
        protected override void ModifyPlayToil(Toil toil)
        {
            base.ModifyPlayToil(toil);
            ToilEffects.WithEffect(toil, () => EffecterDefOf.Joy_PlayPiano, () => base.TargetA.Thing.OccupiedRect().ClosestCellTo(this.pawn.Position));
        }
    }
}