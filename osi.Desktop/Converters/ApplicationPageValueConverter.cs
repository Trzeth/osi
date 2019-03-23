using osi.Core;
using System;
using System.Diagnostics;
using System.Globalization;

namespace osi.Desktop
{
    /// <summary>
    /// Converts the <see cref="ApplicationPage"/> to an actual view/page
    /// </summary>
    public class ApplicationPageValueConverter : BaseValueConverter<ApplicationPageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Find the appropriate page
            switch ((ApplicationPage)value)
            {
                case ApplicationPage.BeatmapListPage:
					//Todo
					return null;

				case ApplicationPage.FavoritePage:
					return null;

				case ApplicationPage.BackupPage:
					return null;

				case ApplicationPage.DownloadPage:
					return null;

				case ApplicationPage.SettingPage:
					return null;

				default:
                    Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
