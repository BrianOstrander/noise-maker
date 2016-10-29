using UnityEditor;
using LunraGames;
using LunraGames.NoiseMaker;

namespace LunraGamesEditor.NoiseMaker
{
	[NodeDrawer(typeof(ColorNode), Strings.Properties, "Color")]
	public class ColorNodeEditor : NodeEditor
	{
		public override INode Draw(Noise noise, INode node)
		{
			var colorNode = node as ColorNode;

			var preview = GetPreview(noise, node);

			colorNode.PropertyValue = Deltas.DetectDelta(colorNode.PropertyValue, EditorGUILayout.ColorField("Color", colorNode.PropertyValue), ref preview.Stale);

			return colorNode;
		}
	}
}