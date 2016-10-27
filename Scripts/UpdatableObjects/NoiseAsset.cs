using System;

namespace LunraGames.NoiseMaker
{
	public class NoiseAsset : UpdatableObject 
	{
		public string NoiseJson;

		/// <summary>
		/// Retrieves a new copy of the serialized graph.
		/// </summary>
		/// <value>The graph.</value>
		public Noise Noise 
		{
			get 
			{
				return Serialization.DeserializeJson<Noise>(NoiseJson, verbose: true);
			}
			set
			{
				var replacement = Serialization.SerializeJson(value, true);
				if (replacement != NoiseJson)
				{
					NoiseJson = replacement;
					UpdatedAt = DateTime.Now;
				}
			}
		}
	}
}