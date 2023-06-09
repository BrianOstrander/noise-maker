using UnityEditor;
using LibNoise;
using LunraGames;
using LunraGames.NoiseMaker;

namespace LunraGamesEditor.NoiseMaker
{
	[NodeDrawer(typeof(NoiseQualityNode), Strings.Properties, "Noise Quality")]
	public class NoiseQualityNodeEditor : NodeEditor
	{
		public override INode Draw(Noise noise, INode node)
		{
			var noiseQualityNode = node as NoiseQualityNode;

			var preview = GetPreview(noise, node);

			noiseQualityNode.PropertyValue = Deltas.DetectDelta<NoiseQuality>(noiseQualityNode.PropertyValue, (NoiseQuality)EditorGUILayout.EnumPopup("Value", noiseQualityNode.PropertyValue), ref preview.Stale);

			return noiseQualityNode;
		}
	}
}