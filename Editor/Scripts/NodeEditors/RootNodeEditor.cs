using UnityEditor;
using UnityEngine;
using LunraGames;
using LunraGames.NumberDemon;
using LunraGames.NoiseMaker;

namespace LunraGamesEditor.NoiseMaker
{
	[NodeDrawer(typeof(RootNode), Strings.Hidden, "Root", Strings.SpecifyAnInput)]
	public class RootNodeEditor : NodeEditor 
	{
		public override INode Draw(Noise noise, INode node)
		{
			var rootNode = DrawFields(noise, node) as RootNode;

			var preview = GetPreview(noise, node);

			noise.Seed = Deltas.DetectDelta<int>(noise.Seed, EditorGUILayout.IntField("Seed", noise.Seed), ref preview.Stale);

			if (GUILayout.Button("Randomize"))
			{
				noise.Seed = DemonUtility.NextInteger;
				preview.Stale = true;
			}

			return rootNode;
		}
	}
}