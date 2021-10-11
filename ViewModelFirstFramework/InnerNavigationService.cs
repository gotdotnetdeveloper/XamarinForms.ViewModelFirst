using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModelFirstFramework.Helpers;
using Xamarin.Forms;

namespace ViewModelFirstFramework
{
	/// <summary>
	/// Сервис навигации.
	/// </summary>
	public sealed class InnerNavigationService
	{
		static readonly Lazy<InnerNavigationService> LazyInstance = new Lazy<InnerNavigationService>(() => new InnerNavigationService(), true);
		InnerNavigationService()
		{
			MessagingCenter.Subscribe<MessageBus, NavigationPushInfo>(this, Consts.NavigationPushMessage, NavigationPushCallback);
			MessagingCenter.Subscribe<MessageBus, NavigationPopInfo>(this, Consts.NavigationPopMessage, NavigationPopCallback);
		}
	    public static Task Init(NavigationPushInfo pushInfo)
	    {
	        return Instance.Initialize(pushInfo);
	    }

	    Task Initialize(NavigationPushInfo pushInfo)
	    {
		    RootPush(pushInfo);
		    return pushInfo.OnCompletedTask.Task;
	    }


		public static InnerNavigationService Instance => LazyInstance.Value;

		void NavigationPushCallback(MessageBus bus, NavigationPushInfo navigationPushInfo)
		{
			if (navigationPushInfo == null) throw new ArgumentNullException(nameof(navigationPushInfo));
			if (navigationPushInfo.Page == null) throw new ArgumentNullException(nameof(navigationPushInfo.Page));

Push(navigationPushInfo);
		}

		void NavigationPopCallback(MessageBus bus, NavigationPopInfo navigationPopInfo)
		{
			if (navigationPopInfo == null) throw new ArgumentNullException(nameof(navigationPopInfo));
		    Pop(navigationPopInfo);
		}

		#region InnerNavigationService internals

		INavigation GetTopNavigation() {
			var mainPage = Application.Current.MainPage;
		
			return (mainPage as NavigationPage)?.Navigation;
		}

	    void Push(NavigationPushInfo pushInfo)
	    {

	        switch (pushInfo.Mode)
	        {
	            case NavigationMode.Normal:
	                NormalPush(pushInfo);
	                break;
	            case NavigationMode.Modal:
	                ModalPush(pushInfo);
	                break;
				case NavigationMode.RootPage:
					RootPush(pushInfo);
					break;
	            
		        default:
	                throw new NotImplementedException();
	        }
	    }

	    void RootPush(NavigationPushInfo pushInfo)
	    {
            pushInfo.ViewModel.Init(pushInfo.NavigationParams);
            
            Device.BeginInvokeOnMainThread(() =>
		    {
			    try
                {
                    pushInfo.Page.BindingContext = pushInfo.ViewModel;
					Application.Current.MainPage = new NavigationPage(pushInfo.Page);
                    pushInfo.OnCompletedTask.SetResult(true);
			    }
			    catch
			    {
                    pushInfo.OnCompletedTask.SetResult(false);
			    }
		    });
	    }

	    void NormalPush(NavigationPushInfo pushInfo)
		{
            pushInfo.ViewModel.Init(pushInfo.NavigationParams);
			Device.BeginInvokeOnMainThread(async () =>
			{
				try
				{
					pushInfo.Page.BindingContext = pushInfo.ViewModel;
					await GetTopNavigation().PushAsync(pushInfo.Page, true);
                    pushInfo.OnCompletedTask.SetResult(true);
				}
				catch
				{
                    pushInfo.OnCompletedTask.SetResult(false);
				}
			});
		}

		/// <summary>
		/// Модальное окошко
		/// </summary>
		void ModalPush(NavigationPushInfo pushInfo)
		{
			pushInfo.ViewModel.Init(pushInfo.NavigationParams);
			Device.BeginInvokeOnMainThread(async () =>
			{
				try
				{
					if (pushInfo.NewNavigationStack)
                        pushInfo.Page = new NavigationPage(pushInfo.Page );
                    
                    pushInfo.Page.BindingContext = pushInfo.ViewModel;

					await GetTopNavigation().PushModalAsync(pushInfo.Page, true);
                    pushInfo.OnCompletedTask.SetResult(true);
				}
				catch
				{
                    pushInfo.OnCompletedTask.SetResult(false);
				}
			});
		}
		
		
		void Pop(NavigationPopInfo popInfo) {
			switch (popInfo.Mode) {
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
	



	
		#endregion
	}
	
    public class NavigationPushInfo
	{
		public BaseViewModel ViewModel { get; set; }
		public Page Page { get; set; }
        public Dictionary<string, object> NavigationParams { get; set; } = new Dictionary<string, object>();
		public NavigationMode Mode { get; set; } = NavigationMode.Normal;
		public bool NewNavigationStack { get; set; }
        public TaskCompletionSource<bool> OnCompletedTask { get; set; } = new TaskCompletionSource<bool>();
    }

	public class NavigationPopInfo
	{
		public NavigationMode Mode { get; set; } = NavigationMode.Normal;
        public TaskCompletionSource<bool> OnCompletedTask { get; set; }
		public BasePage Page { get; set; }
	}
}