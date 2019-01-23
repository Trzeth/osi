using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace osi.Desktop.ViewModel
{
    class MainWindowViewModel:BaseViewModel
    {
        public int BorderMargin { get; set; } = 10;
        public Thickness BorderMarginThickness { get { return new Thickness(BorderMargin); } }

        public int TitleHeight { get; set; } = 50;
    }
}
