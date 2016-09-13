using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VyrVM
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            /*
            if (ViewModelBase.IsInDesignModeStatic)
                SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
            else
                SimpleIoc.Default.Register<IDataService, DataService>();         
            */
            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main { get { return SimpleIoc.Default.GetInstance<MainViewModel>(); } }
    }
}
