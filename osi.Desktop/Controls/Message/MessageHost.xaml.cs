using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace osi.Desktop
{
	/// <summary>
	/// MessageHost.xaml 的交互逻辑
	/// </summary>
	public partial class MessageHost : UserControl
	{
		public MessageHost()
		{
			InitializeComponent();
			var t = new TextBox();
			t.Text = "PPPPPPPPPPPPPPPPPP";
			var q = new TextBox();
			q.Text = "QQQQQQQQQQQQ";
			//List.Children.Add(t);
			List.Children.Insert(0, q);
		}
		public void Show(Control control)
		{
			control.Opacity = 0;
			List.Children.Insert(0, control);
			
		}
	}
}
