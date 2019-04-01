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
			if (((BeatmapListPageViewModel)DataContext).BeatmapListViewModel == null)
			{
				Task.Run(() =>{
					ViewModelLocator.Instance.BeatmapListPageViewModel.BeatmapListViewModel = DownloadManager.GetBeatmapListViewModelAsync(ListType.New).Result;
				});
			};
		}

		private void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			string searchText = InputBox.Text;
			Task.Run(() =>
			{
				BeatmapListViewModel viewModel = DownloadManager.GetBeatmapListViewModelAsync(ListType.Search, searchText).Result;
				ViewModelLocator.Instance.BeatmapListPageViewModel.BeatmapListViewModel = viewModel;
			});
		}

		private void HotButton_Click(object sender, RoutedEventArgs e)
		{
			string searchText = InputBox.Text;
			Task.Run(() =>
			{
				BeatmapListViewModel viewModel = DownloadManager.GetBeatmapListViewModelAsync(ListType.Hot).Result;
				ViewModelLocator.Instance.BeatmapListPageViewModel.BeatmapListViewModel = viewModel;
			});
		}

		private void NewButton_Click(object sender, RoutedEventArgs e)
		{
			string searchText = InputBox.Text;
			Task.Run(() =>
			{
				BeatmapListViewModel viewModel = DownloadManager.GetBeatmapListViewModelAsync(ListType.New).Result;
				ViewModelLocator.Instance.BeatmapListPageViewModel.BeatmapListViewModel = viewModel;
			});
		}
	}
}
