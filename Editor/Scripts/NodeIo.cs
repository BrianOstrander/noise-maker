using UnityEngine;
using System.Collections.Generic;
using LibNoise;
using System;

namespace LunraGamesEditor.NoiseMaker
{
	public class NodeIo
	{
		public string Name;
		public string Tooltip;
		public bool Connecting;
		public bool Active;
		public Action OnClick;
		public bool MatchedType;
		public Type Type;
	}
}