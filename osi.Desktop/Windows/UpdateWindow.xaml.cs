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
				Close();
			};
			backgroundWorker.RunWorkerAsync();
		}
	}
}
