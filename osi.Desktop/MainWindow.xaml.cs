using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using osi.Core;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json.Serialization;
using System.Windows.Forms;

namespace osi.Desktop
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Private Member

		private NotifyIcon notifyIcon;
		private BackgroundWorker mLinkMonitor;
		private List<BeatmapsetDownloadListItemViewModel> items;

		#endregion

		#region  Constructor

		public MainWindow()
		{
			InitializeComponent();

			this.DataContext = new MainWindowViewModel(this);

			BeatmapsetDownloadListViewModel mListViewModel = new BeatmapsetDownloadListViewModel();
			mListViewModel.Items = items = new List<BeatmapsetDownloadListItemViewModel>();

			mLinkMonitor = new BackgroundWorker();
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
				try
				{
					items.Add(new BeatmapsetDownloadListItemViewModel(LinkHelper.ToBeatmapsetId(new Uri(link))));
				}
				catch (LinkHelper.NotValidUri)
				{
					System.Diagnostics.Process.Start(link);
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
			notifyIcon.Dispose();
		}
		private void SetIcon()
		{
			notifyIcon = new NotifyIcon();

			notifyIcon.Icon = Properties.Resources.osi;
			notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu();

			System.Windows.Forms.MenuItem exitItem = new System.Windows.Forms.MenuItem();
			System.Windows.Forms.MenuItem updateConfigItem = new System.Windows.Forms.MenuItem();

			updateConfigItem.Index = 0;
			updateConfigItem.Text = "更新浏览器设置";
			exitItem.Index = 1;
			exitItem.Text = "退出";
			notifyIcon.ContextMenu.MenuItems.Add(updateConfigItem);
			notifyIcon.ContextMenu.MenuItems.Add(exitItem);

			exitItem.Click += delegate
			{
				StopLinkMonitor();
				this.Close();
			};
			notifyIcon.MouseClick += delegate (object sender, System.Windows.Forms.MouseEventArgs e)
			{
				if (e.Button == MouseButtons.Left)
				{
					this.Show();

					//setBrower();
					//Windows.statusPanel statusPanel = new Windows.statusPanel();
					//statusPanel.showMessage("osu!in", "已更新默认浏览器设置", 3000, false);
				}
				else if (e.Button == MouseButtons.Right)
				{

				}
			};
			notifyIcon.Text = "osi";
			notifyIcon.Visible = true;
		}

		#endregion

		public new void Hide()
		{
			base.Hide();
		}
		public new void Close()
		{
			if (Visibility == Visibility.Visible)
			{
				Hide();
			}
			base.Close();
		}
	}
}
