using LunraGames.NoiseMaker;

namespace LunraGamesEditor.NoiseMaker
{
	[NodeDrawer(typeof(VoronoiNode), Strings.Generators, "Voronoi")]
	public class VoronoiNodeEditor : NodeEditor
	{
		public override INode Draw(Noise graph, INode node)
		{
			return DrawFields(graph, node);
		}
	}
}