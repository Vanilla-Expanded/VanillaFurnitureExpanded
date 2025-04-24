using System.Diagnostics;
using RimWorld;
using Verse;

namespace VanillaFurnitureEC;

public class CompCauseJoyHediff_Aoe : ThingComp
{
    private static Room TempWorkingRoom = null;

    private CompPowerTrader powerTrader;

    private CompProperties_CauseJoyHediff_Aoe Props => (CompProperties_CauseJoyHediff_Aoe)props;

    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        powerTrader = parent.GetComp<CompPowerTrader>();
    }

    public override void CompTick()
    {
        if (!parent.IsHashIntervalTick(Props.checkInterval))
            return;

        // Make sure the power is on
        if (powerTrader is not { PowerOn: true })
            return;

        if (!parent.SpawnedOrAnyParentSpawned)
            return;

        try
        {
            foreach (var pawn in parent.MapHeld.mapPawns.AllPawnsSpawned)
            {
                if (IsPawnAffected(pawn))
                    GiveOrUpdateHediff(pawn);
                if (pawn.carryTracker.CarriedThing is Pawn otherPawn && IsPawnAffected(otherPawn))
                    GiveOrUpdateHediff(otherPawn);
            }
        }
        finally
        {
            TempWorkingRoom = null;
        }
    }

    private void GiveOrUpdateHediff(Pawn target)
    {
        var hediff = target.health.hediffSet.GetFirstHediffOfDef(Props.hediff);
        if (hediff == null)
        {
            hediff = target.health.AddHediff(Props.hediff);
            hediff.Severity = 1f;
        }

        if (hediff is HediffWithComps hediffWithComps)
        {
            var causeJoy = hediffWithComps.GetComp<HediffComp_CauseJoy>();
            if (causeJoy == null)
            {
                Log.ErrorOnce($"{nameof(CompCauseJoyHediff_Aoe)} has a hediff in props which does not have a {nameof(HediffComp_CauseJoy)}", 1723408196);
            }
            else
            {
                var joyGainRate = parent.GetStatValue(StatDefOf.JoyGainFactor);
                // If joy gain rate is bigger or equal, refresh hediff
                if (joyGainRate >= causeJoy.joyGainRate)
                {
                    causeJoy.joyGainRate = joyGainRate;
                    causeJoy.joyKind = parent.def.building.joyKind;
                    
                    var disappears = hediffWithComps.GetComp<HediffComp_Disappears>();
                    if (disappears == null)
                        Log.ErrorOnce($"{nameof(CompCauseJoyHediff_Aoe)} has a hediff in props which does not have a {nameof(HediffComp_Disappears)}", 808055567);
                    else
                        disappears.ticksToDisappear = Props.checkInterval + 5;
                }
            }
        }
        else Log.ErrorOnce($"{nameof(CompCauseJoyHediff_Aoe)} has a hediff in props which is not {nameof(HediffWithComps)}", -837742526);
    }

    private bool IsPawnAffected(Pawn target)
    {
        // All check are done in specific order, from the fastest check to slowest (based on my small benchmark).

        // If there's a power trader, make sure it's powered
        if (powerTrader is { PowerOn: false })
            return false;

        // Make sure the pawn cares about joy
        if (target.needs.joy == null)
            return false;

        // Make sure the pawn is alive, not asleep, and can receive a hediff
        if (target.health == null || target.Dead || !target.Awake())
            return false;

        // Make sure that the pawn can hear
        if (!target.health.capacities.CapableOf(PawnCapacityDefOf.Hearing))
            return false;

        // Make sure pawn is in range
        // Using squared distance is faster than calculating a square root, and can be safely used to compare distances.
        if (target.PositionHeld.DistanceToSquared(parent.PositionHeld) > Props.range * Props.range)
            return false;

        // Make sure the pawn is in the same room
        return target.GetRoom() == (TempWorkingRoom ??= parent.GetRoom());
    }

    private void Benchmark(Pawn target)
    {
        Log.Error("$Doing benchmark.");
        var stopwatch = Stopwatch.StartNew();
        for (var i = 0; i < 10000000; i++)
        {
            _ = powerTrader is { PowerOn: false };
        }
        stopwatch.Stop();
        Log.Error($"Power benchmark: {stopwatch.Elapsed.TotalMilliseconds}");
        stopwatch.Restart();
        for (var i = 0; i < 10000000; i++)
        {
            _ = target.Dead;
        }
        stopwatch.Stop();
        stopwatch.Restart();
        for (var i = 0; i < 10000000; i++)
        {
            _ = target.health == null;
        }
        stopwatch.Stop();
        Log.Error($"Null health benchmark: {stopwatch.Elapsed.TotalMilliseconds}");
        stopwatch.Restart();
        for (var i = 0; i < 10000000; i++)
        {
            _ = target.health.capacities.CapableOf(PawnCapacityDefOf.Hearing);
        }
        stopwatch.Stop();
        Log.Error($"Hearing benchmark: {stopwatch.Elapsed.TotalMilliseconds}");
        stopwatch.Restart();
        for (var i = 0; i < 10000000; i++)
        {
            _ = target.GetRoom() != (TempWorkingRoom ??= parent.GetRoom());
        }
        stopwatch.Stop();
        Log.Error($"GetRoom benchmark: {stopwatch.Elapsed.TotalMilliseconds}");
        stopwatch.Restart();
        for (var i = 0; i < 10000000; i++)
        {
            _ = target.PositionHeld.DistanceTo(parent.PositionHeld) <= Props.range;
        }
        stopwatch.Stop();
        Log.Error($"Distance benchmark: {stopwatch.Elapsed.TotalMilliseconds}");
        stopwatch.Restart();
        for (var i = 0; i < 10000000; i++)
        {
            _ = target.PositionHeld.DistanceToSquared(parent.PositionHeld) <= Props.range * Props.range;
        }
        stopwatch.Stop();
        Log.Error($"Squared benchmark: {stopwatch.Elapsed.TotalMilliseconds}");
        var rangeSquared = Props.range * Props.range;
        stopwatch.Restart();
        for (var i = 0; i < 10000000; i++)
        {
            _ = target.PositionHeld.DistanceToSquared(parent.PositionHeld) <= rangeSquared;
        }
        stopwatch.Stop();
        Log.Error($"Squared cached benchmark: {stopwatch.Elapsed.TotalMilliseconds}");
        stopwatch.Restart();
        for (var i = 0; i < 10000000; i++)
        {
            _ = target.needs.joy == null;
        }
        stopwatch.Stop();
        Log.Error($"Joy benchmark: {stopwatch.Elapsed.TotalMilliseconds}");
        stopwatch.Restart();
        for (var i = 0; i < 10000000; i++)
        {
            _ = target.RaceProps.Humanlike;
        }
        stopwatch.Stop();
        Log.Error($"Humanlike benchmark: {stopwatch.Elapsed.TotalMilliseconds}");
        stopwatch.Restart();
        for (var i = 0; i < 10000000; i++)
        {
            _ = target.Awake();
        }
        stopwatch.Stop();
        Log.Error($"Awake benchmark: {stopwatch.Elapsed.TotalMilliseconds}");
        Log.Error("Benchmark ended.");
    }
}