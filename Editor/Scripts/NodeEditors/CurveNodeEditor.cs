using UnityEditor;
using UnityEngine;
using LunraGames;
using LunraGames.NoiseMaker;

namespace LunraGamesEditor.NoiseMaker
{
	[NodeDrawer(typeof(CurveNode), Strings.Properties, "Curve")]
	public class CurveNodeEditor : NodeEditor
	{
		public override INode Draw(Noise noise, INode node)
		{
			var curveNode = node as CurveNode;

			var preview = GetPreview(noise, node);

			var unmodifiedCurve = new AnimationCurve();
			foreach (var key in curveNode.PropertyValue.keys)
			{
				unmodifiedCurve.AddKey(new Keyframe(key.time, key.value, key.inTangent, key.outTangent));
			}
			// for spooky reasons I can't remember, we need to pass the unmodifiedCurve to the CurveField
			curveNode.PropertyValue = EditorGUILayout.CurveField("Curve", unmodifiedCurve);
			preview.Stale = preview.Stale || !AnimationCurveExtensions.CurvesEqual(unmodifiedCurve, curveNode.PropertyValue);

			return curveNode;
		}
	}
}