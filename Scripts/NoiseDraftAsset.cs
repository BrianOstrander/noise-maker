using System.Linq;
using UnityEngine;

namespace LunraGames.NoiseMaker
{
	public class NoiseDraftAsset : ScriptableObject
	{
		public NoiseAsset Noise;
		public Property[] Assets;

		public NoiseDraft GetNoiseDraft(int seed = 0)
		{
			if (Noise == null) return null;
			var noise = Noise.Noise;
			noise.Seed = seed;
			var properties = noise.PropertyNodes;
			if (Assets != null)
			{
				foreach (var asset in Assets) 
				{
					var match = properties.FirstOrDefault(p => p.Name == asset.Name);
					if (match != null) match.RawPropertyValue = asset.Value;
				}
			}
			var draft = new NoiseDraft(noise);

			return draft;
		}

	}
}