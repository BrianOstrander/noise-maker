﻿using UnityEditor;
using LunraGames;
using LunraGames.NoiseMaker;

namespace LunraGamesEditor.NoiseMaker
{
	[NodeDrawer(typeof(IntegerNode), Strings.Properties, "Integer")]
	public class IntegerNodeEditor : NodeEditor
	{
		public override INode Draw(Noise graph, INode node)
		{
			var integerNode = node as IntegerNode;

			var preview = GetPreview(graph, node);

			integerNode.PropertyValue = Deltas.DetectDelta<int>(integerNode.PropertyValue, EditorGUILayout.IntField("Value", integerNode.PropertyValue), ref preview.Stale);

			return integerNode;
		}
	}
}