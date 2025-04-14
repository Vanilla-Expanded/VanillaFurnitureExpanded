using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaFurnitureEC;

public class Graphic_IndexedPadded : Graphic_Indexed
{
    // protected override Type SingleGraphicType => typeof(Graphic_SinglePadded);

    // public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
    // {
    //     base.DrawWorker(loc, rot, thingDef, thing, extraRotation);
    //     
    //     Log.ErrorOnce($"Test draw", 12121);
    // }
    //
    public override void Print(SectionLayer layer, Thing thing, float extraRotation)
    {
        throw new Exception("The method or operation is not implemented.");

        var material = MatAt(thing.Rotation, thing);
        TryGetTextureAtlasReplacementInfo(material, thing.def.category.ToAtlasGroup(), false, true, out var a, out var b, out var c);
        Log.ErrorOnce($"{b.ToStringSafeEnumerable()}", 128763);
        Printer_Plane.PrintPlane(layer, thing.TrueCenter(), new Vector2(1f, 1f), material, extraRotation);
        ShadowGraphic?.Print(layer, thing, 0f);
    }
}