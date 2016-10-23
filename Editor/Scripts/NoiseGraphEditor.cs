﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using LunraGames;
using LunraGames.NoiseMaker;

namespace LunraGamesEditor.NoiseMaker
{
	[CustomEditor(typeof(NoiseGraph), true)]
	public class NoiseGraphEditor : Editor
	{
		const string AdvancedShownKey = "LG_NoiseMaker_NoiseGraphAdvancedShown";

		static bool AdvancedShown { get { return EditorPrefs.GetBool(AdvancedShownKey, false); } set { EditorPrefs.SetBool(AdvancedShownKey, value); } }

		SerializedProperty GraphJsonProperty;
		SerializedProperty PropertiesJsonProperty;

		string LastGraphJson;
		string LastPropertiesJson;

		Graph Graph;
		List<Property> Properties;

		void OnEnable()
		{
			GraphJsonProperty = serializedObject.FindProperty("GraphJson");
			PropertiesJsonProperty = serializedObject.FindProperty("PropertiesJson");

			LastGraphJson = GraphJsonProperty.stringValue;
			LastPropertiesJson = PropertiesJsonProperty.stringValue;

			Properties = Serialization.DeserializeJson<List<Property>>(PropertiesJsonProperty.stringValue, verbose: true);
			Graph = Serialization.DeserializeJson<Graph>(GraphJsonProperty.stringValue, verbose: true);
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			var splashImage = NoiseMakerConfig.Instance.SplashMini;
			GUI.Box (new Rect (0f, Screen.height - splashImage.height - (splashImage.height * 0.2f), splashImage.width, splashImage.height), splashImage, GUIStyle.none);

			var assetPath = AssetDatabase.GetAssetPath(target.GetInstanceID());
			var activelyEditing = assetPath == NoiseMakerWindow.ActiveSavePath;
			var editingAllowed = !(activelyEditing || EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode || Properties == null || Graph == null);

			if (activelyEditing) EditorGUILayout.HelpBox("Cannot modify serialized data while editing in Noise Maker.", MessageType.Warning);
			else if (!editingAllowed) EditorGUILayout.HelpBox("Cannot modify serialized data while compiling or in playmode.", MessageType.Warning);

			GUI.enabled = !activelyEditing;

			if (GUILayout.Button("Open in Noise Maker")) NoiseMakerWindow.OpenNoiseGraph(assetPath);

			GUI.enabled = editingAllowed;

			var graphChanged = LastGraphJson != GraphJsonProperty.stringValue;
			var propertiesChanged = LastPropertiesJson != PropertiesJsonProperty.stringValue;

			if (AdvancedShown = EditorGUILayout.Foldout(AdvancedShown, "Advanced"))
			{
				EditorGUILayout.HelpBox("Messing with the properties below could irreversibly destroy your data, so be careful!", MessageType.Warning);
				GUILayout.BeginHorizontal();
				{
					if (GUILayout.Button("Copy Graph Json", EditorStyles.miniButtonLeft)) EditorGUIUtility.systemCopyBuffer = GraphJsonProperty.stringValue;
					if (GUILayout.Button("Paste Graph Json", EditorStyles.miniButtonRight))	GraphJsonProperty.stringValue = Deltas.DetectDelta<string>(GraphJsonProperty.stringValue, EditorGUIUtility.systemCopyBuffer, ref graphChanged);
					if (GUILayout.Button("Copy Properties Json", EditorStyles.miniButtonLeft)) EditorGUIUtility.systemCopyBuffer = PropertiesJsonProperty.stringValue;
					if (GUILayout.Button("Paste Properties Json", EditorStyles.miniButtonRight)) PropertiesJsonProperty.stringValue = Deltas.DetectDelta<string>(PropertiesJsonProperty.stringValue, EditorGUIUtility.systemCopyBuffer, ref propertiesChanged);
				}
				GUILayout.EndHorizontal();
			}

			if (propertiesChanged || Properties == null) Properties = Serialization.DeserializeJson<List<Property>>(PropertiesJsonProperty.stringValue, verbose: true);

			if (graphChanged || Graph == null) Graph = Serialization.DeserializeJson<Graph>(GraphJsonProperty.stringValue, verbose: true);

			try { DrawProperties(); }
			catch (Exception e)
			{
				EditorGUILayout.HelpBox("Unable to draw properties, exception occurred: "+e.Message, MessageType.Error);
				if (GUILayout.Button("Print Exception")) Debug.LogException(e);
			}

			LastGraphJson = GraphJsonProperty.stringValue;
			LastPropertiesJson = PropertiesJsonProperty.stringValue;

			serializedObject.ApplyModifiedProperties();
		}

