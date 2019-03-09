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
using System.Windows.Media.Animation;
using osi.Desktop.Helper;
using osi.Core.DownloadManager;

namespace osi.Desktop
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MessageHostWindow : Window
	{
		#region Private Member

		#endregion

		#region  Constructor

		public MessageHostWindow()
		{
			Loaded += (sender, e) =>
			{
				Top = SystemParameters.WorkArea.Height + SystemParameters.FixedFrameHorizontalBorderHeight + SystemParameters.ResizeFrameHorizontalBorderHeight - ActualHeight - 10;
				Left = SystemParameters.WorkArea.Width + SystemParameters.FixedFrameVerticalBorderWidth + SystemParameters.ResizeFrameVerticalBorderWidth - ActualWidth - 10;
			};

			InitializeComponent();
		}

		#endregion

		#region Methods



		#endregion

		public new void Hide()
		{
			DoubleAnimation da = new DoubleAnimation();
			da.To = 0;
			da.Duration = TimeSpan.FromMilliseconds(200);
			da.Completed += delegate
			{
				base.Hide();
			};

			BeginAnimation(OpacityProperty, da);
		}
		public new void Close()
		{
			if (Visibility == Visibility.Visible)
			{
				DoubleAnimation da = new DoubleAnimation();
				da.To = 0;
				da.Duration = TimeSpan.FromMilliseconds(200);
				da.Completed += delegate
				{
					base.Close();
				};

				BeginAnimation(OpacityProperty, da);

			}
			else
			{
				base.Close();
			}
		}
		public new void Show()
		{
			this.Visibility = Visibility.Visible;
			DoubleAnimation da = new DoubleAnimation();
			da.To = 1;
			da.Duration = TimeSpan.FromMilliseconds(200);
			da.Completed += delegate
			{
				base.Show();
			};
			BeginAnimation(OpacityProperty, da);

		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			IoC.Get<DownloadTaskListViewModel>().Items.Add(new DownloadTaskViewModel()
			{
				BeatmapsetId = 923990,
				BPM = 218,
				Title = "White parade",
				Artist = "Umeboshi Chazuke",
				Creator = "ATing",
				Length = TimeSpan.FromSeconds(196),

				DownloadingStatus = DownloadingStatus.Downloading,
				Progress = 0.5f
			});
		}
	}
}
