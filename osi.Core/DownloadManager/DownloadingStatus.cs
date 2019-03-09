using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osi.Core.DownloadManager
{
	public enum DownloadingStatus
	{
		Unset,
		Downloading,
		Error,
		Complete,
		Cancel
	}
}
