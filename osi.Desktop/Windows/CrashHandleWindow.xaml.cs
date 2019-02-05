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
using System.Windows.Shapes;

namespace osi.Desktop.Windows
{
	/// <summary>
	/// CrashHandleWindow.xaml 的交互逻辑
	/// </summary>
	public partial class CrashHandleWindow : Window
	{
		public CrashHandleWindow(Exception e)
		{
			InitializeComponent();

			StringBuilder sB = new StringBuilder();
			sB.AppendLine("OSI"+ App.ConfigHelper.ConfigModel.Version);
			sB.AppendLine(e.Source);
			sB.AppendLine(e.ToString());
			sB.AppendLine(e.Message);
			foreach(KeyValuePair<string,string> pair in e.Data)
			{
				sB.AppendLine($"Key:{pair.Key}\nValue{pair.Value}");
			}
			sB.AppendLine(e.StackTrace);

			var iex = e;
			if (iex != null)
			{
				sB.AppendLine("------------------------");
				iex = iex.InnerException;
				sB.AppendLine(iex.Source);
				sB.AppendLine(iex.ToString());
				sB.AppendLine(iex.Message);

				foreach (KeyValuePair<string, string> pair in iex.Data)
				{
					sB.AppendLine($"Key:{pair.Key}\nValue{pair.Value}");
				}
				sB.AppendLine(iex.StackTrace);
			}
			ExpectionTextBox.Text = sB.ToString();
			//Log Information
		}
	}
}
