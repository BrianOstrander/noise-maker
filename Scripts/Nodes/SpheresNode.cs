using LibNoise;

namespace LunraGames.NoiseMaker
{
	public class SpheresNode : Node<IModule>
	{
		[NodeLinker(0)]
		public float Frequency;

		public override IModule GetValue (Noise graph)
		{
			var spheres = Value == null ? new Spheres() : Value as Spheres;

			spheres.Frequency = GetLocalIfValueNull(Frequency, 0, graph);

			Value = spheres;
			return Value;
		}
	}
}