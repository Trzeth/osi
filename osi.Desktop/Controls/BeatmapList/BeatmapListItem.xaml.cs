using osi.Core;
using osi.Core.DownloadManager;
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
using static osi.Core.DownloadManager.BeatmapsetInformationHelper;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace osi.Desktop
{
	/// <summary>
	/// DownloadMessage.xaml 的交互逻辑
	/// </summary>
	public partial class BeatmapListItem : UserControl
	{
		#region Dependency Properties

		#endregion

		public BeatmapListItem()
		{
			InitializeComponent();
		}

		private void Panel_MouseDown(object sender, MouseButtonEventArgs e)
		{
			BeatmapListItemViewModel viewModel = (BeatmapListItemViewModel)DataContext;
			if(viewModel.HasGetDetailed == false)
			{
				Task.Run(() =>
				{
					viewModel = GetDetailBeatmapListItemViewModelAsync(viewModel.BeatmapsetInformation.BeatmapsetId).Result;
					viewModel.HasGetDetailed = true;
					Dispatcher.Invoke(() => { DataContext = viewModel; });
				});
			}

		}
	}
}
