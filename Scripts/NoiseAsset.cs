using UnityEngine;
using System.Collections.Generic;

namespace LunraGames.NoiseMaker
{
	public class NoiseAsset : ScriptableObject 
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
				NoiseJson = Serialization.SerializeJson(value, true);
			}
		}
	}
}