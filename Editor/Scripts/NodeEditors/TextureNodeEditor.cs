using UnityEditor;
using LunraGames;
using LunraGames.NoiseMaker;
using UnityEngine;

namespace LunraGamesEditor.NoiseMaker
{
	[NodeDrawer(typeof(TextureNode), Strings.Properties, "Texture")]
	public class TextureNodeEditor : NodeEditor
	{
		public override INode Draw(Noise noise, INode node)
		{
			var textureNode = node as TextureNode;

			var preview = GetPreview(noise, node);

			textureNode.PropertyValue = Deltas.DetectDelta(textureNode.PropertyValue, EditorGUILayout.ObjectField("Value", textureNode.PropertyValue, typeof(Texture2D), false) as Texture2D, ref preview.Stale);

			return textureNode;
		}
	}
}