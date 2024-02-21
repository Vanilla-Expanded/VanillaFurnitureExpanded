using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaFurnitureEC
{
    public class StatPart_Bin : StatPart
    {
        public override string ExplanationPart(StatRequest req)
        {
            var comp = req.Thing?.TryGetComp<CompBinClean>();
            if (comp != null)
            {
                var beautyOffset = BeautyOffset(comp);
                return "VFE.FilthStoredBeauty".Translate(beautyOffset.ToStringWithSign(), comp.AmountStoredPct.ToStringPercent());
            }
            return null;
        }

        public override void TransformValue(StatRequest req, ref float val)
        {
            var comp = req.Thing?.TryGetComp<CompBinClean>();
            if (comp != null)
            {
                val += BeautyOffset(comp);
            }
        }

        public float BeautyOffset(CompBinClean comp)
        {
            return Mathf.RoundToInt(comp.Props.beautyPerFilthStored.Evaluate(comp.AmountStoredPct));
        }
    }
}