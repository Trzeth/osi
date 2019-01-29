﻿using System;
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

		#region Private Member


		#endregion

		#region Dependency Property
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
		//你 也 是

		public bool IsOsuRunning
		{
			get { return (bool)GetValue(IsOsuRunningProperty); }
			set { SetValue(IsOsuRunningProperty, value); }
		}

		public static readonly DependencyProperty IsOsuRunningProperty = DependencyProperty.Register(nameof(IsOsuRunning), typeof(bool), typeof(SideBarControl), new UIPropertyMetadata(new bool(), IsOsuRunningChanged));

		#endregion

		public SideBarControl()
		{
			InitializeComponent();
		}

		private void UserControl_MouseEnter(object sender, MouseEventArgs e)
		{
			CircleEase cE = new CircleEase();
			cE.EasingMode = EasingMode.EaseOut;

			DoubleAnimation dA = new DoubleAnimation();
			dA.FillBehavior = FillBehavior.HoldEnd;
			if(double.IsNaN(Content.Width))
			{
				dA.From = CollapsedWidth;
			}
			dA.To = ExpandedWidth;
			dA.Duration = TimeSpan.FromMilliseconds(500);
			dA.EasingFunction = cE;

			Control.BeginAnimation(WidthProperty,dA);

		}

		private void UserControl_MouseLeave(object sender, MouseEventArgs e)
		{
			Storyboard sB = new Storyboard();
			CircleEase cE = new CircleEase();
			cE.EasingMode = EasingMode.EaseOut;

			DoubleAnimation dA = new DoubleAnimation();
			dA.FillBehavior = FillBehavior.HoldEnd;
			dA.To = CollapsedWidth;
			dA.Duration = TimeSpan.FromMilliseconds(500);
			dA.EasingFunction = cE;

			Control.BeginAnimation(WidthProperty, dA);
		}

		private static void IsOsuRunningChanged(DependencyObject d,DependencyPropertyChangedEventArgs e)
		{
			double value;

			SideBarControl sideBar = (SideBarControl)d;

			DoubleAnimation dA = new DoubleAnimation();
			dA.Duration = TimeSpan.FromMilliseconds(100);
			dA.FillBehavior = FillBehavior.Stop;

			if ((bool)e.NewValue == true)
			{
				dA.To = value = 0;
			}
			else
			{
				sideBar.IdealngBackground.Opacity = 0;
				dA.To = value = 1;

				//Opacity value is from 0 to 1
				//i must be a silly b
				//da.To = 100
				//Are you silly b????
			}
			dA.Completed += (sender, a) =>
			{
				sideBar.IdealngBackground.Opacity = value;
			};
			sideBar.IdealngBackground.BeginAnimation(OpacityProperty, dA,HandoffBehavior.Compose);

		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			IsOsuRunning = true;
			Task.Run(()=> { });
		}
	}
}
