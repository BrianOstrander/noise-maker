using UnityEngine;
using System.Collections.Generic;
using System;

namespace LunraGames.NoiseMaker
{
	public interface INode
	{
		#region Inspector
		Vector2 EditorPosition { get; set; }
		#endregion

		int SourceCount { get; }
		string Id { get; set; }
		List<string> SourceIds { get; set; }
		object GetRawValue(Noise noise);
		Type OutputType { get; }
		bool HasAncestor(Noise noise, string ancestorId);
	}
}