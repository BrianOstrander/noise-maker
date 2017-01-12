using System;

namespace LunraGames.NoiseMaker
{
	public class NoiseAsset : UpdatableObject 
	{
		public string NoiseJson;

		/// <summary>
		/// Retrieves a new copy of the serialized noise.
		/// </summary>
		/// <value>The noise.</value>
		public Noise Noise 
		{
			get 
			{
				return Noise.FromJson(NoiseJson);
			}
			set
			{
				var replacement = Noise.ToJson(value);
				if (replacement != NoiseJson)
				{
					NoiseJson = replacement;
					UpdatedAt = DateTime.Now;
				}
			}
		}
	}
}