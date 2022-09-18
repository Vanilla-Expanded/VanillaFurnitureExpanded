using RimWorld;
using UnityEngine;
using Verse;

namespace AOMoreFurniture
{
    internal class Building_ACUnit : Building_TempControl
    {
        public override void TickRare()
        {
            bool powerOn = this.compPowerTrader.PowerOn;
            if (powerOn)
            {
                IntVec3 intVec = base.Position + IntVec3.South.RotatedBy(base.Rotation), c = intVec;
                if (base.Rotation == Rot4.East)
                {
                    c = new IntVec3(intVec.x, intVec.y, intVec.z - 1);
                }
                else if (base.Rotation == Rot4.West)
                {
                    c = new IntVec3(intVec.x, intVec.y, intVec.z + 1);
                }
                else if (base.Rotation == Rot4.North)
                {
                    c = new IntVec3(intVec.x + 1, intVec.y, intVec.z);
                }
                else if (base.Rotation == Rot4.South)
                {
                    c = new IntVec3(intVec.x - 1, intVec.y, intVec.z);
                }

                IntVec3 intVec2 = base.Position + IntVec3.North.RotatedBy(base.Rotation), c2 = intVec2;
                if (base.Rotation == Rot4.East)
                {
                    c2 = new IntVec3(intVec2.x, intVec2.y, intVec2.z - 1);
                }
                else if (base.Rotation == Rot4.West)
                {
                    c2 = new IntVec3(intVec2.x, intVec2.y, intVec2.z + 1);
                }
                else if (base.Rotation == Rot4.North)
                {
                    c2 = new IntVec3(intVec2.x + 1, intVec2.y, intVec2.z);
                }
                else if (base.Rotation == Rot4.South)
                {
                    c2 = new IntVec3(intVec2.x - 1, intVec2.y, intVec2.z);
                }

                bool working = false;
                if (!intVec2.Impassable(base.Map) && !c2.Impassable(base.Map) && !intVec.Impassable(base.Map) && !c.Impassable(base.Map))
                {
                    float temperature = intVec2.GetTemperature(base.Map);
                    float temperature2 = intVec.GetTemperature(base.Map);
                    float num = temperature - temperature2;
                    if (temperature - 40f > num)
                    {
                        num = temperature - 40f;
                    }
                    float num2 = 1f - num * 0.0076923077f;
                    if (num2 < 0f)
                    {
                        num2 = 0f;
                    }
                    float energyLimit = this.compTempControl.Props.energyPerSecond * num2 * 4.16666651f;
                    float tempChange = GenTemperature.ControlTemperatureTempChange(intVec, base.Map, energyLimit, this.compTempControl.targetTemperature);
                    if (!Mathf.Approximately(tempChange, 0f))
                    {
                        working = true;
                        intVec.GetRoom(base.Map).Temperature += tempChange;
                    }
                }

                CompProperties_Power props = this.compPowerTrader.Props;
                if (working)
                {
                    this.compPowerTrader.PowerOutput = -props.basePowerConsumption;
                }
                else
                {
                    this.compPowerTrader.PowerOutput = -props.basePowerConsumption * this.compTempControl.Props.lowPowerConsumptionFactor;
                }
                this.compTempControl.operatingAtHighPower = working;
            }
        }
    }
}