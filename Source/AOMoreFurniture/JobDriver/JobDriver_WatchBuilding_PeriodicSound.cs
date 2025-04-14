// using RimWorld;
// using Verse;
// using Verse.Sound;
//
// namespace VanillaFurnitureEC;
//
// public abstract class JobDriver_WatchBuilding_PeriodicSound : JobDriver_WatchBuilding
// {
//     protected int nextSoundTick;
//     protected SoundDef sound;
//
//     public override void Notify_Starting()
//     {
//         base.Notify_Starting();
//
//         nextSoundTick = Find.TickManager.TicksGame + Rand.RangeInclusive(100, 500);
//         InitializeSound();
//     }
//
//     protected override void WatchTickAction()
//     {
//         base.WatchTickAction();
//
//         if (nextSoundTick < Find.TickManager.TicksGame)
//         {
//             nextSoundTick = Find.TickManager.TicksGame + 400 + Rand.RangeInclusive(0, 100);
//             RandomPeriodicTick();
//         }
//     }
//     
//     public override void ExposeData()
//     {
//         base.ExposeData();
//
//         Scribe_Values.Look(ref nextSoundTick, nameof(nextSoundTick));
//
//         if (Scribe.mode == LoadSaveMode.PostLoadInit)
//             InitializeSound();
//     }
//
//     protected virtual void RandomPeriodicTick()
//     {
//         sound?.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map));
//     }
//
//     protected virtual void InitializeSound()
//     {
//         sound = 
//     }
// }