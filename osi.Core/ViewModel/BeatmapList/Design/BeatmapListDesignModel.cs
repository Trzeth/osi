using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osi.Core
{
	public class BeatmapListDesignModel: BeatmapListViewModel
	{
		#region Singleton

		public static BeatmapListDesignModel Instance => new BeatmapListDesignModel();

		#endregion

		#region  Constructor

		public BeatmapListDesignModel()
		{
			Items = new List<BeatmapListItemViewModel>()
			{
				new BeatmapListItemViewModel()
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
							new BeatmapInformation(){ Mode = Mode.Taiko },
						}
					}
				},
				new BeatmapListItemViewModel()
				{
					BeatmapsetInformation = new BeatmapsetInformation()
					{
						BeatmapsetId = 910644,
						BPM = 214,
						Title = "Call of Abyss",
						Artist = "Atusu",
						Creator = "Cyberspace-",
						Length = TimeSpan.FromSeconds(214),

							Beatmaps = new List<BeatmapInformation>
							{
								new BeatmapInformation(){ Mode = Mode.Standard},
								new BeatmapInformation(){ Mode = Mode.Mania },
								new BeatmapInformation(){ Mode = Mode.Taiko}
							}
						}
				},
				new BeatmapListItemViewModel()
				{
					BeatmapsetInformation = new BeatmapsetInformation()
					{
						BeatmapsetId = 915444,
						BPM = 200,
						Title = "Sigmund",
						Artist = "Gram",
						Creator = "-[Apple]-",
						Length = TimeSpan.FromSeconds(196),

						Beatmaps = new List<BeatmapInformation>
						{
							new BeatmapInformation(){ Mode = Mode.Standard},
							new BeatmapInformation(){ Mode = Mode.Mania },
							new BeatmapInformation(){ Mode = Mode.Catch_the_Beat}
						}
					}
				},
				new BeatmapListItemViewModel()
				{

					BeatmapsetInformation = new BeatmapsetInformation()
					{
						BeatmapsetId = 921696,
						BPM = 190,
						Title = "Vous etes fatigues",
						Artist = "Billx & Strez",
						Creator = "[JOS]losffa",
						Length = TimeSpan.FromSeconds(196),

						Beatmaps = new List<BeatmapInformation>
						{
							new BeatmapInformation(){ Mode = Mode.Standard},
							new BeatmapInformation(){ Mode = Mode.Mania },
							new BeatmapInformation(){ Mode = Mode.Catch_the_Beat},
							new BeatmapInformation(){ Mode = Mode.Taiko}
						}

					}
				},
				new BeatmapListItemViewModel()
				{
					BeatmapsetInformation = new BeatmapsetInformation()
					{
						BeatmapsetId = 934371,
						BPM = 190,
						Title = "killy killy JOKER",
						Artist = "Wakeshima Kanon",
						Creator = "shuniki",
						Length = TimeSpan.FromSeconds(196),

						Beatmaps = new List<BeatmapInformation>
						{
							new BeatmapInformation(){ Mode = Mode.Standard},
							new BeatmapInformation(){ Mode = Mode.Mania },
						}
					}

				},
			};

		}

		#endregion
	}
}
