using UnityEditor;
using UnityEngine;
using LunraGames;
using LunraGames.NoiseMaker;

namespace LunraGamesEditor.NoiseMaker
{
	[NodeDrawer(typeof(Vector3Node), Strings.Properties, "Vector3")]
	public class Vector3NodeEditor : NodeEditor
	{
		public override INode Draw(Noise noise, INode node)
		{
			var vector3Node = node as Vector3Node;

			var preview = GetPreview(noise, node);

			vector3Node.PropertyValue = Deltas.DetectDelta<Vector3>(vector3Node.PropertyValue, EditorGUILayout.Vector3Field("Value", vector3Node.PropertyValue), ref preview.Stale);

			return vector3Node;
		}
	}
}