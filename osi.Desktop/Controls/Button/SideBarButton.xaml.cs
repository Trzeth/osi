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
    /// SideBarButton.xaml 的交互逻辑
    /// </summary>
    public partial class SideBarButton : UserControl
    {
		public double NormalWidth
		{
			get { return (double)GetValue(NormalWidthProperty); }
			set { SetValue(NormalWidthProperty, value); }
		}

		public static readonly DependencyProperty NormalWidthProperty = DependencyProperty.Register(nameof(NormalWidth), typeof(double), typeof(SideBarButton), new UIPropertyMetadata(new double()));


		public Color ForegroundColor
		{
			get { return (Color)GetValue(ForegroundColorProperty); }
			set { SetValue(ForegroundColorProperty, value); }
		}

		public static readonly DependencyProperty ForegroundColorProperty = DependencyProperty.Register(nameof(ForegroundColor), typeof(Color), typeof(SideBarButton), new UIPropertyMetadata(new Color()));

		public Color ForegroundAfterColor
		{
			get { return (Color)GetValue(ForegroundAfterColorProperty); }
			set { SetValue(ForegroundAfterColorProperty, value); }
		}

		public static readonly DependencyProperty ForegroundAfterColorProperty = DependencyProperty.Register(nameof(ForegroundAfterColor), typeof(Color), typeof(SideBarButton), new UIPropertyMetadata(new Color()));

		public Color BackgroundAfterColor
		{
			get { return (Color)GetValue(BackgroundAfterColorProperty); }
			set { SetValue(BackgroundAfterColorProperty, value); }
		}

		public static readonly DependencyProperty BackgroundAfterColorProperty = DependencyProperty.Register(nameof(BackgroundAfterColor), typeof(Color), typeof(SideBarButton), new UIPropertyMetadata(new Color()));

		public string IconString
		{
			get { return (string)GetValue(IconStringProperty); }
			set { SetValue(IconStringProperty, value); }
		}

		public static readonly DependencyProperty IconStringProperty = DependencyProperty.Register(nameof(IconString), typeof(string), typeof(SideBarButton), new UIPropertyMetadata(""));

		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(SideBarButton), new UIPropertyMetadata(""));
		public bool IsChecked
		{
			get { return (bool)GetValue(IsCheckedProperty); }
			set { SetValue(IsCheckedProperty, value); }
		}

		public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(SideBarButton), new UIPropertyMetadata(new bool()));

		public SideBarButton()
        {
            InitializeComponent();
        }

		private void Grid_MouseEnter(object sender, MouseEventArgs e)
		{
			if(IsChecked != true)
			{
				AfterIconAndText.Visibility = Visibility.Visible;
				AfterIconAndText.Width = 0;
			}

			DoubleAnimation dA = new DoubleAnimation();
			dA.To = ActualWidth;
			dA.Duration = TimeSpan.FromMilliseconds(500);
			dA.DecelerationRatio = 0.5f;

			AfterIconAndText.BeginAnimation(WidthProperty, dA);
		}

		private void Grid_MouseLeave(object sender, MouseEventArgs e)
		{
			DoubleAnimation dA = new DoubleAnimation();
			if(IsChecked == true)
			{
				dA.To = NormalWidth;
			}
			else
			{
				dA.To = 0;
			}
			dA.Duration = TimeSpan.FromMilliseconds(500);
			dA.DecelerationRatio = 0.5f;
			AfterIconAndText.BeginAnimation(WidthProperty, dA);
		}

		private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
		{

		}

		private void RadioButton_Checked(object sender, RoutedEventArgs e)
		{

		}

		private void RadioButton_Unchecked(object sender, RoutedEventArgs e)
		{

		}
	}
}
