using LunraGames.NoiseMaker;

namespace LunraGamesEditor.NoiseMaker
{
	[NodeDrawer(typeof(VoronoiNode), Strings.Generators, "Voronoi")]
	public class VoronoiNodeEditor : NodeEditor
	{
		public override INode Draw(Noise noise, INode node)
		{
			return DrawFields(noise, node);
		}
	}
}