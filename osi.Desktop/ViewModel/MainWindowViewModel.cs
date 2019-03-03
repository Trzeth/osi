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
