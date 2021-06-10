using System;
namespace GameFramework.ViewBind
{
    /// <summary>
    /// A view to support bindable properties
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGuiView<T> where T : ViewModelBase
    {
        T BindingContext { get; set; }
    }
}
