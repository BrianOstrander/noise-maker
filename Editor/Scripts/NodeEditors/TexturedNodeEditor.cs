using System.Linq;
using UnityEditor;
using LunraGames.NoiseMaker;
using UnityEngine;

namespace LunraGamesEditor.NoiseMaker
{
	[NodeDrawer(typeof(TexturedNode), Strings.Generators, "Textured", "Missing a readable Texture2D input.")]
	public class TexturedNodeEditor : NodeEditor 
	{

		public override INode Draw(Noise noise, INode node)
		{
			var textured = node as TexturedNode;

			var preview = GetPreview(noise, node);
			GUILayout.Box(preview.Preview);
			GUILayout.FlexibleSpace();

			if (textured.SourceIds == null || textured.SourceIds.FirstOrDefault() == null)
			{
				EditorGUILayout.HelpBox("Specify a Texture2D source or the default color will be used.", MessageType.Info);
			}

			textured = DrawFields(noise, textured, false) as TexturedNode;

			return textured;
		}
	}
}