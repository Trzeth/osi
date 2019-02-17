using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows;

namespace osi.Desktop
{
    public static class MessageAnimations
    {
		/// <summary>
		/// Slide And Fade In From Right
		/// </summary>
		/// <param name="message">this Message</param>
		/// <param name="second">Time to animate</param>
		/// <returns></returns>
		public async static Task SlideAndFadeInFromRight(this BaseMessage message,float second)
		{
			TimeSpan tS = TimeSpan.FromSeconds(second);
			Storyboard sB = new Storyboard();

			sB.AddSlideFromRight(tS, message.MessageHostWidth);
			sB.AddFadeIn(tS);

			sB.Begin(message);
			message.Visibility = Visibility.Visible;

			await Task.Delay(TimeSpan.FromSeconds(second));
		}

		/// <summary>
		/// Slide And Fade In To Right
		/// </summary>
		/// <param name="message">this Message</param>
		/// <param name="second">Time to animate</param>
		/// <returns></returns>
		public async static Task SlideAndFadeOutToRight(this BaseMessage message, float second)
		{
			TimeSpan tS = TimeSpan.FromSeconds(second);
			Storyboard sB = new Storyboard();

			sB.AddSlideToRight(tS, message.MessageHostWidth);
			sB.AddFadeOut(tS);

			sB.Begin(message);
			message.Visibility = Visibility.Visible;

			await Task.Delay(TimeSpan.FromSeconds(second));
		}

	}
}
