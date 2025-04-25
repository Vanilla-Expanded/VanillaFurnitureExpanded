using HarmonyLib;
using Verse;

namespace VanillaFurnitureEC;

public class VFECore : Mod
{
    public VFECore(ModContentPack content) : base(content)
    {
        var harmony = new Harmony("VanillaFurnitureExpanded.Core");
        harmony.PatchAll();
    }
}