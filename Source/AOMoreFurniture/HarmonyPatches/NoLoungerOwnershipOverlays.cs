using System.Linq;
using HarmonyLib;
using RimWorld;

namespace VanillaFurnitureEC;

[HarmonyPatch(typeof(CompAssignableToPawn), nameof(CompAssignableToPawn.PlayerCanSeeAssignments), MethodType.Getter)]
public static class NoLoungerOwnershipOverlays
{
    // Only display the overlay if class is not CompAssignableToPawn_Lounger or there's any assigned pawn.
    public static bool Prefix(CompAssignableToPawn __instance) => __instance.AssignedPawns.Any() || !StaticCollections.noUnownedBedOverlayDefs.Contains(__instance.parent.def);
}