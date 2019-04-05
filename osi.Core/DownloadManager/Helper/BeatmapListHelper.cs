using Newtonsoft.Json;
using osi.Core.DownloadManager.ApiModel.V1;
using osi.Core.DownloadManager.ApiRoute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osi.Core.DownloadManager
{
	public static class BeatmapListHelper
	{
		public static async Task<List<BeatmapsetInformation>> GetBeatmapListAsync(BeatmapsetFilter beatmapsetFilter)
		{
			List<BeatmapsetInformation> list = new List<BeatmapsetInformation>();

			WebClient webClient = new WebClient();
			string s = webClient.DownloadString(Router.Api.BeatmapList.GetBeatmapListString(beatmapsetFilter));
			list = (JsonConvert.DeserializeObject<BeatmapList>(s)).ToBeatmapInformationList();

			return list;
		}

		public static async Task<List<BeatmapListItemViewModel>> GetBeatmapListItemViewModelListAsync(BeatmapsetFilter beatmapsetFilter)
		{
			return BeatmapListToBeatmapListViewModel(await GetBeatmapListAsync(beatmapsetFilter));
		}

		public static async Task<BeatmapListViewModel> GetBeatmapListViewModelAsync(BeatmapsetFilter beatmapsetFilter)
		{
			BeatmapListViewModel viewModel = new BeatmapListViewModel();
			viewModel.Items.AddRange(await GetBeatmapListItemViewModelListAsync(beatmapsetFilter));
			return viewModel;
		}

		private static List<BeatmapListItemViewModel> BeatmapListToBeatmapListViewModel(List<BeatmapsetInformation> BeatmapList)
		{
			List<BeatmapListItemViewModel> items = new List<BeatmapListItemViewModel>();

			foreach (BeatmapsetInformation information in BeatmapList)
			{
				items.Add(new BeatmapListItemViewModel() { BeatmapsetInformation = information });
			}
			return items;
		}
	}
}
