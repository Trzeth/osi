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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using osi.Core;

namespace osi.Desktop
{
    /// <summary>
    /// Spinner.xaml 的交互逻辑
    /// </summary>
	public partial class SpinnerControl : UserControl
	{
		#region Dependency Properties

		public Status DownloadStatus
		{
			get { return (Status)GetValue(DownloadStatusProperty); }
			set { SetValue(DownloadStatusProperty, value); }
		}

		public static readonly DependencyProperty DownloadStatusProperty = DependencyProperty.Register(nameof(DownloadStatus), typeof(Status),typeof(SpinnerControl),new UIPropertyMetadata(new Status(),DownloadStatusChanged));

		#endregion

		public SpinnerControl()
        {
            InitializeComponent();
        }



		private static void DownloadStatusChanged(DependencyObject d,DependencyPropertyChangedEventArgs e)
		{
			if ((Status)e.NewValue == Status.Downloading)
				return;

			SpinnerControl control = (SpinnerControl)d;
			DoubleAnimation dA = new DoubleAnimation();
			dA.Duration = TimeSpan.FromMilliseconds(200);
			dA.To = 0;
			dA.FillBehavior = FillBehavior.HoldEnd;
			dA.Completed += (sender, a) => {
				control.SpinnerIcon.Visibility = Visibility.Hidden;
				control.SpinnerIcon.BeginAnimation(OpacityProperty,null);
				control.SpinnerIcon.BeginAnimation(RotateTransform.AngleProperty, null);

				dA.To = 1;
				control.CheckIcons.Visibility = Visibility.Visible;
				control.CheckIcons.BeginAnimation(OpacityProperty, dA);
			};
			control.SpinnerIcon.BeginAnimation(OpacityProperty, dA);
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
