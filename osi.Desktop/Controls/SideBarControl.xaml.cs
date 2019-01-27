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

namespace osi.Desktop
{
	/// <summary>
	/// SideBar.xaml 的交互逻辑
	/// </summary>
	public partial class SideBarControl : UserControl
	{
		#region Dependency Property
		private bool mIsMouseEnter;

		public double ExpandedWidth
		{
			get { return (double)GetValue(ExpandedWidthProperty); }
			set { SetValue(ExpandedWidthProperty, value); }
		}

		public static readonly DependencyProperty ExpandedWidthProperty = DependencyProperty.Register(nameof(ExpandedWidth),typeof(double),typeof(SideBarControl),new PropertyMetadata(new double()));
		//就因为要初始化？？？？？
		//破玩意弄了我一下个下午 打死也赋不上值（摔


		public double CollapsedWidth
		{
			get { return (double)GetValue(CollapsedWidthProperty); }
			set { SetValue(CollapsedWidthProperty, value); }
		}

		public static readonly DependencyProperty CollapsedWidthProperty = DependencyProperty.Register(nameof(CollapsedWidth), typeof(double), typeof(SideBarControl), new PropertyMetadata(new double()));

		#endregion

		public SideBarControl()
		{
			InitializeComponent();
		}

		private void UserControl_MouseEnter(object sender, MouseEventArgs e)
		{
			mIsMouseEnter = true;
			Width = ExpandedWidth;
			CircleEase cE = new CircleEase();
			cE.EasingMode = EasingMode.EaseOut;

			DoubleAnimation dA = new DoubleAnimation();
			if(double.IsNaN(Content.Width))
			{
				dA.From = CollapsedWidth;
			}
			dA.To = ExpandedWidth;
			dA.Duration = TimeSpan.FromMilliseconds(500);
			dA.EasingFunction = cE;

			Content.BeginAnimation(WidthProperty,dA);

		}

		private void UserControl_MouseLeave(object sender, MouseEventArgs e)
		{
			mIsMouseEnter = false;

			CircleEase cE = new CircleEase();
			cE.EasingMode = EasingMode.EaseOut;

			DoubleAnimation dA = new DoubleAnimation();
			dA.To = CollapsedWidth;
			dA.Duration = TimeSpan.FromMilliseconds(500);
			dA.EasingFunction = cE;

			dA.Completed += (a, b) => {
				if(!mIsMouseEnter)Width = CollapsedWidth;
			};
			Content.BeginAnimation(WidthProperty, dA);

		}
	}
}
