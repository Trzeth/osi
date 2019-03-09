using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osi.Core
{
	public class DownloadTaskListViewModel:BaseViewModel
	{
		public AsyncObservableCollection<DownloadTaskViewModel> Items { get; set; } = new AsyncObservableCollection<DownloadTaskViewModel>();
	}
}