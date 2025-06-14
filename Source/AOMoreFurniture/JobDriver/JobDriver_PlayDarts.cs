using Verse;
using Verse.AI;

namespace VanillaFurnitureEC
{
    internal class JobDriver_PlayDarts : JobDriver_ExtendedSitFacingBuilding
    {
        protected override void ModifyPlayToil(Toil toil)
        {
            toil.tickIntervalAction += delta =>
            {
                if (pawn.IsHashIntervalTick(400, delta))
                    ThrowDart(pawn, TargetA.Cell);
            };
        }

        private static void ThrowDart(Pawn thrower, IntVec3 targetCell)
        {
            var map = thrower.Map;
            var position = thrower.Position;

            if (position.ShouldSpawnMotesAt(map) || !map.moteCounter.SaturatedLowPriority)
            {
                var vector = targetCell.ToVector3Shifted();
                vector.z += 0.6f;

                var moteThrown = (MoteThrown)ThingMaker.MakeThing(VFE_DefOf.Mote_FlyingDart);
                moteThrown.Scale = 0.5f;
                moteThrown.rotationRate = 0.05f;
                moteThrown.exactPosition = thrower.DrawPos;
                moteThrown.exactRotation = (vector - moteThrown.exactPosition).AngleFlat();
                moteThrown.SetVelocity((vector - moteThrown.exactPosition).AngleFlat(), 5);
                moteThrown.MoveAngle = (vector - moteThrown.exactPosition).AngleFlat();
                moteThrown.airTimeLeft = (moteThrown.exactPosition - vector).MagnitudeHorizontal() / 5.2f;
                GenSpawn.Spawn(moteThrown, position, map);
            }
        }
    }
}