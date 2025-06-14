using System.Collections.Generic;
using Verse;

namespace VanillaFurnitureEC;

public class HediffCompProperties_CauseJoy : HediffCompProperties
{
    public List<JobDef> jobDefsDisablingRecreation;

    public HediffCompProperties_CauseJoy() => compClass = typeof(HediffComp_CauseJoy);
}