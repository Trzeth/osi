using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using osi.Core.DownloadManager.ApiModel.V2;
using static osi.Core.DownloadManager.ApiRoute.Router;

namespace osi.Core.DownloadManager
{
	public static class BeatmapsetInformationHelper
	{
		public static async Task GetInformationAsync(this BeatmapsetInformation beatmapsetInformation)
		{
			if (beatmapsetInformation.BeatmapsetId == 0)
				return;

			WebClient webClient = new WebClient();
			string s = webClient.DownloadString(Api.GetBeatmapInfoString(beatmapsetInformation.BeatmapsetId));

			JsonConvert.DeserializeObject<BeatmapInfo>(s).ToBeatmapsetInformation(ref beatmapsetInformation);
		}

		public static async Task<BeatmapListItemViewModel> GetDetailBeatmapListItemViewModelAsync(int beatmapsetId)
		{

			WebClient webClient = new WebClient();
			string s = webClient.DownloadString(Api.GetBeatmapInfoString(beatmapsetId));

			BeatmapsetInformation beatmapsetInformation = new BeatmapsetInformation();
			JsonConvert.DeserializeObject<BeatmapInfo>(s).ToBeatmapsetInformation(ref beatmapsetInformation);

			BeatmapListItemViewModel viewModel = new BeatmapListItemViewModel();
			viewModel.BeatmapsetInformation = beatmapsetInformation;

			return viewModel;
		}

		// TODO FINISH IT
		//public static async Task<List<BeatmapInformation>> GetBeatmapInformationListAsync()
		//{
		//	WebClient webClient = new WebClient();
		//	string s = webClient.DownloadString(Api.)
		//}

	}
}
