using ViewModelFirstFramework;
using Xamarin.Forms;


namespace XamarinForms.ViewModelFirst
{
    public partial class App : Application
    {

		/// <summary>
		/// Конструктор.
        /// </summary>
        public App()
        {
            // не убирать. Фикс бага под ios 
            Current.MainPage = new ContentPage();
        }

        /// <summary>
        /// Старт приложения.
        /// </summary>
        protected override async void OnStart()
        {
            InitializeComponent();
            
            await InnerNavigationService.Init(new NavigationPushInfo() { Page = new Page()});
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
	}
}
