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
		public override INode Draw(Noise graph, INode node)
		{
			var rootNode = DrawFields(graph, node) as RootNode;

			var preview = GetPreview(graph, node);

			graph.Seed = Deltas.DetectDelta<int>(graph.Seed, EditorGUILayout.IntField("Seed", graph.Seed), ref preview.Stale);

			if (GUILayout.Button("Randomize"))
			{
				graph.Seed = DemonUtility.NextInteger;
				preview.Stale = true;
			}

			return rootNode;
		}
	}
}