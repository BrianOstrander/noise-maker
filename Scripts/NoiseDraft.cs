using UnityEngine;
using LibNoise;
using LibNoise.Models;
using Plane = LibNoise.Models.Plane;

namespace LunraGames.NoiseMaker
{
	public class NoiseDraft
	{
		Noise _Noise;
		Property[] _Properties;

		public Noise Noise 
		{
			get { return _Noise; }
			set
			{
				Plane = null;
				Cube = null;
				Sphere = null;
				_Noise = value;
				if (_Noise != null && _Properties != null) _Noise.Apply(_Properties);
			}
		}

		public Property[] Properties
		{
			get { return _Properties; }
			set 
			{
				_Properties = value;
				if (_Noise == null || _Properties == null) return;

				Plane = null;
				Cube = null;
				Sphere = null;
				_Noise.Apply(_Properties);	
			}
		}

		Plane Plane;
		IModule Cube;
		Sphere Sphere;

		public NoiseDraft(Noise graph, params Property[] properties)
		{
			Noise = graph;
			Properties = properties;
		}

		public float PlaneValue(Vector2 position)
		{
			Plane = Plane ?? (Plane = new Plane(Noise.Root));
			return Plane.GetValue(position.x, position.y);
		}

		public float CubeValue(Vector3 position)
		{
			Cube = Cube ?? (Cube = Noise.Root);
			return Cube.GetValue(position.x, position.y, position.z);
		}

		public float SphereValue(float latitude, float longitude)
		{
			Sphere = Sphere ?? (Sphere = new Sphere(Noise.Root));
			return Sphere.GetValue(latitude, longitude);
		}

		public float SphereValue(Vector3 cartesian)
		{
			var geographic = SphereUtils.CartesianToGeographic(cartesian);
			return SphereValue(geographic.x, geographic.y);
		}
	}
}