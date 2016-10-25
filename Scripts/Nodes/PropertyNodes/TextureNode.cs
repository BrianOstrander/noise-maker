using Newtonsoft.Json;
using UnityEngine;

namespace LunraGames.NoiseMaker
{
	public class TextureNode : PropertyNode<Texture2D> 
	{
		[JsonIgnore]
		public override Texture2D PropertyValue
		{
			get
			{
				return base.PropertyValue;
			}
			set
			{
				base.PropertyValue = value;
			}
		}
	}
}