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
		public static readonly DependencyProperty BeatmapListProperty = DependencyProperty.Register(nameof(BeatmapList), typeof(List<BeatmapInformation>), typeof(BeatmapInfoBar), new UIPropertyMetadata(new List<BeatmapInformation>(),BeatmapListChanged));

		public double PanelWidth
		{
			get { return (double)GetValue(PanelWidthProperty); }
			set
			{
				SetValue(PanelWidthProperty, value);
			}
		}

		public static readonly DependencyProperty PanelWidthProperty = DependencyProperty.Register(nameof(PanelWidth), typeof(double), typeof(BeatmapInfoBar), new UIPropertyMetadata(new double()));

		public CornerRadius CornerRadius
		{
			get { return (CornerRadius)GetValue(CornerRadiusProperty); }
			set
			{
				SetValue(CornerRadiusProperty, value);
			}
		}

		public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius),typeof(CornerRadius),typeof(BeatmapInfoBar),new UIPropertyMetadata(new CornerRadius()));


		#endregion

		public BeatmapInfoBar()
		{
			InitializeComponent();
		}

		private static void BeatmapListChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue == null)
				return;


			BeatmapInfoBar bar = (BeatmapInfoBar)d;
			List<BeatmapInformation> list = (List<BeatmapInformation>)e.NewValue;

			Mode[] mode = { Mode.Standard, Mode.Mania, Mode.Taiko,Mode.Catch_the_Beat };

			int count = 0;
			foreach (Mode m in mode)
			{
				if (list.Exists((x) => x.Mode == m))
				{

					Grid grid = GetBeatmapInfoBar(m, bar);

					grid.MouseEnter += Grid_MouseEnter;
					grid.MouseLeave += Grid_MouseLeave;

					count++;

				}
				else
				{
					GetBeatmapInfoBar(m, bar).Visibility = Visibility.Collapsed;
				}
			}
		}

		private static void Grid_MouseEnter(object sender, MouseEventArgs e)
		{
			Grid grid = (Grid)sender;

			DoubleAnimation dAG = new DoubleAnimation();

			dAG.To = 65;
			dAG.DecelerationRatio = 0.5f;
			dAG.Duration = TimeSpan.FromMilliseconds(400);

			grid.BeginAnimation(WidthProperty, dAG);
		}

		private static void Grid_MouseLeave(object sender, MouseEventArgs e)
		{
			Grid grid = (Grid)sender;

			DoubleAnimation dA = new DoubleAnimation();

			dA.To = 15;
			dA.DecelerationRatio = 0.5f;
			dA.Duration = TimeSpan.FromMilliseconds(400);

			grid.BeginAnimation(WidthProperty, dA);
		}


		private static Grid GetBeatmapInfoBar(Mode m,BeatmapInfoBar bar)
		{
			switch (m)
			{
				case Mode.Standard:
					return bar.Standard;
				case Mode.Mania:
					return bar.Mania;
				case Mode.Taiko:
					return bar.Taiko;
				case Mode.Catch_the_Beat:
					return bar.Ctb;
				default:
					return null;
			}
		}
	}
}
