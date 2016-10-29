using LunraGames.NumberDemon;

namespace LunraGames.NoiseMaker
{
	public class SeedNode : Node<int> 
	{
		int? Seed;
		int LastRootSeed;

		public override int GetValue (Noise noise)
		{
			if (!Seed.HasValue || LastRootSeed != noise.Seed) 
			{
				LastRootSeed = noise.Seed;
 				Seed = DemonUtility.CantorPair(Id.GetHashCode(), LastRootSeed);
			}

			return Seed.Value;
		}
	}
}