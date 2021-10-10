namespace ViewModelFirstFramework
{
    /// <summary>
    /// Представление MVVM.
    /// </summary>
    public interface IView
    {
        bool IsBusy { get; set; }
        /// <summary>
        /// Получает или задает объект, содержащий свойства, на которые будет нацелен
        /// связанные свойства, принадлежащие этому объекту Xamarin.Forms.BindableObject.
        /// Как правило, производительность среды выполнения лучше, если Xamarin.Forms.BindableObject.BindingContext
        /// устанавливается после всех вызовов Xamarin.Forms.BindableObject.SetBinding 
        /// </summary>
        object BindingContext { get; set; }


        /// <summary>
        /// Вызывает Xamarin.Forms.Page.OnBackButtonPressed.
        /// </summary>
        bool SendBackButtonPressed();
    }
}