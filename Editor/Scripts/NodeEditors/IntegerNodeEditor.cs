using UnityEditor;
using LunraGames;
using LunraGames.NoiseMaker;

namespace LunraGamesEditor.NoiseMaker
{
	[NodeDrawer(typeof(IntegerNode), Strings.Properties, "Integer")]
	public class IntegerNodeEditor : NodeEditor
	{
		public override INode Draw(Noise noise, INode node)
		{
			var integerNode = node as IntegerNode;

			var preview = GetPreview(noise, node);

			integerNode.PropertyValue = Deltas.DetectDelta<int>(integerNode.PropertyValue, EditorGUILayout.IntField("Value", integerNode.PropertyValue), ref preview.Stale);

			return integerNode;
		}
	}
}