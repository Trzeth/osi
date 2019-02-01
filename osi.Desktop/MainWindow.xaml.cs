using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using osi.Core;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json.Serialization;
using System.Windows.Forms;

namespace osi.Desktop
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Private Member

		private List<BeatmapsetDownloadListItemViewModel> items;

		#endregion

		#region  Constructor

		public MainWindow()
		{
			InitializeComponent();

			this.DataContext = new MainWindowViewModel(this);
		}

		#endregion

		#region Methods


		#endregion

		public new void Hide()
		{

			base.Hide();
		}
		public new void Close()
		{
			if (Visibility == Visibility.Visible)
			{
				Hide();
			}
			base.Close();
		}
		public new void Show()
		{
			base.Show();
		}
	}
}
