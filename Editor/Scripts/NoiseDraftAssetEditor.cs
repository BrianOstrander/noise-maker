using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using LunraGames;
using LunraGames.NoiseMaker;

namespace LunraGamesEditor.NoiseMaker
{
	[CustomEditor(typeof(NoiseDraftAsset), true)]
	public class NoiseDraftAssetEditor : Editor
	{
		const string AdvancedShownKey = "LG_NoiseMaker_NoiseGraphAdvancedShown";

		static bool AdvancedShown { get { return EditorPrefs.GetBool(AdvancedShownKey, false); } set { EditorPrefs.SetBool(AdvancedShownKey, value); } }

		public override void OnInspectorGUI()
		{
			var typedTarget = target as NoiseDraftAsset;
			var dirty = false;

			var splashImage = NoiseMakerConfig.Instance.SplashMini;
			GUI.Box (new Rect (0f, Screen.height - splashImage.height - (splashImage.height * 0.2f), splashImage.width, splashImage.height), splashImage, GUIStyle.none);

			var assetPath = typedTarget.Noise == null ? null : AssetDatabase.GetAssetPath(typedTarget.Noise.GetInstanceID());
			var activelyEditing = !string.IsNullOrEmpty(assetPath) && assetPath == NoiseMakerWindow.ActiveSavePath;
			var editingAllowed = !EditorApplication.isCompiling;

			if (!editingAllowed) EditorGUILayout.HelpBox("Cannot modify serialized data while compiling.", MessageType.Warning);

			GUI.enabled = !activelyEditing;

			if (GUILayout.Button("Open in Noise Maker")) NoiseMakerWindow.OpenNoiseGraph(assetPath);

			GUI.enabled = editingAllowed;

			var noiseChanged = false;
			typedTarget.Noise =	Deltas.DetectDelta(typedTarget.Noise, EditorGUILayout.ObjectField("Noise", typedTarget.Noise, typeof(NoiseAsset), false) as NoiseAsset, ref noiseChanged);
			if (noiseChanged)
			{
				var newProperties = new List<Property>();
				if (typedTarget.Noise != null)
				{
					foreach (var propertyNode in typedTarget.Noise.Noise.PropertyNodes)
					{
						var property = new Property();
						property.Name = propertyNode.Name;
						property.SetValue(propertyNode.RawPropertyValue, propertyNode.OutputType);
						newProperties.Add(property);
					}
				}
				typedTarget.Assets = newProperties.ToArray();
			}
			dirty = dirty || noiseChanged;

			try { dirty = DrawProperties(typedTarget.Assets); }
			catch (Exception e)
			{
				EditorGUILayout.HelpBox("Unable to draw properties, exception occurred: "+e.Message, MessageType.Error);
				if (GUILayout.Button("Print Exception")) Debug.LogException(e);
			}

			if (dirty) EditorUtility.SetDirty(typedTarget);
		}

		bool DrawProperties(params Property[] properties)
		{
			var rootChanged = false;

			GUI.color = LunraGamesEditor.NoiseMaker.Styles.RootColor;
			GUILayout.Label("Todo: seed");
			//Graph.Seed = Deltas.DetectDelta(Graph.Seed, EditorGUILayout.IntField("Root Seed", Graph.Seed), ref rootChanged);
			GUI.color = Color.white;

			Property changedProperty = null;
			foreach (var property in properties)
			{
				var unmodifiedProperty = property;

				var value = unmodifiedProperty.Value;
				var type = unmodifiedProperty.Type;
				var propertyName = StringExtensions.IsNullOrWhiteSpace(unmodifiedProperty.Name) ? "Null name" : unmodifiedProperty.Name;
				var helpboxName = StringExtensions.IsNullOrWhiteSpace(unmodifiedProperty.Name) ? "with a null name" : "\""+unmodifiedProperty.Name+"\"";
				var changed = false;

				if (type == typeof(float))
				{
					var typedValue = (float)value;
					unmodifiedProperty.SetValue(Deltas.DetectDelta(typedValue, EditorGUILayout.FloatField(propertyName, typedValue), ref changed));
				}
				else if (type == typeof(int))
				{
					var typedValue = (int)value;
					unmodifiedProperty.SetValue(Deltas.DetectDelta(typedValue, EditorGUILayout.IntField(propertyName, typedValue), ref changed));
				}
				else if (type == typeof(bool))
				{
					var typedValue = (bool)value;
					unmodifiedProperty.SetValue(Deltas.DetectDelta(typedValue, EditorGUILayout.Toggle(propertyName, typedValue), ref changed));
				}
				else if (typeof(Enum).IsAssignableFrom(type))
				{
					var typedValue = (Enum)value;
					unmodifiedProperty.SetValue(Deltas.DetectDelta(typedValue, EditorGUILayout.EnumPopup(propertyName, typedValue), ref changed), type);
				}
				else if (type == typeof(Vector3))
				{
					var typedValue = (Vector3)value;
					unmodifiedProperty.SetValue(Deltas.DetectDelta(typedValue, EditorGUILayout.Vector3Field(propertyName, typedValue), ref changed));
				}
				else if (type == typeof(Color))
				{
					var typedValue = (Color)value;
					unmodifiedProperty.SetValue(Deltas.DetectDelta(typedValue, EditorGUILayout.ColorField(propertyName, typedValue), ref changed));
				}
				else if (type == typeof(AnimationCurve))
				{
					var typedValue = (AnimationCurve)value;

					var unmodifiedCurve = new AnimationCurve();
					foreach (var key in typedValue.keys)
					{
						unmodifiedCurve.AddKey(new Keyframe(key.time, key.value, key.inTangent, key.outTangent));
					}
					// for spooky reasons I can't remember, we need to pass the unmodifiedCurve to the CurveField
					typedValue = EditorGUILayout.CurveField(propertyName, unmodifiedCurve);
					changed = changed || !AnimationCurveExtensions.CurvesEqual(unmodifiedCurve, typedValue);

					unmodifiedProperty.SetValue(typedValue);
				}
				else if (type == typeof(Texture2D))
				{
					// weird check here because unity lies about serialized fields being null.
					var typedValue = value == null || value.Equals(null) ? null : (Texture2D)value;
					var result = EditorGUILayout.ObjectField(propertyName, typedValue, typeof(Texture2D), false);
					unmodifiedProperty.SetValue(Deltas.DetectDelta(typedValue, result == null ? null : result as Texture2D, ref changed));
				}
				else
				{
					GUILayout.BeginHorizontal();
					{
						EditorGUILayout.HelpBox("Property " + helpboxName + " is of unsupported type \"" + type + "\".", MessageType.Error);
						if (GUILayout.Button("Context", EditorStyles.miniButton, GUILayout.Height(40f))) DebugExtensions.OpenFileAtContext();
					}
					GUILayout.EndHorizontal();
				}

				if (changed) changedProperty = unmodifiedProperty;
			}

			return rootChanged || changedProperty != null;
		}
	}
}