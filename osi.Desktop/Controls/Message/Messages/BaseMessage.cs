using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace osi.Desktop
{
    public class BaseMessage:UserControl
    {
		#region Public Members

		public double MessageHostWidth;
		public double MessageHostHeight;

		public MessageAnimation MessageLoadAnimation { get; set; } = MessageAnimation.SlideAndFadeInFromRight;
		public MessageAnimation MessageUnloadAnimation { get; set; } = MessageAnimation.SlideAndFadeOutToRight;

		public float DurationSecond = 0;

		public float SlideSecond = 0.8f;

		#endregion


		#region Constructor

		public BaseMessage()
		{
			if (MessageLoadAnimation != MessageAnimation.None)
				Visibility = Visibility.Collapsed;

			Loaded += BaseMessage_Loaded;
		}

		#endregion

		private async void BaseMessage_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			await AnimateIn();
		}


		#region Animate In/Out

		public async Task AnimateIn()
		{
			if (MessageLoadAnimation == MessageAnimation.None)
				return;

			switch (MessageLoadAnimation)
			{
				case MessageAnimation.SlideAndFadeInFromRight:

					//Begin Animation
					await this.SlideAndFadeInFromRight(SlideSecond);

					break;
			}
		}

		public async Task AnimateOut()
		{
			if (MessageLoadAnimation == MessageAnimation.None)
				return;

			switch (MessageLoadAnimation)
			{
				case MessageAnimation.SlideAndFadeOutToRight:

					//Begin Animation   
					await this.SlideAndFadeOutToRight(SlideSecond);

					break;
			}
		}

		#endregion
	}
}
