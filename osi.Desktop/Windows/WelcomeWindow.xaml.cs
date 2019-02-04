using osi.Desktop.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace osi.Desktop.Windows
{
	/// <summary>
	/// WelcomeWindow.xaml 的交互逻辑
	/// </summary>
	public partial class WelcomeWindow : Window
	{
		private RegistryHelper mRegistryHelper;
		private static class StepText
		{
			public const string Windows_10_Above_And_Include_Build10122 = @"请在之后打开的窗口中找到 默认程序\Web浏览器 选择 osi ";
			public const string Windows_10_Below_Build10122_Windows_8_And_Above = @"请在之后打开的窗口中选择 osi ";
			public const string Windows_7_And_Vista = @"请将 协议\http 与 https 设置为 osi ";
			public const string Windows_2000_And_Xp = @"啊咧？步骤不见了？ 肯定是你眼花了。啊咧咧？真的不见了！";
		}
		public WelcomeWindow(RegistryHelper registryHelper,OSVersion osVersion)
		{
			InitializeComponent();
			mRegistryHelper = registryHelper;

			switch (osVersion)
			{
				case OSVersion.Windows_10_Above_And_Include_Build10122:
					Steps.Text = StepText.Windows_10_Above_And_Include_Build10122;
					break;
				case OSVersion.Windows_10_Below_Build10122:
				case OSVersion.Windows_8_And_Above:
					Steps.Text = StepText.Windows_10_Below_Build10122_Windows_8_And_Above;
					break;
				case OSVersion.Windows_7_And_Vista:
					Steps.Text = StepText.Windows_7_And_Vista;
					break;
				case OSVersion.Windows_2000_And_Xp:
					Steps.Text = StepText.Windows_2000_And_Xp;
					break;
				default:
					throw new Exception("Unsupported");
			}

			CircleEase dE = new CircleEase();
			dE.EasingMode = EasingMode.EaseOut;

			DoubleAnimation dA = new DoubleAnimation(0, TimeSpan.FromMilliseconds(500));
			dA.EasingFunction = dE;
			dA.BeginTime = TimeSpan.FromSeconds(5);
			dA.Completed += delegate
			{
				WelcomePage.Visibility = Visibility.Hidden;
			};
			WelcomePage.BeginAnimation(OpacityProperty, dA);
		}

		private void AccpetButton_Click(object sender, RoutedEventArgs e)
		{
			CircleEase dE = new CircleEase();
			dE.EasingMode = EasingMode.EaseOut;

			DoubleAnimation dA = new DoubleAnimation(0, TimeSpan.FromMilliseconds(500));
			dA.EasingFunction = dE;

			dA.Completed += delegate
			{
				AnalyticsPage.Visibility = Visibility.Hidden;
			};
			AnalyticsPage.BeginAnimation(OpacityProperty, dA);

		}
		private void StartSetting(object sender, RoutedEventArgs e)
		{
			BackgroundWorker backgroundWorker = new BackgroundWorker();
			backgroundWorker.DoWork += delegate
			{
				mRegistryHelper.Register();
			};
			backgroundWorker.RunWorkerCompleted += delegate
			{
				this.Close();
			};
			backgroundWorker.RunWorkerAsync();
		}

		private void RejectButton_Click(object sender, RoutedEventArgs e)
		{
			RejectMessage.Visibility = Visibility.Visible;
		}
	}
}
