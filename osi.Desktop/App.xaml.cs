using osi.Desktop.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using GoogleAnalyticsTracker.Simple;
using System.Diagnostics;

namespace osi.Desktop
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
		private UpdateHelper mUpdateHelper = new UpdateHelper();

		private RegistryHelper mRegistryHelper = new RegistryHelper();

		private AnalyticsHelper mAnalyticsHelper;

		private ConfigHelper mConfigHelper = new ConfigHelper();

		private string ProductVersion = System.Windows.Forms.Application.ProductVersion;
		public ConfigHelper ConfigHelper
		{
			get { return mConfigHelper; }
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			bool HasUpdate = mUpdateHelper.HasUpdate(ProductVersion);
#if DEBUG
			HasUpdate = false;
#endif
			if (HasUpdate)
			{
				mUpdateHelper.DownloadUpdateFile();
				Process.Start($"{Environment.CurrentDirectory}/LinkMonitor.exe","--Update Restart");

				Application.Current.Shutdown();
			}
			else
			{
				ApplyConfig();

				mAnalyticsHelper.TrackEventAsync(AnalyticsModel.Category.Application, AnalyticsModel.Action.Startup, null, null);

				Current.MainWindow = new MainWindow();
				Current.MainWindow.Show();
			}
		}

		protected override void OnExit(ExitEventArgs e)
		{
			mConfigHelper.ChangeRunningStatus(false);
			mRegistryHelper.UnRegister(mRegistryHelper.UserBrowserRegistry);
			mAnalyticsHelper.TrackEventAsync(AnalyticsModel.Category.Application, AnalyticsModel.Action.Exit, null, null);

			base.OnExit(e);
		}
		
		private void ApplyConfig()
		{
			ConfigModel configModel = null;
			bool IsUpdated = false;
			bool IsInstall = !mConfigHelper.ReadConfigFromFile();

			//First Run
			if (IsInstall)
			{
				configModel = new ConfigModel();
				mRegistryHelper.Register();
				configModel.Registry.osiBrowserRegistry = mRegistryHelper.osiBrowserRegistry;
				configModel.Registry.UserBrowserRegistry = mRegistryHelper.UserBrowserRegistry;
				configModel.OSVersion = RegistryHelper.osVersion;
				configModel.IsRunning = true;

				configModel.Registry.osuPath = mRegistryHelper.GetOsuPath();
				configModel.Guid = Guid.NewGuid();
				configModel.Version = ProductVersion;

				mConfigHelper.ConfigModel = configModel;
				mConfigHelper.SaveConfig();
			}
			else
			{
				configModel = mConfigHelper.ConfigModel;
				if (configModel.Version != ProductVersion)
				{
					//Updated
					IsUpdated = true;
				}
				mConfigHelper.ChangeRunningStatus(true);
				mConfigHelper.SaveConfig();
				configModel = mConfigHelper.ConfigModel;
				mRegistryHelper.Register(configModel.Registry.osiBrowserRegistry);
			}

			mAnalyticsHelper = new AnalyticsHelper(configModel.Guid,configModel.ClientId,configModel.Version);
			if (IsUpdated) mAnalyticsHelper.TrackEventAsync(AnalyticsModel.Category.Application,AnalyticsModel.Action.Update,null,null);
			if (IsInstall) mAnalyticsHelper.TrackEventAsync(AnalyticsModel.Category.Application, AnalyticsModel.Action.Install, null, null);
		}
	}
}
