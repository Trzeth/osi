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
using osi.Core;

namespace osi.Desktop
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
		private UpdateHelper mUpdateHelper;

		private LinkMonitor mLinkMonitor = new LinkMonitor();

#if RELEASE
		mUpdateHelper = new UpdateHelper();
#endif

		private RegistryHelper mRegistryHelper = new RegistryHelper();

		private string ProductVersion = System.Windows.Forms.Application.ProductVersion;

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			IoC.Setup();
			Task.Run(()=> mLinkMonitor.StartMointorAsync());

			//Current.MainWindow = new MessageHostWindow();
			//Current.MainWindow.ShowDialog();


			//ShutdownMode = ShutdownMode.OnExplicitShutdown;

			bool HasUpdate = false;

#if DEBUG
			HasUpdate = false;
#else
			HasUpdate = mUpdateHelper.HasUpdate(ProductVersion);
			Dispatcher.UnhandledException += Dispatcher_UnhandledException;
			TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
#endif

			Config.Current = Config.Load();

			if (HasUpdate)
			{
				new Windows.UpdateWindow(mUpdateHelper).ShowDialog();
				Process.Start($"{Environment.CurrentDirectory}/LinkMonitor.exe", "--Update Restart");
				Application.Current.Shutdown();
			}
			else
			{
				ApplyConfig();

				AnalyticsHelper.Current.TrackEventAsync(AnalyticsModel.Category.Application, AnalyticsModel.Action.Startup, null, null);

				Current.MainWindow = new MainWindow();
				Current.MainWindow.Closed += delegate
				{
					Current.Shutdown();
				};

				Current.MainWindow.Show();
			}

		}
		protected override void OnExit(ExitEventArgs e)
		{
			Config.Current.IsRunning = false;
			Config.Current.Save();

			mRegistryHelper.UnRegister(mRegistryHelper.UserBrowserRegistry);
			AnalyticsHelper.Current.TrackEventAsync(AnalyticsModel.Category.Application, AnalyticsModel.Action.Exit, null, null);

			base.OnExit(e);
		}

		private void ApplyConfig()
		{
			Config config = Config.Current = Config.Load();

			bool IsUpdated = false;
			bool IsInstall = Config.IsInstall;

			//First Run
			if (IsInstall)
			{
				Current.MainWindow = new Windows.WelcomeWindow(mRegistryHelper, RegistryHelper.osVersion);
				MainWindow.ShowDialog();

				config.Registry.osiBrowserRegistry = mRegistryHelper.osiBrowserRegistry;
				config.Registry.UserBrowserRegistry = mRegistryHelper.UserBrowserRegistry;
				config.OSVersion = RegistryHelper.osVersion;
				config.IsRunning = true;

				config.Registry.osuPath = mRegistryHelper.GetOsuPath();
				config.Guid = Guid.NewGuid();
				config.Version = ProductVersion;
			}
			else
			{
				if (config.Version != ProductVersion)
				{
					//Updated
					IsUpdated = true;
				}

				config.IsRunning = true;
				config.Save();
				mRegistryHelper.Register(config.Registry.osiBrowserRegistry);
			}

			if (!Directory.Exists(Environment.CurrentDirectory + @"\download\")) Directory.CreateDirectory(Environment.CurrentDirectory + @"\download\");
			AnalyticsHelper.Current = new AnalyticsHelper(config.Guid, config.ClientId, config.Version);

			if (IsUpdated) AnalyticsHelper.Current.TrackEventAsync(AnalyticsModel.Category.Application,AnalyticsModel.Action.Update,null,null);
			if (IsInstall) AnalyticsHelper.Current.TrackEventAsync(AnalyticsModel.Category.Application, AnalyticsModel.Action.Install, null, null);
		}

		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Windows.CrashHandleWindow crashHandleWindow = new Windows.CrashHandleWindow(e.ExceptionObject as Exception);
			crashHandleWindow.Show();
		}

		private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
		{
			e.SetObserved();

			Windows.CrashHandleWindow crashHandleWindow = new Windows.CrashHandleWindow(e.Exception);
			crashHandleWindow.Show();
		}

		private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			e.Handled = true;

			Windows.CrashHandleWindow crashHandleWindow = new Windows.CrashHandleWindow(e.Exception);
			crashHandleWindow.Show();
		}

	}
}
