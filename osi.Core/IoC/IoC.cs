using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace osi.Core
{
	public static class IoC
	{
		public static IKernel Kernel { get; private set; } = new StandardKernel();

		public static void Setup()
		{
			BindViewModels();
		}

		private static void BindViewModels()
		{
			Kernel.Bind<DownloadTaskListViewModel>().ToConstant(new DownloadTaskListViewModel());
			Kernel.Bind<ApplicationViewModel>().ToConstant(new ApplicationViewModel());
		}

		public static T Get<T>()
		{
			return Kernel.Get<T>();
		}
	}
}
