using System;
using System.Linq;
using LunraGames.NumberDemon;
using UnityEngine;

namespace LunraGames.NoiseMaker
{
	public class EchoAsset : UpdatableObject
	{
		[SerializeField]
		NoiseAsset _Noise;
		[SerializeField]
		Property[] _Properties;
		public int Seed;
		public Vector3 Translation = Vector3.zero;
		public Vector3 Rotation = Vector3.zero;
		public Vector3 Scale = Vector3.one;

		public NoiseAsset NoiseAsset
		{
			get { return _Noise; }
			set
			{
				if (_Noise == value) return;

				_Noise = value;
				UpdatedAt = DateTime.Now;

				if (value == null)
				{
					Properties = null;
					return;
				}
				var noise = _Noise.Noise;
				Properties = MergedProperties(Properties, noise);
			}
		}

		public Property[] Properties
		{
			get 
			{
				if (NoiseAsset == null || NoiseAsset.UpdatedAt < UpdatedAt) return _Properties;
				_Properties = MergedProperties(_Properties, NoiseAsset.Noise);
				UpdatedAt = DateTime.Now;
				return _Properties;
			}
			set
			{
				_Properties = value;
				UpdatedAt = DateTime.Now;
			}
		}

		/// <summary>
		/// Instantiates and returns an Echo object with the properties of this asset.
		/// </summary>
		/// <returns>The new Echo instance.</returns>
		public Echo GetEcho()
		{
			return GetEcho(Seed, Translation, Rotation, Scale);
		}

		public Echo GetEcho(int seed)
		{
			return GetEcho(seed, Translation, Rotation, Scale);
		}

		public Echo GetEcho(Vector3 translation, Vector3 rotation, Vector3 scale)
		{
			return GetEcho(Seed, translation, rotation, scale);
		}

		public Echo GetEcho(int seed, Vector3 translation, Vector3 rotation, Vector3 scale)
		{
			if (NoiseAsset == null)
			{
				Debug.LogError("Null NoiseAsset", this);
				return null;
			}
			return new Echo(NoiseAsset.Noise, seed, translation, rotation, scale, Properties);
		}

		static Property[] MergedProperties(Property[] oldAssets, Noise noise)
		{
			oldAssets = oldAssets ?? new Property[0];
			var newAssets = noise.PropertyNodes.Select(p => p.Property).ToList();

			foreach (var asset in newAssets)
			{
				var existing = oldAssets.FirstOrDefault(p => p.Name == asset.Name && p.Type == asset.Type);
				if (existing != null) asset.SetValue(existing.Value, asset.Type);
			}
			return newAssets.ToArray();
		}
	}
}