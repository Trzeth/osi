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

		public List<Mode> Modes
		{
			get { return (List<Mode>)GetValue(ModesProperty); }
			set
			{
				SetValue(ModesProperty, value);
			}
		}
		public static readonly DependencyProperty ModesProperty = DependencyProperty.Register(nameof(Modes), typeof(List<Mode>), typeof(BeatmapInfoBar), new UIPropertyMetadata(new List<Mode>(), ModesChanged));

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

		public bool IsExpanded
		{
			get { return (bool)GetValue(IsExpandedProperty); }
			set
			{
				SetValue(IsExpandedProperty, value);
			}
		}

		public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(nameof(IsExpanded), typeof(bool), typeof(BeatmapInfoBar), new UIPropertyMetadata(new bool(),OnIsExpandedChanged));

		#endregion

		public BeatmapInfoBar()
		{
			InitializeComponent();
		}

		private static void ModesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue == null)
				return;


			BeatmapInfoBar bar = (BeatmapInfoBar)d;
			List<Mode> list = (List<Mode>)e.NewValue;

			Mode[] mode = { Mode.Standard, Mode.Mania, Mode.Taiko,Mode.Catch_the_Beat };

			int count = 0;
			foreach (Mode m in mode)
			{
				if (list.Exists((x) => x == m))
				{

					Grid grid = GetBeatmapInfoBar(m, bar);

					grid.MouseEnter += bar.Grid_MouseEnter;
					grid.MouseLeave += bar.Grid_MouseLeave;

					count++;

				}
				else
				{
					GetBeatmapInfoBar(m, bar).Visibility = Visibility.Collapsed;
				}
			}
		}

		#region Grid Animation

		private void Grid_MouseEnter(object sender, MouseEventArgs e)
		{
			Grid grid = (Grid)sender;

			DoubleAnimation dAG = new DoubleAnimation();

			dAG.To = 65;
			dAG.DecelerationRatio = 0.5f;
			dAG.Duration = TimeSpan.FromMilliseconds(400);

			grid.BeginAnimation(WidthProperty, dAG);
			IsExpanded = true;
		}

		private void Grid_MouseLeave(object sender, MouseEventArgs e)
		{
			Grid grid = (Grid)sender;

			DoubleAnimation dA = new DoubleAnimation();

			dA.To = 15;
			dA.DecelerationRatio = 0.5f;
			dA.Duration = TimeSpan.FromMilliseconds(400);

			grid.BeginAnimation(WidthProperty, dA);
			IsExpanded = false;
		}

		private static Grid GetBeatmapInfoBar(Mode m, BeatmapInfoBar bar)
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

		private static void OnIsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			//Mode[] mode = { Mode.Standard, Mode.Mania, Mode.Taiko, Mode.Catch_the_Beat };
			//foreach (Mode m in mode)
			//{
			//	Grid grid = GetBeatmapInfoBar(m, (BeatmapInfoBar)d);
			//	if (grid.Tag.ToString() == "First")
			//	{
			//		DoubleAnimation dA = new DoubleAnimation();
			//		dA.DecelerationRatio = 0.5f;
			//		dA.Duration = TimeSpan.FromMilliseconds(400);

			//		if ((bool)e.NewValue)
			//		{
			//			dA.To = 15;
			//		}
			//		else
			//		{
			//			dA.To = 65;
			//		}

			//		grid.BeginAnimation(WidthProperty, dA);
			//		break;
			//	}
			//}
		}

		#endregion

	}
}
