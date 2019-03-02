using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osi.Core.DownloadManager.ApiModel
{
	public enum RankStatus
	{
		loved = 4,
		qualified = 3,
		approved = 2,
		ranked = 1,
		pending = 0,
		WIP = -1,
		graveyard = -2
	}
}
