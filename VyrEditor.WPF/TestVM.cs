using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VyrEditor.WPF
{
    public class TestVM : ViewModelBase
    {
        public RelayCommand TestCommand
        {
            get
            {
                return new RelayCommand(() => System.Windows.MessageBox.Show("Hmpf. Ho"));
            }
        }
    }
}
