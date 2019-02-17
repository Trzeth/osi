using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace osi.Desktop
{
    public static class StoryboradHelper
    {
		public static void AddSlideFromRight(this Storyboard storyboard,TimeSpan second,double offset,float decelerationRatio = 0.9f)
		{
			ThicknessAnimation tA = new ThicknessAnimation()
			{
				Duration = second,
				From = new Thickness(offset, 0, -offset, 0),
				To = new Thickness(0),
				DecelerationRatio = decelerationRatio
			};

			Storyboard.SetTargetProperty(tA,new PropertyPath("Margin"));
			storyboard.Children.Add(tA);
		}
		public static void AddSlideToRight(this Storyboard storyboard, TimeSpan second, double offset, float decelerationRatio = 0.9f)
		{
			ThicknessAnimation tA = new ThicknessAnimation()
			{
				Duration = second,
				To = new Thickness(offset, 0, -offset, 0),
				DecelerationRatio = decelerationRatio
			};

			Storyboard.SetTargetProperty(tA, new PropertyPath("Margin"));
			storyboard.Children.Add(tA);
		}

		public static void AddFadeIn(this Storyboard storyboard, TimeSpan second, float decelerationRatio = 0.9f)
		{
			DoubleAnimation dA = new DoubleAnimation()
			{
				Duration = second,
				From = 0,
				To = 100,
				DecelerationRatio = decelerationRatio
			};

			Storyboard.SetTargetProperty(dA, new PropertyPath("Opacity"));
			storyboard.Children.Add(dA);
		}
		public static void AddFadeOut(this Storyboard storyboard, TimeSpan second, float decelerationRatio = 0.9f)
		{
			DoubleAnimation dA = new DoubleAnimation()
			{
				Duration = second,
				From = 100,
				To = 0,
				DecelerationRatio = decelerationRatio
			};

			Storyboard.SetTargetProperty(dA, new PropertyPath("Opacity"));
			storyboard.Children.Add(dA);
		}
	}
}
