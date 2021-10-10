using Xamarin.Forms;

namespace ViewModelFirstFramework
{
    /// <summary>
    /// Базовое представление MVVM.
    /// </summary>
    public abstract class BaseView :  ContentPage , IView
    {

        /// <summary>
        /// При переопределении позволяет разработчикам приложений настраивать 
        /// поведение непосредственно перед тем, как Xamarin.Forms.Page станет видимым.
        /// </summary>
        protected override void OnAppearing()
        {
            if(BindingContext is IViewModel viewModel )
                viewModel.OnAppearing();

            base.OnAppearing();
        }
        /// <summary>
        /// Резюме:
        /// При переопределении позволяет разработчику приложения настроить поведение как
        /// Xamarin.Forms.Page исчезает.
        /// Примечания:
        /// Xamarin.Forms.Page.OnDisappearing вызывается, когда страница исчезает из-за навигации
        /// дальше от страницы в приложении. Он не вызывается, когда приложение исчезает из-за
        /// к событию, внешнему по отношению к приложению (например, пользователь переходит на главный экран или другой
        /// приложение, телефонный звонок получен, устройство заблокировано, устройство выключено).
        /// </summary>
        protected override void OnDisappearing()
        {
            if (BindingContext is IViewModel viewModel)
                viewModel.OnDisappearing();

            base.OnDisappearing();
        }

        /// <summary>
        /// // Разработчики приложений могут переопределить этот метод, чтобы обеспечить поведение, когда
        /// нажата кнопка возврата.
        ///
        /// Возвращает:
        /// Независимо от того, обрабатывалась ли обратная навигация переопределением.
        ///
        /// Примечания:
        /// Если вы хотите обработать или отменить навигацию самостоятельно, вы можете сделать это в этом
        /// метод, а затем вернуть true.
        /// Обратите внимание, что это работает только на Android и UWP для аппаратной кнопки возврата. На
        /// iOS, этот метод никогда не будет вызван, потому что нет аппаратной кнопки возврата.
        /// </summary>
        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }

    }
}