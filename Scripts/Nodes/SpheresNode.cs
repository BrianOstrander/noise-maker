using LibNoise;

namespace LunraGames.NoiseMaker
{
	public class SpheresNode : Node<IModule>
	{
		[NodeLinker(0)]
		public float Frequency;

		public override IModule GetValue (Noise noise)
		{
			var spheres = Value == null ? new Spheres() : Value as Spheres;

			spheres.Frequency = GetLocalIfValueNull(Frequency, 0, noise);

			Value = spheres;
			return Value;
		}
	}
}