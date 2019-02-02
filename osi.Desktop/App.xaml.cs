using osi.Desktop.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace osi.Desktop
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
		public ConfigHelper ConfigHelper = new ConfigHelper();
		private RegistryHelper mRegistryHelper = new RegistryHelper();

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			ApplyConfig();

			Current.MainWindow = new MainWindow();
			Current.MainWindow.Show();
		}

		protected override void OnExit(ExitEventArgs e)
		{
			ConfigHelper.ChangeRunningStatus(false);
			mRegistryHelper.UnRegister(mRegistryHelper.UserBrowserRegistry);
			base.OnExit(e);
		}
		
		private void ApplyConfig()
		{
			if (!ConfigHelper.IsConfigFileExist())
			{
				ConfigModel configModel = new ConfigModel();
				mRegistryHelper.Register();
				configModel.Registry.osiBrowserRegistry = mRegistryHelper.osiBrowserRegistry;
				configModel.Registry.UserBrowserRegistry = mRegistryHelper.UserBrowserRegistry;
				configModel.OSVersion = RegistryHelper.osVersion;
				configModel.IsRunning = true;

				ConfigHelper.SaveConfig(configModel);
			}
			else
			{
				ConfigHelper.ChangeRunningStatus(true);
				mRegistryHelper.Register(ConfigHelper.GetConfigFromFile().Registry.osiBrowserRegistry);
			}
		}
	}
}
