using LunraGames.NoiseMaker;

namespace LunraGamesEditor.NoiseMaker
{
	[NodeDrawer(typeof(VoronoiNode), Strings.Generators, "Voronoi")]
	public class VoronoiNodeEditor : NodeEditor
	{
		public override INode Draw(Graph graph, INode node)
		{
			return DrawFields(graph, node);
		}
	}
}