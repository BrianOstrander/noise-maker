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
		[NodeLinker(2)]
		public Color DefaultColor = Color.gray;

		Texture2D lastTexture;
		Color[][] lastArray;

		public override IModule GetValue(Noise noise)
		{
			var values = NullableValues(noise);

			var texture = GetLocalIfValueNull(Texture, 0, values);
			var channel = GetLocalIfValueNull(Channel, 1, values);
			var color = GetLocalIfValueNull(DefaultColor, 2, values);

			var textured = Value == null ? new Textured() : Value as Textured;

			if (texture != lastTexture)
			{
				lastTexture = texture;
				Color[][] newArray = null;

				if (texture != null)
				{
					try
					{
						newArray = texture == null ? null : Texture2DExtensions.GetAsArray(texture);
					}
					catch (UnityException)
					{
						texture = null;
						lastTexture = null;
					}
				}
				
				lastArray = newArray;
			}

			textured.Texture = lastArray;
			textured.Channel = channel;
			textured.DefaultColor = color;

			Value = textured;

			return Value;
		}
	}
}