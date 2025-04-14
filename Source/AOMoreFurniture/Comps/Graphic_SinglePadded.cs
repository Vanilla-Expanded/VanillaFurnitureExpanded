using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace VanillaFurnitureEC;

public class Graphic_SinglePadded : Graphic_Single
{
    private const float TexPadding = 0.03125f;

    public override void Init(GraphicRequest req)
    {
        base.Init(req);

        // Log.ErrorOnce($"Padding textures by {TexPadding}", 12312335);
        mat.mainTextureOffset = new Vector2(TexPadding, TexPadding);
        mat.mainTextureScale = new Vector2(1 - (2 * TexPadding), 1 - (2 * TexPadding));

        // var type = GenTypes.AllTypes.FirstOrDefault(x => x.Name == "MaterialAllocator" && x.Namespace == "Verse");
        // var method = type.GetMethod("Create", [typeof(Material)]);
    }
}