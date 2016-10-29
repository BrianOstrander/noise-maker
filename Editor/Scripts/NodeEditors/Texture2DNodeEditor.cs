using UnityEditor;
using LunraGames;
using LunraGames.NoiseMaker;
using UnityEngine;

namespace LunraGamesEditor.NoiseMaker
{
	[NodeDrawer(typeof(Texture2DNode), Strings.Properties, "Texture2D")]
	public class Texture2DNodeEditor : NodeEditor
	{
		public override INode Draw(Noise noise, INode node)
		{
			var textureNode = node as Texture2DNode;

			var preview = GetPreview(noise, node);

			textureNode.PropertyValue = Deltas.DetectDelta(textureNode.PropertyValue, EditorGUILayout.ObjectField("Value", textureNode.PropertyValue, typeof(Texture2D), false) as Texture2D, ref preview.Stale);

			return textureNode;
		}
	}
}