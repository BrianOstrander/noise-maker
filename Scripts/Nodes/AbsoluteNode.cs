﻿using LibNoise;
using LibNoise.Modifiers;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LunraGames.NoiseMaker
{
	public class AbsoluteNode : Node<IModule>
	{
		/// <summary>
		/// The source used if SourceIds[0] is null.
		/// </summary>
		[NodeLinker(0, hide: true), JsonIgnore]
		public IModule Source;

		public override IModule GetValue (Noise noise)
		{
			var values = NullableValues(noise);

			var source = GetLocalIfValueNull(Source, 0, values);

			if (source == null) return null;

			var absolute = Value == null ?  new AbsoluteOutput(source) : Value as AbsoluteOutput;

			absolute.SourceModule = source;

			Value = absolute;

			return Value;
		}
	}
}