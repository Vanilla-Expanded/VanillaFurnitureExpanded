using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaFurnitureEC
{
    internal class JobDriver_PlayDarts : JobDriver_WatchBuilding
    {
        protected override void WatchTickAction()
        {
            if (pawn.IsHashIntervalTick(400 - Rand.RangeInclusive(0, 100)))
            {
                ThrowDart(pawn, TargetA.Cell);
            }
            base.WatchTickAction();
        }

        private void ThrowDart(Pawn thrower, IntVec3 targetCell)
        {
            Map map = thrower.Map;
            var position = thrower.Position;

            if (position.ShouldSpawnMotesAt(map) || !map.moteCounter.SaturatedLowPriority)
            {
                var vector = targetCell.ToVector3Shifted();
                vector.z += 0.6f;

                var moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefOf.Mote_FlyingDart);
                moteThrown.Scale = 0.5f;
                moteThrown.rotationRate = 0.05f;
                moteThrown.exactPosition = thrower.DrawPos;
                moteThrown.exactRotation = (vector - moteThrown.exactPosition).AngleFlat();
                moteThrown.SetVelocity((vector - moteThrown.exactPosition).AngleFlat(), 5);
                moteThrown.MoveAngle = (vector - moteThrown.exactPosition).AngleFlat();
                moteThrown.airTimeLeft = (moteThrown.exactPosition - vector).MagnitudeHorizontal() / (5.2f);
                GenSpawn.Spawn(moteThrown, position, map, WipeMode.Vanish);
            }
        }
    }
}