using osi.Core;
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
using static osi.Core.DownloadManager.BeatmapsetFilter;
using System.Windows.Shapes;
using osi.Core.DownloadManager;

namespace osi.Desktop
{
	/// <summary>
	/// MessageHost.xaml 的交互逻辑
	/// </summary> 
	public partial class BeatmapList : UserControl
	{
		public BeatmapList()
		{
			InitializeComponent();
		}
		public void Show(Control control)
		{
			control.Opacity = 0;
		}

		private void ListView_ScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			bool isAtButtom = false;
			double dVer = e.VerticalOffset;
			double dViewport = e.ViewportHeight;
			double dExtent = e.ExtentHeight;
			if (dVer != 0)
			{
				if (dVer + dViewport == dExtent)
				{
					isAtButtom = true;
				}
				else
				{
					isAtButtom = false;
				}
			}
			else
			{
				isAtButtom = false;
			}

			if (isAtButtom)
			{
				Task.Run(() => 
				{
					BeatmapsetFilter currentFilter = ViewModelLocator.Instance.BeatmapListPageViewModel.CurrentBeatmapsetFilter;
					currentFilter.Range.AddToBoth(24);

					ViewModelLocator.Instance.BeatmapListPageViewModel.CurrentBeatmapsetFilter = currentFilter;
					ViewModelLocator.Instance.BeatmapListPageViewModel.BeatmapListViewModel.Items.AddRange(BeatmapListHelper.GetBeatmapListItemViewModelListAsync(currentFilter).Result);

					List<BeatmapListItemViewModel> viewModels = ViewModelLocator.Instance.BeatmapListPageViewModel.BeatmapListViewModel.Items;
					ViewModelLocator.Instance.BeatmapListPageViewModel.BeatmapListViewModel.Items = viewModels;
					string s = "";
				});
			}

		}
	}
}
