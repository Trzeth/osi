using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using osi.Core;

namespace osi.Desktop
{
    /// <summary>
    /// SideBar.xaml 的交互逻辑
    /// </summary>
    public partial class SideBarCollapsed : UserControl
    {
        public SideBarCollapsed()
        {
            InitializeComponent();
        }

		private void BeatmapListPage_Click(object sender, RoutedEventArgs e)
		{
			IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.BeatmapListPage);
		}
		private void FavoritePage_Click(object sender, RoutedEventArgs e)
		{
			IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.FavoritePage);
		}
		private void BackupPage_Click(object sender, RoutedEventArgs e)
		{
			IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.BackupPage);
		}
		private void DownloadPage_Click(object sender, RoutedEventArgs e)
		{
			IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.DownloadPage);
		}
		private void SettingPage_Click(object sender, RoutedEventArgs e)
		{
			IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.SettingPage);
		}
	}
}
