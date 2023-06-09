using LibNoise;
using System.Collections.Generic;
using LibNoise.Modifiers;
using Newtonsoft.Json;

namespace LunraGames.NoiseMaker
{
	public class InvertNode : Node<IModule> 
	{
		[NodeLinker(0, hide: true), JsonIgnore]
		public IModule Source;

		public override IModule GetValue (Noise noise)
		{
			var source = GetLocalIfValueNull<IModule>(Source, 0, noise);

			if (source == null) return null;

			var invert = Value == null ? new InvertOutput(source) : Value as InvertOutput;

			invert.SourceModule = source;

			Value = invert;

			return Value;
		}
	}
}