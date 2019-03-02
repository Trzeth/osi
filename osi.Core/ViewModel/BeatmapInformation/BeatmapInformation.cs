
namespace osi.Core
{
	public class BeatmapInformation: BaseViewModel
	{
		public int BeatmapId { get; set; }

		public Mode Mode { get; set; }

		/// <summary>
		/// The title string of this version Beatmap
		/// </summary>
		public string Version { get; set; }

		/// <summary>
		/// The music second of the beatmap
		/// </summary>
		public int Length { get; set; }

		#region Map Information

		/// <summary>
		/// Circle Size CS
		/// </summary>
		public float CircleSize { get; set; }

		/// <summary>
		/// HP Drain HP
		/// </summary>
		public float HPDrain { get; set; }

		public float Accuracy { get; set; }

		/// <summary>
		/// Approach Rate AR
		/// </summary>
		public float ApproachRate { get; set; }

		public float StarDifficulty { get; set; }

		/// <summary>
		/// OverallDifficulty OD
		/// </summary>
		public float OverallDifficulty { get; set; }

		/// <summary>
		/// Star of this beatmap
		/// </summary>
		public float Star { get; set; }

		public int CircleCount { get; set; }

		public int SlidersCount { get; set; }

		public int SpinnersCount { get; set; }

		public int MaxCombo { get; set; }

		#endregion

		public int PlayCount { get; set; }

		public int PassCount { get; set; }
	}
}
