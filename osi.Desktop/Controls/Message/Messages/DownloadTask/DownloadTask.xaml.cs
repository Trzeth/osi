using osi.Core;
using osi.Core.DownloadManager;
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

namespace osi.Desktop
{
	/// <summary>
	/// DownloadMessage.xaml 的交互逻辑
	/// </summary>
	public partial class DownloadTask : BaseMessage
	{
		#region Dependency Properties

		//public DownloadingStatus DownloadStatus
		//{
		//	get { return (DownloadingStatus)GetValue(DownloadStatusProperty); }
		//	set { SetValue(DownloadStatusProperty, value); }
		//}

		//public static readonly DependencyProperty DownloadStatusProperty = DependencyProperty.Register(nameof(DownloadStatus), typeof(DownloadingStatus), typeof(DownloadTask), new UIPropertyMetadata(new DownloadingStatus(), DownloadStatusChanged));

		#endregion

		public DownloadTask()
		{
			InitializeComponent();
		}

		//private static void DownloadStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		//{
		//	DownloadTask message = (DownloadTask)d;
		//	switch ((DownloadingStatus)e.NewValue)
		//	{
		//		case DownloadingStatus.Downloading:
		//			break;
		//		case DownloadingStatus.Complete:
		//			break;
		//		case DownloadingStatus.Error:
		//			message.BackgroundPlaceHolder.Background = Brushes.Red;
		//			message.ProgressText.Visibility = Visibility.Hidden;
		//			message.ProgressPlaceHolder.Visibility = Visibility.Visible;
		//			message.ProgressPlaceHolder.Text = "错误";
		//			break;
		//	}

		//}
	}
}
