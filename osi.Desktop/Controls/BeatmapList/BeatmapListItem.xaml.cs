﻿using osi.Core;
using osi.Core.DownloadManager;
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
using System.Windows.Shapes;

namespace osi.Desktop
{
	/// <summary>
	/// DownloadMessage.xaml 的交互逻辑
	/// </summary>
	public partial class BeatmapListItem : BaseMessage
	{
		#region Dependency Properties

		#endregion

		public BeatmapListItem()
		{
			InitializeComponent();
			this.SlideAndFadeInFromRight(200);
		}
	}
}