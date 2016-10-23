using UnityEditor;
using LunraGames;
using LunraGames.NoiseMaker;

namespace LunraGamesEditor.NoiseMaker
{
	[NodeDrawer(typeof(RangeOverrideNode), Strings.Properties, "Range Override")]
	public class RangeOverrideNodeEditor : NodeEditor
	{
		public override INode Draw(Graph graph, INode node)
		{
			var overrideNode = node as RangeOverrideNode;

			var preview = GetPreview(graph, node);

			overrideNode.PropertyValue = Deltas.DetectDelta(overrideNode.PropertyValue, (RangeOverrides)EditorGUILayout.EnumPopup("Value", overrideNode.PropertyValue), ref preview.Stale);

			return overrideNode;
		}
	}
}
