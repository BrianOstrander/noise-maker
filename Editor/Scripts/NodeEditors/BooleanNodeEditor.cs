using UnityEditor;
using LunraGames;
using LunraGames.NoiseMaker;

namespace LunraGamesEditor.NoiseMaker
{
	[NodeDrawer(typeof(BooleanNode), Strings.Properties, "Boolean")]
	public class BooleanNodeEditor : NodeEditor
	{
		public override INode Draw(Noise noise, INode node)
		{
			var booleanNode = node as BooleanNode;

			var preview = GetPreview(noise, node);

			booleanNode.PropertyValue = Deltas.DetectDelta<bool>(booleanNode.PropertyValue, EditorGUILayout.Toggle("Value", booleanNode.PropertyValue), ref preview.Stale);

			return booleanNode;
		}
	}
}