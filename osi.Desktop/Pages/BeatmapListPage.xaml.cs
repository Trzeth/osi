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
using osi.Core.DownloadManager;
using osi.Core;

namespace osi.Desktop
{
    /// <summary>
    /// BeatmapListPage.xaml 的交互逻辑
    /// </summary>
    public partial class BeatmapListPage : BasePage
    {
        public BeatmapListPage()
        {
            InitializeComponent();
			BeatmapListViewModel beatmapListViewModel = new BeatmapListViewModel();
			beatmapListViewModel.Items = new List<BeatmapListItemViewModel>();

			foreach (BeatmapsetInformation information in DownloadManager.GetBeatmapListAsync().Result)
			{
				beatmapListViewModel.Items.Add(new BeatmapListItemViewModel() { BeatmapsetInformation = information});
			}
			BeatmapList.DataContext = beatmapListViewModel;
		}

		private void SearchButton_Click(object sender, RoutedEventArgs e)
		{
		}
	}
}
