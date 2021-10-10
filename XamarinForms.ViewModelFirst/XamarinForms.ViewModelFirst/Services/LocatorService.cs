using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;

namespace XamarinForms.ViewModelFirst.Services
{
    public class LocatorService
    {
        public LocatorService()
        {
            var builder = new ContainerBuilder();
            /*
            регистрация
            */



            var container = builder.Build();
            var csl = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => csl);


        }
      //  public MenuViewModel menuViewModelXaml => ServiceLocator.Current.GetInstance<MenuViewModel>();
    }
}
