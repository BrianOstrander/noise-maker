using System;
using System.Collections.Generic;
using LibNoise;
using Newtonsoft.Json;
using System.Linq;
using UnityEngine;
using LibNoise.Models;

namespace LunraGames.NoiseMaker
{
	public class Noise
	{
		const float DefaultDatum = 0.5f;
		const float DefaultDeviation = 0.1f;

		[JsonProperty]
		List<INode> Nodes = new List<INode>();

		INode[] CachedNodes;

		[JsonIgnore]
		public INode[] AllNodes { get { return CachedNodes ?? (CachedNodes = Nodes.ToArray()); } }

		IPropertyNode[] CachedProperties;

		[JsonIgnore]
		public IPropertyNode[] PropertyNodes { get { return CachedProperties ?? (CachedProperties = AllNodes.OfType<IPropertyNode>().Where(p => p.IsEditable).ToArray()); } }

		public string RootId;
		public int Seed;

		IModule _Root;

		[JsonIgnore]
		public IModule Root 
		{ 
			get 
			{
				if (StringExtensions.IsNullOrWhiteSpace(RootId)) throw new NullReferenceException("No RootId has been set");
				if (_Root == null)
				{
					var node = AllNodes.FirstOrDefault(n => n.Id == RootId);
					if (node == null) throw new NullReferenceException("No node found for the RootId \""+RootId+"\"");
					_Root = node.GetRawValue(this) as IModule;
				}
				return _Root;
			}
		}

		RootNode _RootNode;

		[JsonIgnore]
		public RootNode RootNode
		{
			get
			{
				if (StringExtensions.IsNullOrWhiteSpace(RootId)) throw new NullReferenceException("No RootId has been set");
				if (_RootNode == null || _RootNode.Id != RootId)
				{
					var node = AllNodes.FirstOrDefault(n => n.Id == RootId);
					if (node == null) throw new NullReferenceException("No node found for the RootId \""+RootId+"\"");
					_RootNode = node as RootNode;
				}
				return _RootNode;
			}
		}

		public void Add(INode node)
		{
			if (node == null) throw new ArgumentNullException("node");
			if (Nodes.Any(n => n.Id == node.Id)) throw new ArgumentException("The specified node already exists in the Graph", "node");

			Nodes.Add(node);
			CleanCache();
		}

		public void Remove(INode node)
		{
			if (node == null) throw new ArgumentNullException("node");
			if (!Nodes.Any(n => n.Id == node.Id)) throw new ArgumentException("The specified node dose not exist in the Graph", "node");

			foreach (var curr in AllNodes)
			{
				if (curr.SourceIds == null) continue;
				for (var i = 0; i < curr.SourceIds.Count; i++)
				{
					if (curr.SourceIds[i] == node.Id) curr.SourceIds[i] = null;
				}
			}

			Nodes.Remove(node);
			CleanCache();
		}

		void CleanCache() 
		{
			CachedNodes = null;
			CachedProperties = null;
			_Root = null;
			_RootNode = null;
		}

		/// <summary>
		/// Populates an array with values derived from a spherical model of this graph.
		/// </summary>
		/// <param name="vertices">Vertices array to update.</param>
		/// <param name="datum">Datum is similar to a "sea level" that all values are relative to.</param>
		/// <param name="deviation">Deviation is the scalar of the datam to multiply the values by.</param>
		public void GetSphereAltitudes(ref Vector3[] vertices, float datum = DefaultDatum, float deviation = DefaultDeviation)
		{
			var root = Root;

			if (root == null) throw new NullReferenceException("Couldn't find root IModule");

			var sphere = root is Sphere ? root as Sphere : new Sphere(root);

			GetSphereAltitudes(sphere, ref vertices, datum, deviation);
		}

		/// <summary>
		/// Gets the sphere altitudes.
		/// </summary>
		/// <param name="sphere">Sphere module to get values from.</param>
		/// <param name="vertices">Vertices array to update.</param>
		/// <param name="datum">Datum is similar to a "sea level" that all values are relative to.</param>
		/// <param name="deviation">Deviation is the scalar of the datam to multiply the values by.</param>
		public static void GetSphereAltitudes(Sphere sphere, ref Vector3[] vertices, float datum = DefaultDatum, float deviation = DefaultDeviation)
		{
			if (sphere == null) throw new ArgumentNullException("sphere");

			for (var i = 0; i < vertices.Length; i++)
			{
				// Get the value of the specified vert, by converting it's euler position to a latitude and longitude.
				var vert = vertices[i];
				var latLong = SphereUtils.CartesianToGeographic(vert.normalized);
				vertices[i] = (vert.normalized * datum) + (vert.normalized * (float)sphere.GetValue(latLong.x, latLong.y) * (datum * deviation));
			}
		}

		public void Apply(params Property[] properties)
		{
			foreach (var property in properties)
			{
				if (property == null)
				{
					Debug.LogWarning("Can't provide null properties, skipping");
					continue;
				}
				if (property.Value == null)
				{
					Debug.LogWarning("Can't provide properties with null values, skipping");
					continue;
				}

				SetRaw(property.Name, property.Value);
			}
			CleanCache();
		}

		public T Get<T>(string name)
		{
			var property = PropertyNodes.FirstOrDefault(p => p.Name == name && (p as INode).OutputType == typeof(T));

			if (property == null)
			{
				Debug.LogError("No property named \"" + name + "\" found, returning default value");
				return default(T);
			}

			return (T)property.RawPropertyValue;
		}

		public void Set(string name, bool value) { SetRaw(name, value); }

		public void Set(string name, AnimationCurve value) { SetRaw(name, value); }

		public void Set(string name, CurveRangeOverrides value) { SetRaw(name, value); }

		public void Set(string name, float value) { SetRaw(name, value); }

		public void Set(string name, int value) { SetRaw(name, value); }

		public void Set(string name, NoiseQuality value) { SetRaw(name, value); }

		public void Set(string name, RangeOverrides value) { SetRaw(name, value); }

		public void Set(string name, Vector3 value) { SetRaw(name, value); }

		public void SetRaw(string name, object value)
		{
			var property = PropertyNodes.FirstOrDefault(p => p.Name == name);

			if (property == null)
			{
				Debug.LogWarning("No property named \"" + name + "\" found");
				return;
			}

			property.RawPropertyValue = value;
			CleanCache();
		}
	}
}