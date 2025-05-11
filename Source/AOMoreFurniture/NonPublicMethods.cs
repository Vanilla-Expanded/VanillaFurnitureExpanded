using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaFurnitureEC;

[StaticConstructorOnStartup]
public static class NonPublicMethods
{
    public static EverPossibleToWatchFromDelegate WatchBuildingUtility_EverPossibleToWatchFrom =
        AccessTools.MethodDelegate<EverPossibleToWatchFromDelegate>(AccessTools.DeclaredMethod(typeof(WatchBuildingUtility), "EverPossibleToWatchFrom"));

    public delegate bool EverPossibleToWatchFromDelegate(IntVec3 watchCell, IntVec3 buildingCenter, Map map, bool bedAllowed, ThingDef def);
}