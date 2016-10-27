using UnityEditor;
using LunraGames;
using LunraGames.NoiseMaker;

namespace LunraGamesEditor.NoiseMaker
{
	[NodeDrawer(typeof(BooleanNode), Strings.Properties, "Boolean")]
	public class BooleanNodeEditor : NodeEditor
	{
		public override INode Draw(Noise graph, INode node)
		{
			var booleanNode = node as BooleanNode;

			var preview = GetPreview(graph, node);

			booleanNode.PropertyValue = Deltas.DetectDelta<bool>(booleanNode.PropertyValue, EditorGUILayout.Toggle("Value", booleanNode.PropertyValue), ref preview.Stale);

			return booleanNode;
		}
	}
}