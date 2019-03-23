using osi.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osi.Desktop
{
	public class ViewModelLocator
	{
		public static ViewModelLocator Instance { get; private set; } = new ViewModelLocator();

		public DownloadTaskListViewModel DownloadTaskListViewModel => IoC.Get<DownloadTaskListViewModel>();

		public ApplicationViewModel ApplicationViewModel => IoC.Get<ApplicationViewModel>();
	}
}
