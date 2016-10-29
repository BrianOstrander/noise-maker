using System;
using LibNoise;
using UnityEngine;

namespace LunraGames.NoiseMaker
{
	public class TexturedNode : Node<IModule>
	{
		[NodeLinker(0, hide: true), NonSerialized]
		public Texture2D Texture;
		[NodeLinker(1)]
		public TextureChannels Channel;

		Texture2D lastTexture;
		Color[][] lastArray;

		public override IModule GetValue(Noise noise)
		{
			var values = NullableValues(noise);

			var texture = GetLocalIfValueNull(Texture, 0, values);
			var channel = GetLocalIfValueNull(Channel, 1, values);

			var textured = Value == null ? new Textured() : Value as Textured;

			if (texture == null) return null;

			if (texture != lastTexture)
			{
				lastTexture = texture;
				Color[][] newArray = null;

				try
				{
					newArray = texture == null ? null : Texture2DExtensions.GetAsArray(texture);
				} 
				catch (UnityException)
				{
					texture = null;
					lastTexture = null;
					lastArray = null;
					return null;
				}

				lastArray = newArray;
			}

			textured.Texture = lastArray;
			textured.Channel = channel;

			Value = textured;

			return Value;
		}
	}
}