using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaFurnitureEC
{
    public class Command_CleanupTarget : Gizmo
    {
        public CompBinClean comp;

        public override float GetWidth(float maxWidth)
        {
            return 160;
        }

        public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
        {
            Rect rect = new Rect(topLeft.x, topLeft.y, GetWidth(maxWidth), 75f);
            Widgets.DrawWindowBackground(rect);
            var labelRect = new Rect(rect.x + 10, rect.y, rect.width - 20, 48);
            Text.Anchor = TextAnchor.MiddleLeft;
            Widgets.Label(labelRect, "VFE.SetTargetCleanup".Translate(comp.cleanupTarget.ToStringPercent()));
            Text.Anchor = TextAnchor.UpperLeft;
            var sliderRect = new Rect(labelRect.x, labelRect.yMax, labelRect.width, 24);
            comp.cleanupTarget = Widgets.HorizontalSlider_NewTemp(sliderRect, comp.cleanupTarget, 0, 1);
            return new GizmoResult(GizmoState.Clear);
        }
    }
}