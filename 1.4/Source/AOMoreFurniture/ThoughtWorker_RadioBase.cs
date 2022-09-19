using RimWorld;
using Verse;

namespace VanillaFurnitureEC
{
    internal class ThoughtWorker_RadioBase : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (!p.Spawned)
                return false;

            var spacerRadios = p.Map.listerThings.ThingsOfDef(ThingDefOf.Radio_Spacer);
            for (int r = 0; r < spacerRadios.Count; r++)
            {
                if (ShouldActivateThought(p, spacerRadios[r], 8))
                    return ThoughtState.ActiveAtStage(1);
            }

            var industrialRadios = p.Map.listerThings.ThingsOfDef(ThingDefOf.Radio_Industrial);
            for (int r = 0; r < industrialRadios.Count; r++)
            {
                if (ShouldActivateThought(p, industrialRadios[r], 5))
                    return ThoughtState.ActiveAtStage(0);
            }

            return false;
        }

        private bool ShouldActivateThought(Pawn p, Thing thing, int radius)
        {
            var comp = thing.TryGetComp<CompPowerTrader>();
            if ((comp == null || comp.PowerOn) && p.Position.InHorDistOf(thing.Position, radius))
            {
                return true;
            }

            return false;
        }
    }
}