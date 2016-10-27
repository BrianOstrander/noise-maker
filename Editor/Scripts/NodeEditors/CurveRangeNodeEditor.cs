using UnityEditor;
using UnityEngine;
using LunraGames.NoiseMaker;

namespace LunraGamesEditor.NoiseMaker
{
	[NodeDrawer(typeof(CurveRangeNode), Strings.Utility, "Curve Range")]
	public class CurveRangeNodeEditor : NodeEditor 
	{
		public override INode Draw(Noise graph, INode node)
		{
			var curveRange = DrawFields(graph, node, false) as CurveRangeNode;

			var currValue = curveRange.GetValue(graph);

			GUILayout.BeginHorizontal();
			{
				GUILayout.Label("Current Value");
				GUILayout.FlexibleSpace();
				EditorGUILayout.SelectableLabel(currValue.ToString(), GUI.skin.textField, GUILayout.Height(16f), GUILayout.Width(55f));
			}
			GUILayout.EndHorizontal();

			return curveRange;
		}
	}
}