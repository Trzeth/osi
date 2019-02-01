using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace osi.Desktop
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
		private RegistryHelper mRegistryHelper = new RegistryHelper();

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			var appsettings = config.AppSettings.Settings;


			if (appsettings["FirstRun"].Value == "true")
			{
				mRegistryHelper.OpeningSettingPage += delegate { };
				mRegistryHelper.Register();

				BrowserRegistry bR = mRegistryHelper.UserBrowserRegistry;
				BrowserRegistry oR = mRegistryHelper.osiBrowserRegistry;
				if (oR.http_Hash != null)
				{
					appsettings["HttpHash"].Value = oR.http_Hash;
				}
				if (oR.https_Hash != null)
				{
					appsettings["HttpsHash"].Value = oR.https_Hash;
				}

				appsettings["FirstRun"].Value = "false";
			}
			else
			{
				mRegistryHelper.Register(new osiBrowserRegistry(appsettings["HttpHash"].Value, appsettings["HttpsHash"].Value));
			}
			config.Save();

			Current.MainWindow = new MainWindow();
			Current.MainWindow.Show();
		}

		protected override void OnExit(ExitEventArgs e)
		{
			mRegistryHelper.UnRegister(mRegistryHelper.UserBrowserRegistry);
			base.OnExit(e);
		}
	}
}
