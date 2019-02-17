using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json;
using osi.Core;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.IO.Pipes;
using System.IO;
using System.Windows.Forms;
using osi.Desktop.Helper;
using GoogleAnalyticsTracker.Simple;

namespace osi.Desktop
{
    public class MainWindowViewModel:BaseViewModel
    {
        #region Private Member

        private MainWindow mWindow;
		private NotifyIcon mNotifyIcon;
		private string mPreviousLink;
		#endregion

		#region Public Member

		/// <summary>
		/// Outer window Border Margin
		/// </summary>
		public int OuterBorderPadding { get; set; } = 10;
		public Thickness BorderMargiOuterBorderPaddingThicknessnThickness { get { return new Thickness(OuterBorderPadding); } }

		/// <summary>
		/// Window title height
		/// </summary>
		public int TitleHeight { get; set; } = 40;

		public BeatmapsetDownloadListViewModel BeatmapsetDownloadList { get; set; } = new BeatmapsetDownloadListViewModel();

		//public BeatmapsetListViewModel BeatmapsetList
		//{
		//	get
		//	{
		//		string json = @"{'status':0,'endid':914241,'data':[{'sid':915817,'modes':1,'approved':-1,'lastupdate':1548297103,'title':'Kawaki o Ameku (TV Size)','artist':'Minami','titleU':'','artistU':'','creator':'Alphabet','favourite_count':1,'order':0},{'sid':914973,'modes':0,'approved':-1,'lastupdate':1548218278,'title':'Stella-rium','artist':'Kano','titleU':'','artistU':'','creator':'Shallow','favourite_count':0,'order':0},{'sid':914907,'modes':0,'approved':-1,'lastupdate':1548114086,'title':'Kawaki wo Ameku','artist':'Minami','titleU':'','artistU':'','creator':'agosinter','favourite_count':0,'order':0},{'sid':914786,'modes':1,'approved':-1,'lastupdate':1548309167,'title':'Gotoubun no Kimochi (TV Size)','artist':'Nakanoke no Itsuzugo','titleU':'','artistU':'','creator':'Ianos','favourite_count':0,'order':0},{'sid':914766,'modes':0,'approved':-1,'lastupdate':1548253910,'title':'M flat','artist':'Kato Megumi (CV.Yasuno Kiyono)','titleU':'','artistU':'','creator':'realy0_','favourite_count':0,'order':0},{'sid':914523,'modes':0,'approved':-1,'lastupdate':1548050271,'title':'Domestic na Kanojo Opening','artist':'','titleU':'','artistU':'','creator':'kamisinha','favourite_count':0,'order':0},{'sid':914492,'modes':0,'approved':-1,'lastupdate':1548056346,'title':'Kawaki wo Ameku','artist':'Minami','titleU':'','artistU':'','creator':'Hvgin','favourite_count':0,'order':0},{'sid':914346,'modes':0,'approved':-1,'lastupdate':1548014825,'title':'Yuudachi no Ribbon','artist':'Kano','titleU':'','artistU':'','creator':'Failure444','favourite_count':1,'order':0},{'sid':914335,'modes':0,'approved':-1,'lastupdate':1548012533,'title':'Happiness Magical Kanon','artist':'DJ Michelle','titleU':'','artistU':'','creator':'LawyerKirby','favourite_count':0,'order':0},{'sid':914242,'modes':0,'approved':-1,'lastupdate':1547999106,'title':'Walk This Way!','artist':'Kano','titleU':'','artistU':'','creator':'Sotarks','favourite_count':4,'order':0}]}";

		//		BeatmapsetListViewModel beatmapsetListViewModel = new BeatmapsetListViewModel();
		//		List<BeatmapsetListItemViewModel> beatmapsetLists = new List<BeatmapsetListItemViewModel>();

		//		JObject data = JObject.Parse(json);
		//		IList<JToken> beatmaps = data["data"].Children().ToList();
		//		foreach (JToken j in beatmaps)
		//		{
		//			BeatmapsetListItemViewModel item = new BeatmapsetListItemViewModel();
		//			item.Artist = (string)j["artist"];
		//			item.BeatmapsetId = int.Parse(j["sid"].ToString());
		//			item.Title = (string)j["title"];
		//			item.Creator = (string)j["creator"];

		//			beatmapsetLists.Add(item);
		//		}
		//		beatmapsetListViewModel.Items = beatmapsetLists;

