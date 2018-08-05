using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace osi_Desktop
{
    /// <summary>
    /// Guide.xaml 的交互逻辑
    /// </summary>
    public partial class Guide : Window
    {
        ThicknessAnimation ta = new ThicknessAnimation();
        Grid[] pageCollection;
        Rectangle[] bannerBackgroundCollection;
        Color[] colorCollection;
        int currentPage = 0;
        bool isClicked = false;

        public Guide()
        {
            InitializeComponent();

            pageCollection = new Grid[] {Page1,Page2,Page3 };
            bannerBackgroundCollection = new Rectangle[] { Page1_Background, Page2_Background, Page3_Background };
            ta.Duration = new Duration(TimeSpan.FromMilliseconds(300));
            ta.DecelerationRatio = 0.5;
        }
        protected override void OnInitialized(EventArgs e)
        {
            ThicknessAnimation ta = new ThicknessAnimation();
            ta.DecelerationRatio = 0.4;
            ta.Duration = new Duration(TimeSpan.FromMilliseconds(450));
            ta.FillBehavior = FillBehavior.HoldEnd;

            ta.To = new Thickness(0);
            ta.From = new Thickness(-Banner.Width, 0, 0, 0);
            Banner.BeginAnimation(MarginProperty, ta);

            for (int i = 0; i < Pages.Children.Count; i++)
            {
                if (Pages.Children[i] != button)
                {
                    Grid g = (Grid)Pages.Children[i];
                    ta.From = new Thickness(-(g.Width + 20), 0, 0, 0);
                    ta.To = new Thickness(80, 0, 0, 0);
                    ta.BeginTime = TimeSpan.FromMilliseconds(30 * i);
                    g.BeginAnimation(MarginProperty, ta);
                }
                else
                {
                    Button b = (Button)Pages.Children[i];
                    ta.From = new Thickness(-(b.Width + 20), 230, 0, 0);
                    ta.To = new Thickness(430, 230, 0, 0);
                    ta.BeginTime = TimeSpan.FromMilliseconds(30 * (i -1));
                    b.BeginAnimation(MarginProperty, ta);
                }
            }

            base.OnInitialized(e);
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < 3)
            {
                ta.To = new Thickness(-(pageCollection[currentPage].Width + 5), 0, 0, 0);
                pageCollection[currentPage].BeginAnimation(MarginProperty, ta);

                if (currentPage < 2)
                {
                    DoubleAnimation da = new DoubleAnimation();
                    da.To = 1;
                    da.Duration = new Duration(TimeSpan.FromMilliseconds(300));
                    da.DecelerationRatio = 0.5;
                    bannerBackgroundCollection[currentPage + 1].BeginAnimation(OpacityProperty,da);
                }

                if (currentPage == 2)
                {
                    ta.To = new Thickness(Width, 0, 0, 0);
                    DoubleAnimation da = new DoubleAnimation();
                    da.To = Width;
                    da.Duration = new Duration(TimeSpan.FromMilliseconds(300));
                    da.DecelerationRatio = 0.5;
                    da.Completed += delegate {
                        button.Visibility = Visibility.Hidden;
                        ta.Completed += delegate { Close(); };
                        Banner.BeginAnimation(MarginProperty, ta);
                    };
                    Banner.BeginAnimation(WidthProperty, da);
                }
            }

            currentPage += 1;
            isClicked = true;
        }
        private void button_MouseEnter(object sender, MouseEventArgs e)
        {
            ta.To = new Thickness(70,0,0,0);
            pageCollection[currentPage].BeginAnimation(MarginProperty, ta);
        }
        private void button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (isClicked)
            {
                isClicked = false;
            }
            else
            {
                ta.To = new Thickness(80, 0, 0, 0);
                pageCollection[currentPage].BeginAnimation(MarginProperty, ta);
            }

        }
    }
}
