using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osi.Core
{
	public class BeatmapListItemDesignModel:BeatmapListItemViewModel
	{
		#region Singleton

		public static BeatmapListItemDesignModel Instance => new BeatmapListItemDesignModel();

		#endregion

		#region  Constructor

		public BeatmapListItemDesignModel()
		{
			BeatmapsetInformation = new BeatmapsetInformation()
			{
				BeatmapsetId = 923990,
				BPM = 218,
				Title = "White parade",
				Artist = "Umeboshi Chazuke",
				Creator = "ATing",
				Length = TimeSpan.FromSeconds(196),

				Beatmaps = new List<BeatmapInformation>
				{
					new BeatmapInformation(){ Mode = Mode.Standard},
					new BeatmapInformation(){ Mode = Mode.Mania },
				}

			};
	}

		#endregion


	}
}
