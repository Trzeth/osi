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
    public partial class BeatmapInfoBar : UserControl
    {
		#region Dependency Properties

		public List<BeatmapInformation> BeatmapList
		{
			get { return (List<BeatmapInformation>)GetValue(BeatmapListProperty); }
			set
			{
				SetValue(BeatmapListProperty, value);
			}
		}
		public static readonly DependencyProperty BeatmapListProperty = DependencyProperty.Register(nameof(BeatmapList), typeof(List<BeatmapInformation>), typeof(BeatmapInfoBar), new UIPropertyMetadata(new List<BeatmapInformation>()));


		public double PanelWidth
		{
			get { return (double)GetValue(PanelWidthProperty); }
			set
			{
				SetValue(PanelWidthProperty, value);
			}
		}

		public static readonly DependencyProperty PanelWidthProperty = DependencyProperty.Register(nameof(PanelWidth), typeof(double), typeof(BeatmapInfoBar), new UIPropertyMetadata(new double()));

		public Geometry CardClip
		{
			get { return (Geometry)GetValue(CardClipProperty); }
			set
			{
				SetValue(CardClipProperty, value);
			}
		}

		public static readonly DependencyProperty CardClipProperty = DependencyProperty.Register(nameof(CardClip), typeof(Geometry), typeof(BeatmapInfoBar), new UIPropertyMetadata(new CombinedGeometry()));


		#endregion

		public BeatmapInfoBar()
		{
			RectangleGeometry rect = new RectangleGeometry();
			rect.Rect = new Rect();
			InitializeComponent();
		}

		private void SpinnerIcon_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{

		}

		private static void ProgressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{

		}


		private static void DownloadingStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
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