		//		return beatmapsetListViewModel;
		//	}
		//}

		#endregion

		#region Command

		public ICommand CloseCommand { get; set; }
		public ICommand OpenSettingWindowCommnad { get; set; }

		#endregion

		#region Constructor

		public MainWindowViewModel(MainWindow window)
		{
			mWindow = window;
			/*mWindow.Loaded += (sender,e) => {           
				//Located window postion
				mWindow.Top = SystemParameters.WorkArea.Height + SystemParameters.FixedFrameHorizontalBorderHeight + SystemParameters.ResizeFrameHorizontalBorderHeight - mWindow.ActualHeight - 10;
				mWindow.Left = SystemParameters.WorkArea.Width + SystemParameters.FixedFrameVerticalBorderWidth + SystemParameters.ResizeFrameVerticalBorderWidth - mWindow.ActualWidth - 10;
			};
			*/
			//Create command
			CloseCommand = new RelayCommand(() => mWindow.Hide());
			OpenSettingWindowCommnad = new RelayCommand(() => { });

			BackgroundWorker mLinkMonitor = new BackgroundWorker();
			mLinkMonitor.DoWork += LinkMonitor_DoWork;
			mLinkMonitor.RunWorkerAsync();

			SetIcon();
		}
		#endregion

		#region Methods
		private void LinkMonitor_DoWork(object sender, DoWorkEventArgs e)
		{
			NamedPipeServerStream server = new NamedPipeServerStream("osi", PipeDirection.In);
			string link = null;
			while (link != "Stop")
			{
				server.WaitForConnection();

				StreamReader sr = new StreamReader(server);
				link = sr.ReadToEnd();
				server.Disconnect();

				Uri uri = null;

				try
				{
					uri = new Uri(link);
					int beatmapsetId = LinkHelper.ToBeatmapsetId(uri);
					if (link == mPreviousLink)
					{
						Config.Current.Registry.UserBrowserRegistry.OpenUrl(uri);
					}
					else
					{
						mWindow.Dispatcher.BeginInvoke((Action)(() =>
						{
							var item = new BeatmapsetDownloadListItemViewModel(beatmapsetId);
							BeatmapsetDownloadList.Items.Add(item);
							item.Download();
							
						}));

						AnalyticsHelper.Current.TrackEventAsync(AnalyticsModel.Category.User, AnalyticsModel.Action.DownloadBeatmapset, beatmapsetId.ToString(), null);
						mPreviousLink = link;
					}
				}
				catch (LinkHelper.NotValidUri)
				{
					Config.Current.Registry.UserBrowserRegistry.OpenUrl(uri);
				}
				catch (UriFormatException) { }
			}
		}

		private void StopLinkMonitor()
		{
			NamedPipeClientStream client = new NamedPipeClientStream(".", "osi", PipeDirection.Out);
			client.Connect();
			StreamWriter sw = new StreamWriter(client);
			sw.Write("Stop");
			sw.Flush();
			sw.Close();
			client.Close();
			mNotifyIcon.Dispose();
		}

		private void SetIcon()
		{
			mNotifyIcon = new NotifyIcon();

			mNotifyIcon.Icon = Properties.Resources.osi;
			mNotifyIcon.ContextMenu = new ContextMenu();

			MenuItem exitItem = new MenuItem();
			MenuItem updateConfigItem = new MenuItem();

			updateConfigItem.Index = 0;
			updateConfigItem.Text = "更新浏览器设置";
			exitItem.Index = 1;
			exitItem.Text = "退出";
			mNotifyIcon.ContextMenu.MenuItems.Add(updateConfigItem);
			mNotifyIcon.ContextMenu.MenuItems.Add(exitItem);

			exitItem.Click += delegate
			{
				StopLinkMonitor();
				mWindow.Close();
			};
			mNotifyIcon.MouseClick += delegate (object sender, System.Windows.Forms.MouseEventArgs e)
			{
				if (e.Button == MouseButtons.Left)
				{
					mWindow.Show();

					//setBrower();
					//Windows.statusPanel statusPanel = new Windows.statusPanel();
					//statusPanel.showMessage("osu!in", "已更新默认浏览器设置", 3000, false);
				}
				else if (e.Button == MouseButtons.Right)
				{

				}
			};
			mNotifyIcon.Text = "osi";
			mNotifyIcon.Visible = true;
		}

		#endregion
	}
}
