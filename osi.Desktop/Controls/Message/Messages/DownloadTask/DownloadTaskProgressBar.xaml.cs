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
			SpinnerIcon.IsVisibleChanged += SpinnerIcon_IsVisibleChanged;
		}

		private void SpinnerIcon_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if ((bool)e.NewValue)
			{
				RotateTransform rT = new RotateTransform();
				DoubleAnimation da = new DoubleAnimation();
				da.From = 0;
				da.To = 360;
				da.Duration = TimeSpan.FromMilliseconds(500);
				da.RepeatBehavior = RepeatBehavior.Forever;
				SpinnerIcon.RenderTransform = rT;

				rT.BeginAnimation(RotateTransform.AngleProperty, da);
			}
			else
			{
				SpinnerIcon.BeginAnimation(RotateTransform.AngleProperty, null);
			}
		}

		private static void ProgressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{

		}


		private static void DownloadingStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DownloadTaskProgressBar control = (DownloadTaskProgressBar)d;
			DownloadingStatus newStatus = (DownloadingStatus)e.NewValue;
			DownloadingStatus oldStatus = (DownloadingStatus)e.OldValue;

			Storyboard storyboard = new Storyboard();

			switch (newStatus)
			{
				case DownloadingStatus.Downloading:
					if(oldStatus == DownloadingStatus.Unset)
					{
						storyboard.Children.Add(FadeOutAnimation(200, control.DownloadIcon, TimeSpan.FromMilliseconds(5000)));

						storyboard.Children.Add(FadeInAnimation(200, control.SpinnerIcon,TimeSpan.FromMilliseconds(5200)));
					}
					else if (oldStatus == DownloadingStatus.Error)
					{
						storyboard.Children.Add(FadeOutAnimation(200, control.BackgroundPlaceHolder));
						storyboard.Children.Add(FadeOutAnimation(200, control.ProgressPlaceHolder));
						storyboard.Children.Add(FadeOutAnimation(200, control.RetryButton));

						storyboard.Children.Add(FadeInAnimation(200, control.ProgressText));

						storyboard.Children.Add(FadeInAnimation(200, control.SpinnerIcon, TimeSpan.FromMilliseconds(200)));

					}
					break;

				case DownloadingStatus.Complete:

					control.DownloadIcon.Visibility = Visibility.Hidden;

					storyboard.Children.Add(FadeOutAnimation(200,control.SpinnerIcon));
					storyboard.Children.Add(FadeInAnimation(200, control.CheckIcon, TimeSpan.FromMilliseconds(200)));

					break;
				case DownloadingStatus.Error:
					control.DownloadIcon.Visibility = Visibility.Hidden;

					storyboard.Children.Add(FadeOutAnimation(200, control.ProgressText));
					storyboard.Children.Add(FadeOutAnimation(200, control.SpinnerIcon));

					storyboard.Children.Add(FadeInAnimation(200, control.BackgroundPlaceHolder));
					storyboard.Children.Add(FadeInAnimation(200, control.ProgressPlaceHolder));
					storyboard.Children.Add(FadeInAnimation(200, control.RetryButton));

					control.ProgressPlaceHolder.Text = "重试";
					storyboard.Children.Add(FadeInAnimation(200, control.ProgressPlaceHolder));
					break;
				case DownloadingStatus.Cancel:

					break;
			}

			storyboard.Begin(control);
		}

		#region Animation

		public static DoubleAnimation FadeOutAnimation(float seconds,FrameworkElement frameworkElement,TimeSpan? beginTime = null)
		{
			DoubleAnimation dA = new DoubleAnimation();
			dA.Duration = TimeSpan.FromMilliseconds(seconds);
			dA.To = 0;
			if (beginTime != null) dA.BeginTime = beginTime;
			dA.DecelerationRatio = 0.9f;
			Storyboard.SetTargetName(dA, frameworkElement.Name);
			Storyboard.SetTargetProperty(dA, new PropertyPath("Opacity"));

			dA.Completed += (sender, e) => { frameworkElement.Visibility = Visibility.Hidden; };
			return dA;
		}

		public static DoubleAnimation FadeInAnimation(float seconds, FrameworkElement frameworkElement, TimeSpan? beginTime = null)
		{
			frameworkElement.Opacity = 0;
			frameworkElement.Visibility = Visibility.Visible;

			DoubleAnimation dA = new DoubleAnimation();
			dA.Duration = TimeSpan.FromMilliseconds(seconds);
			dA.To = 1;
			if (beginTime != null) dA.BeginTime = beginTime;
			dA.DecelerationRatio = 0.9f;
			Storyboard.SetTargetName(dA, frameworkElement.Name);
			Storyboard.SetTargetProperty(dA, new PropertyPath("Opacity"));
			return dA;
		}
		#endregion
	}
}
