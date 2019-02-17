using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace osi.Desktop.Windows
{
	/// <summary>
	/// UpdateWindow.xaml 的交互逻辑
	/// </summary>
	public partial class UpdateWindow : Window
	{
		private Helper.UpdateHelper mUpdateHelper;
		public UpdateWindow(Helper.UpdateHelper updateHelper)
		{
			InitializeComponent();
			mUpdateHelper = updateHelper;
			BackgroundWorker backgroundWorker = new BackgroundWorker();
			backgroundWorker.DoWork += delegate
			{
				updateHelper.DownloadUpdateFile();
			};
			backgroundWorker.RunWorkerCompleted += delegate
			{
				//Close();
			};
			backgroundWorker.RunWorkerAsync();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			DoubleAnimation da = new DoubleAnimation();
			da.To = 0;
			da.Duration = TimeSpan.FromMilliseconds(200);
			da.FillBehavior = FillBehavior.Stop;
			da.Completed += delegate
			{
				WindowState = WindowState.Minimized;
				Opacity = 0;
			};

			BeginAnimation(OpacityProperty, da);

		}
		protected override void OnStateChanged(EventArgs e)
		{
			if(WindowState == WindowState.Normal)
			{
				DoubleAnimation da = new DoubleAnimation();
				da.To = 1;
				da.Duration = TimeSpan.FromMilliseconds(200);
				da.FillBehavior = FillBehavior.Stop;
				da.Completed += delegate
				{
					Opacity = 1;
				};

				BeginAnimation(OpacityProperty, da);
			}

			base.OnStateChanged(e);
		}
	}
}
