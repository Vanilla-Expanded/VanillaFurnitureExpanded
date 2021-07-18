using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace AOMoreFurniture
{
    internal class JobDriver_PlayDarts : JobDriver_WatchBuilding
    {
        private const int throwDartInterval = 400;

        public static void ThrowDart(Pawn thrower, IntVec3 targetCell)
        {
            bool flag = thrower.Position.ShouldSpawnMotesAt(thrower.Map) || !thrower.Map.moteCounter.SaturatedLowPriority;
            if (flag)
            {
                float num = 5f;
                Vector3 vector = targetCell.ToVector3Shifted();
                vector.z += 0.6f;
                ThingDef mote_FlyingDart = ThingDefOf.Mote_FlyingDart;
                MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(mote_FlyingDart, null);
                moteThrown.Scale = 0.5f;
                moteThrown.rotationRate = 0.05f;
                moteThrown.exactPosition = thrower.DrawPos;
                moteThrown.exactRotation = (vector - moteThrown.exactPosition).AngleFlat();
                moteThrown.SetVelocity((vector - moteThrown.exactPosition).AngleFlat(), num);
                moteThrown.MoveAngle = (vector - moteThrown.exactPosition).AngleFlat();
                moteThrown.airTimeLeft = (moteThrown.exactPosition - vector).MagnitudeHorizontal() / (num + 0.2f);
                GenSpawn.Spawn(moteThrown, thrower.Position, thrower.Map, WipeMode.Vanish);
            }
        }

        protected override void WatchTickAction()
        {
            bool flag = this.pawn.IsHashIntervalTick(400 - Convert.ToInt32((float)this.pawn.skills.GetSkill(SkillDefOf.Shooting).Level) * 10);
            bool flag2 = flag;
            if (flag2)
            {
                JobDriver_PlayDarts.ThrowDart(this.pawn, base.TargetA.Cell);
            }
            base.WatchTickAction();
        }
    }
}