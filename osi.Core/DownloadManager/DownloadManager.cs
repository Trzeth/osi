using Newtonsoft.Json;
using osi.Core.DownloadManager.ApiModel.V1;
using osi.Core.DownloadManager.ApiRoute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace osi.Core.DownloadManager
{
	public class DownloadManager
	{

		#region Public Properties

		public static DownloadManager Current = new DownloadManager();

		public EventHandler<DownloadCompleteEventArgs> DownloadComplete;

		public class DownloadCompleteEventArgs : EventArgs
		{
			public string Path { get; set; }
		}

		public void OnDownloadComplete(DownloadCompleteEventArgs e)
		{
			DownloadComplete?.Invoke(this, e);
		}

		public string Path { get; set; } = @"E:\Coding\osi\osi.Desktop\bin\Debug\download\123.osz";


		#endregion

		#region Private Properties

		private AsyncObservableCollection<DownloadTaskViewModel> mDownloadTaskList
		{
			get { return IoC.Get<DownloadTaskListViewModel>().Items; }
		}

		#endregion

		#region Contructor

		public DownloadManager(){ }

		#endregion


		#region GetBeatmapList Method

		public static async Task<List<BeatmapsetInformation>> GetBeatmapListAsync(ListType type, object keyword = null, int limit = 25, int offset = 0)
		{
			List<BeatmapsetInformation> list = new List<BeatmapsetInformation>();

			WebClient webClient = new WebClient();
			string s = webClient.DownloadString(Router.Api.BeatmapList.GetBeatmapListString(type, keyword, limit, offset));
			list = (JsonConvert.DeserializeObject<BeatmapList>(s)).ToBeatmapInformationList();

			return list;
		}

		public static async Task<List<BeatmapsetInformation>> SearchBeatmapListAsync(object keyword, BeatmapsetFilter filter = null, int limit = 25, int offset = 0)
		{
			List<BeatmapsetInformation> list = new List<BeatmapsetInformation>();

			WebClient webClient = new WebClient();
			string s = webClient.DownloadString(Router.Api.BeatmapList.GetBeatmapListString(ListType.Search, filter,keyword, limit, offset));
			list = (JsonConvert.DeserializeObject<BeatmapList>(s)).ToBeatmapInformationList();

			return list;
		}
		public static async Task<BeatmapListViewModel> SearchBeatmapListViewModelAsync(object keyword, BeatmapsetFilter filter = null, int limit = 25, int offset = 0)
		{
			BeatmapListViewModel beatmapListViewModel = new BeatmapListViewModel();


			return beatmapListViewModel;
		}

		public static async Task<BeatmapListViewModel> GetBeatmapListViewModelAsync(ListType type, object keyword = null, int limit = 25, int offset = 0)
		{
			return BeatmapListToBeatmapListViewModel(await GetBeatmapListAsync(type, keyword, limit, offset));
		}

		private static BeatmapListViewModel BeatmapListToBeatmapListViewModel(List<BeatmapsetInformation> BeatmapList)
		{
			BeatmapListViewModel beatmapListViewModel = new BeatmapListViewModel();
			beatmapListViewModel.Items = new List<BeatmapListItemViewModel>();

			foreach (BeatmapsetInformation information in BeatmapList)
			{
				beatmapListViewModel.Items.Add(new BeatmapListItemViewModel() { BeatmapsetInformation = information });
			}

			return beatmapListViewModel;
		}

		#endregion

		#region Donwload Methods 

		public async Task DownloadBeatmapsetAsync(int beatmapsetId)
		{
			await DownloadBeatmapsetAsync(new DownloadTaskViewModel(beatmapsetId));
		}

		public async Task DownloadBeatmapsetAsync(DownloadTaskViewModel downloadTaskViewModel)
		{
			await downloadTaskViewModel.GetInformationAsync();
			mDownloadTaskList.Add(downloadTaskViewModel);

			Download(downloadTaskViewModel.BeatmapsetId, downloadTaskViewModel);
		}

		public void RetryDownloadBeatmapset(DownloadTaskViewModel downloadTaskViewModel)
		{
			//downloadTaskViewModel.DownloadStatus.DownloadingStatus = DownloadingStatus.Downloading;

			//Task.Run(() => Download(downloadTaskViewModel.BeatmapsetId, downloadTaskViewModel.DownloadStatus));

		}

		private void Download(int beatmapsetId, DownloadTaskViewModel downloadTaskViewModel)
		{
			WebClient webClient = new WebClient();

			downloadTaskViewModel.DownloadingStatus = DownloadingStatus.Downloading;
			webClient.DownloadProgressChanged += (sender, e) =>
			{
				downloadTaskViewModel.Progress = ((float)e.ProgressPercentage) / 100;
			};

			webClient.DownloadFileCompleted += (sender, e) =>
			{
				if (e.Error != null)
				{

				}
				else
				{
					downloadTaskViewModel.DownloadingStatus = DownloadingStatus.Complete;

					OnDownloadComplete(new DownloadCompleteEventArgs() { Path = this.Path });
				}
			};
			Uri s = Router.Resource.Beatmapsets.GetBeatmapsetUri(beatmapsetId);
			webClient.DownloadFileAsync(s, Path);
		}

		#endregion
		}
	}
