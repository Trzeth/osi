using osi.Core;
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
	public partial class DownloadMessage : BaseMessage
	{
		#region Dependency Properties

		public Status DownloadStatus
		{
			get { return (Status)GetValue(DownloadStatusProperty); }
			set { SetValue(DownloadStatusProperty, value); }
		}

		public static readonly DependencyProperty DownloadStatusProperty = DependencyProperty.Register(nameof(DownloadStatus), typeof(Status), typeof(DownloadMessage), new UIPropertyMetadata(new Status(), DownloadStatusChanged));

		#endregion

		public DownloadMessage()
		{
			InitializeComponent();
		}
		private static void DownloadStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DownloadMessage message = (DownloadMessage)d;
			switch ((Status)e.NewValue)
			{
				case Status.Downloading:
					break;
				case Status.Finished:
					break;
				case Status.Error:

					message.BackgroundPlaceHolder.Background = Brushes.Red;
					message.ProgressText.Visibility = Visibility.Hidden;
					message.ProgressPlaceHolder.Visibility = Visibility.Visible;
					message.ProgressPlaceHolder.Text = "错误";
					break;
			}

		}
	}
}
