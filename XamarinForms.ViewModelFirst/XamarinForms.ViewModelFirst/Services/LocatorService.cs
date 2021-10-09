using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;

using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp.Portable.Service
{
    public class LocatorService
    {
        public LocatorService()
        {
            //var builder = new ContainerBuilder();
            //builder.RegisterType<MenuViewModel>().AsSelf().SingleInstance();
            //builder.Register(Context => new HttpService(new ConfigurationClass()._ConfigurationClass.BaseAdress)).AsSelf().SingleInstance();
            //builder.RegisterType<AuthService>().As<IAuthService>().SingleInstance();
            //var container = builder.Build();
            //var csl = new AutofacServiceLocator(container);
            //ServiceLocator.SetLocatorProvider(() => csl);

            
        }
      //  public MenuViewModel menuViewModelXaml => ServiceLocator.Current.GetInstance<MenuViewModel>();
    }
}
