using LunraGames.NumberDemon;

namespace LunraGames.NoiseMaker
{
	public class SeedNode : Node<int> 
	{
		int? Seed;
		int LastRootSeed;

		public override int GetValue (Noise graph)
		{
			if (!Seed.HasValue || LastRootSeed != graph.Seed) 
			{
				LastRootSeed = graph.Seed;
 				Seed = DemonUtility.CantorPair(Id.GetHashCode(), LastRootSeed);
			}

			return Seed.Value;
		}
	}
}