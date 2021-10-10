using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ViewModelFirstFramework
{
    /// <summary>
    /// Сервис Навигации.
    /// </summary>
    public class NavigationService
    {
		/// <summary>
		/// Ленивая инициализация.
		/// </summary>
        static readonly Lazy<NavigationService> LazyInstance = new Lazy<NavigationService>(() => new NavigationService(), true);

		/// <summary>
		/// Доступ до экземпляра сервиса навигации, при ленивой инициализации.
		/// </summary>
		public static NavigationService Instance => LazyInstance.Value;


        /// <summary>
        /// Убрать страницу из стека. (удалить страницу)
        /// </summary>
        public void Pop(NavigationPopInfo popInfo)
        {
            switch (popInfo.Mode)
            {
                case NavigationMode.Normal:
                    NormalPop(popInfo.OnCompletedTask);
                    break;
                case NavigationMode.Modal:
                    ModalPop(popInfo.OnCompletedTask);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        public void Push(NavigationPushInfo pushInfo, Page newPage = null)
        {

            switch (pushInfo.Mode)
            {
                case NavigationMode.Normal:
                    NormalPush(newPage, pushInfo.OnCompletedTask);
                    break;
                case NavigationMode.Modal:
                    ModalPush(newPage, pushInfo.OnCompletedTask, pushInfo.NewNavigationStack);
                    break;
                case NavigationMode.RootPage:
                    RootPush(newPage, pushInfo.OnCompletedTask);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }




		#region Приватные методы

		/// <summary>
		/// Получить новигацию со стека верхнего уровеня 
		/// </summary>
		INavigation GetTopNavigation()
		{
			var mainPage = Application.Current.MainPage;
			return (mainPage as NavigationPage)?.Navigation;
		}

	

		void RootPush(Page newPage, TaskCompletionSource<bool> pushInfoOnCompletedTask)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				try
				{
					Application.Current.MainPage = new NavigationPage(newPage);
					pushInfoOnCompletedTask.SetResult(true);
				}
				catch
				{
					pushInfoOnCompletedTask.SetResult(false);
				}
			});
		}

		void NormalPush(Page newPage, TaskCompletionSource<bool> completed)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				try
				{
					await GetTopNavigation().PushAsync(newPage, true);
					completed.SetResult(true);
				}
				catch
				{
					completed.SetResult(false);
				}
			});
		}
		void ModalPush(Page newPage, TaskCompletionSource<bool> completed, bool newNavigationStack = true)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				try
				{
					if (newNavigationStack) newPage = new NavigationPage(newPage);
					await GetTopNavigation().PushModalAsync(newPage, true);
					completed.SetResult(true);
				}
				catch
				{
					completed.SetResult(false);
				}
			});
		}

      
		void ModalPop(TaskCompletionSource<bool> completed)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				try
				{
					await GetTopNavigation().PopModalAsync();
					completed.SetResult(true);
				}
				catch
				{
					completed.SetResult(false);
				}
			});
		}

		void NormalPop(TaskCompletionSource<bool> completed)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				try
				{
					await GetTopNavigation().PopAsync();
					completed.SetResult(true);
				}
				catch
				{
					completed.SetResult(false);
				}
			});
		}
		static string GetTypeBaseName(MemberInfo info)
		{
			if (info == null) throw new ArgumentNullException(nameof(info));
			return info.Name.Replace(@"Page", "").Replace(@"ViewModel", "");
		}
		static Dictionary<string, Type> GetAssemblyPageTypes()
		{
			return typeof(BaseView).GetTypeInfo().Assembly.DefinedTypes
				.Where(ti => ti.IsClass && !ti.IsAbstract && ti.Name.Contains(@"Page") && ti.BaseType.Name.Contains(@"Page"))
				.ToDictionary(GetTypeBaseName, ti => ti.AsType());
		}
		static Dictionary<string, Type> GetAssemblyViewModelTypes()
		{
			return typeof(BaseViewModel).GetTypeInfo().Assembly.DefinedTypes
										.Where(ti => ti.IsClass && !ti.IsAbstract && ti.Name.Contains(@"ViewModel") &&
													 ti.BaseType.Name.Contains(@"ViewModel"))
										.ToDictionary(GetTypeBaseName, ti => ti.AsType());
		}

	

	
	
		

		#endregion

		void Show(IViewModel viewModel, IView view)
        {
            
        }
    }
	/// <summary>
	/// контекст для положить страницу в стек. (отобразить страницу)
	/// </summary>
    public class NavigationPushInfo
    {
        public string From { get; set; }
        public string To { get; set; }
        public Dictionary<string, object> NavigationParams { get; set; }
        public NavigationMode Mode { get; set; } = NavigationMode.Normal;
        public bool NewNavigationStack { get; set; }
        public TaskCompletionSource<bool> OnCompletedTask { get; set; }
    }
    /// <summary>
    /// контекст для убрать страницу из стека. (удалить страницу)
    /// </summary>
	public class NavigationPopInfo
    {
        public NavigationMode Mode { get; set; } = NavigationMode.Normal;
        public TaskCompletionSource<bool> OnCompletedTask { get; set; }
        public string To { get; set; }
    }

    public enum NavigationMode
    {
		/// <summary>
		/// Обычная навигация (полноэкранная форма)
		/// </summary>
        Normal,
		/// <summary>
		/// Модальное (диалоговое) окно.
		/// </summary>
        Modal,
		/// <summary>
		/// первая страница приложения.
		/// </summary>
        RootPage,
    }

	/// <summary>
	/// Режимы отображения страницы.
	/// </summary>
    public enum PageState
    {
		/// <summary>
		/// 
		/// </summary>
        Clean,
		/// <summary>
		/// Отображение процесса загрузки страницы (IsBuisy)
		/// </summary>
        Loading,
		/// <summary>
		/// Загружены данные. Все ок.
		/// </summary>
        Normal,
		/// <summary>
		/// Отсутствие контента (запрос вернул пустую коллекцию)
		/// </summary>
        NoData,
		/// <summary>
		/// Необработанное исключение.
		/// </summary>
        Error,
		/// <summary>
		/// Отсутствие соединение.
		/// </summary>
        NoInternet
    }

}
