using System;

namespace GameFramework.ViewBind
{
    public class ViewModelBase
    {
        protected bool _isInitialized;
        public ViewModelBase ParentViewModel { get; set; }

        protected virtual void OnInitialize()
        {

        }

        public virtual void OnDestory()
        {

        }

    }
}