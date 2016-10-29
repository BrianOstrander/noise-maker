using UnityEngine;
using LibNoise;
using LibNoise.Models;
using Plane = LibNoise.Models.Plane;

namespace LunraGames.NoiseMaker
{
	public class Echo
	{
		int _Seed;

		public int Seed
		{
			get { return _Seed; }
			set 
			{
				if (_Seed == value) return;
				_Seed = value;
				if (_Noise == null) return;
				CleanCache();
				_Noise.Seed = _Seed;
			}
		}

		Noise _Noise;

		public Noise Noise 
		{
			get { return _Noise; }
			set
			{
				CleanCache();
				_Noise = value;
				if (_Noise == null) return;
				_Noise.Seed = Seed;
				_Noise.Translation = _Translation;
				_Noise.Rotation = _Rotation;
				_Noise.Scale = _Scale;
				if (_Properties != null) _Noise.Apply(_Properties);
			}
		}

		Property[] _Properties;

		public Property[] Properties
		{
			get { return _Properties; }
			set 
			{
				_Properties = value;
				if (_Noise == null || _Properties == null) return;

				CleanCache();
				_Noise.Apply(_Properties);	
			}
		}

		Vector3 _Translation;

		public Vector3 Translation
		{
			get { return _Translation; }
			set
			{
				if (_Translation == value) return;
				_Translation = value;
				CleanCache();
				if (_Noise == null) return;
				_Noise.Translation = _Translation;
			}
		}

		Vector3 _Rotation;

		public Vector3 Rotation
		{
			get { return _Rotation; }
			set
			{
				if (_Rotation == value) return;
				_Rotation = value;
				CleanCache();
				if (_Noise == null) return;
				_Noise.Rotation = _Rotation;
			}
		}

		Vector3 _Scale;

		public Vector3 Scale
		{
			get { return _Scale; }
			set
			{
				if (_Scale == value) return;
				_Scale = value;
				CleanCache();
				if (_Noise == null) return;
				_Noise.Scale = _Scale;
			}
		}

		void CleanCache()
		{
			_Plane = null;
			_Cube = null;
			_Sphere = null;
		}

		Plane _Plane;
		public Plane Plane { get { return _Plane ?? (_Plane = new Plane(Noise.Root)); } }

		IModule _Cube;
		public IModule Cube { get { return _Cube ?? (_Cube = Noise.Root); } }

		Sphere _Sphere;
		public Sphere Sphere { get { return _Sphere ?? (_Sphere = new Sphere(Noise.Root)); } }

		public Echo(Noise noise, params Property[] properties) : this(noise, 0, properties) {}
		public Echo(Noise noise, int seed, params Property[] properties) : this(noise, seed, Vector3.zero, Vector3.zero, Vector3.one, properties) {}
		public Echo(Noise noise, Vector3 translation, Vector3 rotation, Vector3 scale, params Property[] properties) : this(noise, 0, translation, rotation, scale, properties) {}
		public Echo(Noise noise, int seed, Vector3 translation, Vector3 rotation, Vector3 scale, params Property[] properties)
		{
			Seed = seed;
			Translation = translation;
			Rotation = rotation;
			Scale = scale;
			Properties = properties;
			Noise = noise;
		}

		public float PlaneValue(Vector2 position)
		{
			return Plane.GetValue(position.x, position.y);
		}

		public float CubeValue(Vector3 position)
		{
			return Cube.GetValue(position.x, position.y, position.z);
		}

		public float SphereValue(float latitude, float longitude)
		{
			return Sphere.GetValue(latitude, longitude);
		}

		public float SphereValue(Vector3 cartesian)
		{
			var geographic = SphereUtils.CartesianToGeographic(cartesian);
			return SphereValue(geographic.x, geographic.y);
		}

		public void SphereTransformations(ref Vector3[] vertices, float datum = Noise.DefaultDatum, float deviation = Noise.DefaultDatum)
		{
			// Cache this here so we're not making constant null checks.
			var sphere = Sphere;

			for (var i = 0; i < vertices.Length; i++)
			{
				// Get the value of the specified vert, by converting it's euler position to a latitude and longitude.
				var vert = vertices[i];
				var latLong = SphereUtils.CartesianToGeographic(vert.normalized);
				vertices[i] = (vert.normalized * datum) + (vert.normalized * sphere.GetValue(latLong.x, latLong.y) * (datum * deviation));
			}
		}
	}
}