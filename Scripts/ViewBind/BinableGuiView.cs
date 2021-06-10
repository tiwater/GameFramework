using System;
using UnityEngine;

namespace GameFramework.ViewBind
{

    public class BindableGuiView<T> : MonoBehaviour, IGuiView<T> where T : ViewModelBase
    {
        private bool _isInitialized;
        protected readonly PropertyBinder<T> Binder = new PropertyBinder<T>();
        public readonly BindableProperty<T> ViewModelProperty = new BindableProperty<T>();
        public T BindingContext
        {
            get { return ViewModelProperty.Value; }
            set {

                if (!_isInitialized)
                {
                    OnInitialize();
                    _isInitialized = true;
                }
                ViewModelProperty.Value = value;
            }
        }


        /// <summary>
        /// Initialize the view.
        /// </summary>
        protected virtual void OnInitialize()
        {
            //Monitor the context changing
            ViewModelProperty.OnValueChanged += OnBindingContextChanged;
        }


        /// <summary>
        /// Bind/unbind the listeners to context's properties changing
        /// </summary>
        public virtual void OnBindingContextChanged(T oldValue, T newValue)
        {
            Binder.Unbind(oldValue);
            Binder.Bind(newValue);
        }

    }
}