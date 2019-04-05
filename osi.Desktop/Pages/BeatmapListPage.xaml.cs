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
using static osi.Core.DownloadManager.BeatmapListHelper;
using static osi.Core.DownloadManager.BeatmapsetInformationHelper;
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
					BeatmapsetFilter filter = new BeatmapsetFilter(ListType.New, new Range(0, 24));
					ViewModelLocator.Instance.BeatmapListPageViewModel.CurrentBeatmapsetFilter = filter;
					BeatmapListViewModel viewModel = GetBeatmapListViewModelAsync(filter).Result;
					ViewModelLocator.Instance.BeatmapListPageViewModel.BeatmapListViewModel = viewModel;
				});
			};
		}

		private void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			string searchText = InputBox.Text;
			Task.Run(() =>
			{
				BeatmapsetFilter filter = new BeatmapsetFilter(searchText, new Range(0, 24));
				ViewModelLocator.Instance.BeatmapListPageViewModel.CurrentBeatmapsetFilter = filter;

				BeatmapListViewModel viewModel = GetBeatmapListViewModelAsync(filter).Result;
				ViewModelLocator.Instance.BeatmapListPageViewModel.BeatmapListViewModel = viewModel;
			});
		}

		private void FilterSearchButton_Click(object sender, RoutedEventArgs e)
		{
			string searchText = InputBox.Text;
			Task.Run(() =>
			{
				BeatmapsetFilter filter = new BeatmapsetFilter(searchText, new Range(0, 24));
				ViewModelLocator.Instance.BeatmapListPageViewModel.CurrentBeatmapsetFilter = filter;

				BeatmapListViewModel viewModel = GetBeatmapListViewModelAsync(filter).Result;
				ViewModelLocator.Instance.BeatmapListPageViewModel.BeatmapListViewModel = viewModel;
			});
		}
		

		private void HotButton_Click(object sender, RoutedEventArgs e)
		{
			string searchText = InputBox.Text;
			Task.Run(() =>
			{
				BeatmapsetFilter filter = new BeatmapsetFilter(ListType.Hot, new Range(0, 24));
				ViewModelLocator.Instance.BeatmapListPageViewModel.CurrentBeatmapsetFilter = filter;

				BeatmapListViewModel viewModel = GetBeatmapListViewModelAsync(filter).Result;
				ViewModelLocator.Instance.BeatmapListPageViewModel.BeatmapListViewModel = viewModel;
			});
		}

		private void NewButton_Click(object sender, RoutedEventArgs e)
		{
			string searchText = InputBox.Text;
			Task.Run(() =>
			{
				BeatmapsetFilter filter = new BeatmapsetFilter(ListType.New,new Range(0,24));
				ViewModelLocator.Instance.BeatmapListPageViewModel.CurrentBeatmapsetFilter = filter;

				BeatmapListViewModel viewModel = GetBeatmapListViewModelAsync(filter).Result;
				ViewModelLocator.Instance.BeatmapListPageViewModel.BeatmapListViewModel = viewModel;
			});
		}

		private void FilterButton_Click(object sender, RoutedEventArgs e)
		{
			Filter.IsOpen = true;
		}
	}
}
