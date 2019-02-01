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
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			var appsettings = config.AppSettings.Settings;

			if (appsettings["FirstRun"].Value == "true")
			{
				string httpHash, httpsHash;

				RegisterAsDefaultBrowserFunction.Register(out httpHash, out httpsHash);
				if (httpHash != null)
				{
					appsettings["HttpHash"].Value = httpHash;
				}
				if (httpsHash != null)
				{
					appsettings["HttpsHash"].Value = httpsHash;
				}

				appsettings["FirstRun"].Value = "false";
			}
			else
			{
				RegisterAsDefaultBrowserFunction.RegisterAsDefaultBrowser(appsettings["HttpHash"].Value, appsettings["HttpsHash"].Value);
			}
			config.Save();

			Current.MainWindow = new MainWindow();
			Current.MainWindow.Show();
		}
	}
}
