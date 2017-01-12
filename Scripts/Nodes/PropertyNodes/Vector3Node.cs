using UnityEngine;

namespace LunraGames.NoiseMaker
{
	public class Vector3Node : PropertyNode<Vector3> 
	{
		[NodeLinker(0, hide: true)]
		public float X;
		[NodeLinker(1, hide: true)]
		public float Y;
		[NodeLinker(2, hide: true)]
		public float Z;

		public override Vector3 GetValue(Noise noise)
		{
			Value = base.GetValue(noise);

			var values = NullableValues(noise);

			var currX = GetLocalIfValueNull(Value.x, 0, values);
			var currY = GetLocalIfValueNull(Value.y, 1, values);
			var currZ = GetLocalIfValueNull(Value.z, 2, values);

			return Value = new Vector3(currX, currY, currZ);
		}
	}
}