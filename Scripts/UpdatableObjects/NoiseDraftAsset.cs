using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LunraGames.NoiseMaker
{
	public class NoiseDraftAsset : UpdatableObject
	{
		[SerializeField]
		NoiseAsset _Noise;
		[SerializeField]
		Property[] _Assets;

		public NoiseAsset Noise
		{
			get { return _Noise; }
			set
			{
				if (_Noise == value) return;

				_Noise = value;

				if (value == null)
				{
					Assets = null;
					return;
				}

				Assets = MergedAssets(Assets, _Noise);
			}
		}

		public Property[] Assets
		{
			get 
			{
				if (Noise == null || Noise.UpdatedAt < UpdatedAt) return _Assets;
				_Assets = MergedAssets(_Assets, Noise);
				UpdatedAt = DateTime.Now;
				return _Assets;
			}
			set
			{
				_Assets = value;
				UpdatedAt = DateTime.Now;
			}
		}

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

		static Property[] MergedAssets(Property[] oldAssets, NoiseAsset noise)
		{
			oldAssets = oldAssets ?? new Property[0];
			var newAssets = noise.Noise.PropertyNodes.Select(p => p.Property).ToList();

			foreach (var asset in newAssets)
			{
				var existing = oldAssets.FirstOrDefault(p => p.Name == asset.Name && p.Type == asset.Type);
				if (existing != null) asset.SetValue(existing.Value, asset.Type);
			}
			return newAssets.ToArray();
		}
	}
}