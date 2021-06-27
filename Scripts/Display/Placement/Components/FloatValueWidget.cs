using System;
using UnityEngine;

namespace GameFramework.Display.Placement.Components
{
    public class FloatValueWidget : BaseValueWidget<float>
    {
        /// <summary>
        /// Check whether the value of the widget is proper
        /// </summary>
        protected override void CheckValue()
        {

            if (MinValue > MaxValue)
            {
                Debug.LogWarning("MinValue " + MinValue + " > MaxValue: " + MaxValue);
                MinValue = MaxValue;
            }
            if (Value > MaxValue)
            {
                Debug.LogWarning("Value " + Value + " > MaxValue: " + MaxValue);
                Value = MaxValue;
            }
            if (Value < MinValue)
            {
                Debug.LogWarning("Value " + Value + " < MinValue: " + MinValue);
                Value = MinValue;
            }
        }


        /// <summary>
        /// Update the intermediate value according to target value and speed
        /// </summary>
        protected override void UpdateCurrentValue()
        {
            if (AnimatedProgress)
            {
                float delta = Speed * Time.deltaTime;
                float gap = Value - CurrentValue;
                if (delta > Mathf.Abs(gap))
                {
                    //If move too much, then we are at the target position now
                    CurrentValue = Value;
                }
                else
                {
                    //Decide the move direction
                    if (gap > 0)
                    {
                        CurrentValue += delta;
                    }
                    else
                    {
                        CurrentValue -= delta;
                    }
                }
            }
            else
            {
                CurrentValue = Value;
            }
        }

        /// <summary>
        /// Get the percentage that the CurrentValue stands for
        /// </summary>
        /// <returns></returns>
        protected float GetCurrentPercentage()
        {
            return (CurrentValue - MinValue) / (MaxValue - MinValue);
        }
    }
}