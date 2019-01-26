using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace osi.Core
{
    /// <summary>
    /// the base view model that fires property as need
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// the event that fire at property change
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender,e)=> { };

        public void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }

}
