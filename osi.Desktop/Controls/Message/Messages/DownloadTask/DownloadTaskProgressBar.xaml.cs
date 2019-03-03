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
using osi.Core.DownloadManager;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using osi.Core;

namespace osi.Desktop
{
    /// <summary>
    /// DownloadTaskProgressBar.xaml 的交互逻辑
    /// </summary>
    public partial class DownloadTaskProgressBar : UserControl
    {
		#region Dependency Properties

		public DownloadingStatus DownloadingStatus
		{
			get { return (DownloadingStatus)GetValue(DownloadingStatusProperty); }
			set { SetValue(DownloadingStatusProperty, value); }
		}

		public static readonly DependencyProperty DownloadingStatusProperty = DependencyProperty.Register(nameof(DownloadingStatus), typeof(DownloadingStatus), typeof(DownloadTaskProgressBar), new UIPropertyMetadata(new DownloadingStatus(), DownloadingStatusChanged));

		public float Progress
		{
			get { return (float)GetValue(ProgressProperty); }
			set { SetValue(ProgressProperty, value); }
		}

		public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register(nameof(Progress), typeof(float), typeof(DownloadTaskProgressBar), new UIPropertyMetadata(new float(), ProgressChanged));

		public GridLength SideBarWidth
		{
			get { return (GridLength)GetValue(SideBarWidthProperty); }
			set
			{
				SetValue(SideBarWidthProperty, value);
			}
		}

		public static readonly DependencyProperty SideBarWidthProperty = DependencyProperty.Register(nameof(SideBarWidth), typeof(GridLength), typeof(DownloadTaskProgressBar), new UIPropertyMetadata(new GridLength(0.15,GridUnitType.Star)));

		public GridLength ProgressBarHeight
		{
			get { return (GridLength)GetValue(ProgressBarHeightProperty); }
			set { SetValue(ProgressBarHeightProperty, value); }
		}

		public static readonly DependencyProperty ProgressBarHeightProperty = DependencyProperty.Register(nameof(ProgressBarHeight), typeof(GridLength), typeof(DownloadTaskProgressBar), new UIPropertyMetadata(new GridLength(0.15,GridUnitType.Star)));

		public float RetryCommand
		{
			get { return (float)GetValue(RetryCommandProperty); }
			set { SetValue(RetryCommandProperty, value); }
		}

		public static readonly DependencyProperty RetryCommandProperty = DependencyProperty.Register(nameof(RetryCommand), typeof(ICommand), typeof(DownloadTaskProgressBar), new PropertyMetadata());


		#endregion

		public DownloadTaskProgressBar()
		{
			InitializeComponent();
		}

		private static void ProgressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{

		}


		private static void DownloadingStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			switch ((DownloadingStatus)e.NewValue)
			{
				case DownloadingStatus.Downloading:
					break;
				case DownloadingStatus.Complete:

					DownloadTaskProgressBar control = (DownloadTaskProgressBar)d;
					DoubleAnimation dA = new DoubleAnimation();
					dA.Duration = TimeSpan.FromMilliseconds(200);
					dA.To = 0;
					dA.FillBehavior = FillBehavior.HoldEnd;

					dA.Completed += (sender, a) => {
						control.SpinnerIcon.Visibility = Visibility.Hidden;
						control.SpinnerIcon.BeginAnimation(OpacityProperty, null);
						control.SpinnerIcon.BeginAnimation(RotateTransform.AngleProperty, null);

						dA.To = 1;
						control.CheckIcon.Visibility = Visibility.Visible;
						control.CheckIcon.BeginAnimation(OpacityProperty, dA);
					};

					control.SpinnerIcon.BeginAnimation(OpacityProperty, dA);

					break;
				case DownloadingStatus.Error:

					break;
			}
		}

		/// <summary>
		/// Begin Spinner Rotating
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Control_Loaded(object sender, RoutedEventArgs e)
		{
			DoubleAnimation dA = new DoubleAnimation();
			RotateTransform rT = new RotateTransform();

			dA.Duration = TimeSpan.FromMilliseconds(500);
			dA.From = 0;
			dA.To = 360;
			dA.RepeatBehavior = RepeatBehavior.Forever;

			SpinnerIcon.RenderTransform = rT;
			rT.BeginAnimation(RotateTransform.AngleProperty, dA);
		}
	}
}
