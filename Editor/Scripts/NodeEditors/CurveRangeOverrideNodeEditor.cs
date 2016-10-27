using UnityEditor;
using LunraGames;
using LunraGames.NoiseMaker;

namespace LunraGamesEditor.NoiseMaker
{
	[NodeDrawer(typeof(CurveRangeOverrideNode), Strings.Properties, "Curve Override")]
	public class CurveRangeOverrideNodeEditor : NodeEditor
	{
		public override INode Draw(Noise graph, INode node)
		{
			var overrideNode = node as CurveRangeOverrideNode;

			var preview = GetPreview(graph, node);

			overrideNode.PropertyValue = Deltas.DetectDelta(overrideNode.PropertyValue, (CurveRangeOverrides)EditorGUILayout.EnumPopup("Value", overrideNode.PropertyValue), ref preview.Stale);

			return overrideNode;
		}
	}
}