		void DrawProperties()
		{
			if (Graph == null) EditorGUILayout.HelpBox("There were errors deserializing the Graph", MessageType.Error);
			else if (Properties == null) EditorGUILayout.HelpBox("There were errors deserializing the Properties", MessageType.Error);
			else
			{
				var rootChanged = false;

				GUI.color = LunraGamesEditor.NoiseMaker.Styles.RootColor;
				Graph.Seed = Deltas.DetectDelta(Graph.Seed, EditorGUILayout.IntField("Root Seed", Graph.Seed), ref rootChanged);
				GUI.color = Color.white;

				Property changedProperty = null;
				foreach (var property in Properties)
				{
					if (property == null)
					{
						EditorGUILayout.HelpBox("Null properties are not supported.", MessageType.Error);
						continue;
					}

					var value = property.Value;
					var propertyName = StringExtensions.IsNullOrWhiteSpace(property.Name) ? "Null name" : property.Name;
					var helpboxName = StringExtensions.IsNullOrWhiteSpace(property.Name) ? "with a null name" : "\""+property.Name+"\"";
					var changed = false;

					if (value == null) EditorGUILayout.HelpBox("The null value of property "+helpboxName+" is not supported." , MessageType.Error);
					else if (value is float)
					{
						var typedValue = (float)value;
						property.Value = Deltas.DetectDelta(typedValue, EditorGUILayout.FloatField(propertyName, typedValue), ref changed);
					}
					else if (value is int)
					{
						var typedValue = (int)value;
						property.Value = Deltas.DetectDelta(typedValue, EditorGUILayout.IntField(propertyName, typedValue), ref changed);
					}
					else if (value is bool)
					{
						var typedValue = (bool)value;
						property.Value = Deltas.DetectDelta(typedValue, EditorGUILayout.Toggle(propertyName, typedValue), ref changed);
					}
					else if (value is Enum) 
					{
						var typedValue = (Enum)value;
						property.Value = Deltas.DetectDelta(typedValue, EditorGUILayout.EnumPopup(propertyName, typedValue), ref changed);
					} 
					else if (value is Vector3) 
					{
						var typedValue = (Vector3)value;
						property.Value = Deltas.DetectDelta(typedValue, EditorGUILayout.Vector3Field(propertyName, typedValue), ref changed);
					}
					else if (value is AnimationCurve)
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

						property.Value = typedValue;
					}
					else EditorGUILayout.HelpBox("Property "+helpboxName+" is of unsupported type \""+value.GetType()+"\".", MessageType.Error);

					if (changed) changedProperty = property;
				}

				if (rootChanged || changedProperty != null)
				{
					try
					{
						// Only apply properties to graph if one of them changed.
						if (changedProperty != null) Graph.Apply(changedProperty);

						var freshGraph = Serialization.SerializeJson(Graph, true);
						var freshProperties = Serialization.SerializeJson(Properties, true);

						if (string.IsNullOrEmpty(freshGraph)) throw new Exception("Graph could not be serialized");
						else if (string.IsNullOrEmpty(freshProperties)) throw new Exception("Properties could not be serialized");
						else
						{
							GraphJsonProperty.stringValue = freshGraph;
							PropertiesJsonProperty.stringValue = freshProperties;
						}
					}
					catch (Exception e) { Debug.LogException(e); }
				}
			}
		}
	}
}