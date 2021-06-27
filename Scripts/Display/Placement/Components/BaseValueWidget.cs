using UnityEngine;
using System.Collections;
using System;

namespace GameFramework.Display.Placement.Components
{
    public abstract class BaseValueWidget<T> : MonoBehaviour
    {

        /// <summary>
        /// The max value the progress bar accepts
        /// </summary>
        [Tooltip("The max value the progress bar accepts")]
        public T MinValue;

        /// <summary>
        /// The min value the progress bar accepts
        /// </summary>
        [Tooltip("The min value the progress bar accepts")]
        public T MaxValue;

        /// <summary>
        /// The value of the progress
        /// </summary>
        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                T oldValue = _value;
                _value = value;
                OnValueChanged(oldValue, _value);
            }
        }
        [Tooltip("The value of the progress")]
        [SerializeField]
        private T _value;

        /// <summary>
        /// Whether the progress of the value changing is displayed in animation
        /// </summary>
        [Tooltip("Whether the progress of the value changing is displayed in animation")]
        public bool AnimatedProgress = true;

        /// <summary>
        /// The speed of the progress bar changing. Ignored when AnimatedProgress is false
        /// </summary>
        [Tooltip("The speed of the progress bar changing. Ignored when AnimatedProgress is false")]
        public float Speed = 20;

        /// <summary>
        /// The value to keep the intermediate value for animation
        /// </summary>
        protected T CurrentValue = default(T);

        /// <summary>
        /// The string to format the value
        /// </summary>
        public string ValueFormat;

        protected virtual void OnValueChanged(T oldValue, T newValue)
        {
            CheckValue();
            UpdateCurrentValue();
        }

        protected virtual void Update()
        {
            if (!CurrentValue.Equals(Value))
            {
                UpdateCurrentValue();
            }
        }

        /// <summary>
        /// Check whether the value of the widget is proper
        /// </summary>
        protected virtual void CheckValue()
        {
        }


        /// <summary>
        /// Update the intermediate value according to target value and speed
        /// </summary>
        protected virtual void UpdateCurrentValue() { }

        /// <summary>
        /// Return the formatted CurrentValue
        /// </summary>
        /// <returns></returns>
        protected virtual string FormattedCurrentValue()
        {
            return FormatValue(CurrentValue);
        }

        /// <summary>
        /// Format the value with the predefined format string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual string FormatValue(T value)
        {
            if (string.IsNullOrEmpty(ValueFormat))
            {
                return value.ToString();
            }
            else
            {
                return string.Format(ValueFormat, value);
            }
        }
    }
}