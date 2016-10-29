using UnityEngine;
using System;
using LibNoise.Models;
using LibNoise.Modifiers;
using LunraGames.NumberDemon;

namespace LunraGames.NoiseMaker
{
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	public class NoiseFilter : MonoBehaviour 
	{
		public const float DefaultDatum = 0.5f;
		public const float DefaultDeviation = 0.1f;

		#region Inspector
		public bool GenerateOnAwake;
		public bool OverrideSeed;
		public int Seed;
		public EchoAsset Echo;
		public MercatorMap MercatorMap;
		public int MapWidth = 128;
		public int MapHeight = 128;
		public Vector3 Translation;
		public Vector3 Rotation;
		public Vector3 Scale = Vector3.one;
		public Filtering Filtering;
		public float Datum = DefaultDatum;
		public float Deviation = DefaultDeviation;
		#endregion

		Mesh CachedMesh;

		void Awake()
		{
			if (GenerateOnAwake) Regenerate();
		}

		public void Regenerate()
		{
			if (Echo == null) throw new NullReferenceException("A NoiseGraph must be specified");
			if (MercatorMap == null) throw new NullReferenceException("A MercatorMap must be specified");

			var meshFilter = GetComponent<MeshFilter>();
			var meshRenderer = GetComponent<MeshRenderer>();

			if (meshFilter == null) throw new NullReferenceException("A MeshFilter is required");
			if (meshRenderer == null) throw new NullReferenceException("A MeshRenderer is required");
			if (meshFilter.sharedMesh == null) throw new NullReferenceException("A sharedMesh must be specified for this gameobject's MeshFilter");
			if (meshRenderer.sharedMaterial == null) throw new NullReferenceException("A sharedMaterial must be specified for this gameobject's MeshRenderer");

			if (MapWidth <= 0) throw new ArgumentOutOfRangeException("MapWidth", "MapWidth must be greater than zero");
			if (MapHeight <= 0) throw new ArgumentOutOfRangeException("MapHeight", "MapHeight must be greater than zero");

			if (CachedMesh == null) CachedMesh = meshFilter.sharedMesh;

			var map = MercatorMap.MercatorInstantiation;

			if (map == null) throw new NullReferenceException("Couldn't instantiate the MercatorMap");

			var echo = OverrideSeed ? Echo.GetEcho(Seed, Translation, Rotation, Scale) : Echo.GetEcho(Translation, Rotation, Scale);

			if (echo == null) throw new NullReferenceException("Couldn't instantiate the Echo");

			var mesh = Instantiate(CachedMesh);

			var verts = mesh.vertices;
			echo.SphereTransformations(ref verts, Datum, Deviation);
			mesh.vertices = verts;

			meshFilter.sharedMesh = mesh;

			var texture = new Texture2D(MapWidth, MapHeight);
			var colors = new Color[MapWidth * MapHeight];

			map.GetSphereColors(MapWidth, MapHeight, echo, ref colors);
			texture.SetPixels(colors);
			texture.Apply();

			meshRenderer.material.mainTexture = texture;
		}
	}
}