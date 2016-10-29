using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using LunraGames.NoiseMaker;
using LunraGames;

namespace LunraGamesEditor.NoiseMaker
{
	[CustomEditor(typeof(NoiseAsset), true)]
	public class NoiseAssetEditor : Editor
	{
		Texture2D Preview;
		DateTime UpdatedAt;
		Noise Noise;
		Property[] Properties;

		public override void OnInspectorGUI()
		{
			var splashImage = NoiseMakerConfig.Instance.SplashMini;
			GUI.Box(new Rect(0f, Screen.height - splashImage.height - (splashImage.height * 0.2f), splashImage.width, splashImage.height), splashImage, GUIStyle.none);

			var typedTarget = target as NoiseAsset;

			var assetPath = AssetDatabase.GetAssetPath(target.GetInstanceID());
			var activelyEditing = assetPath == NoiseMakerWindow.ActiveSavePath;
			var openingAllowed = !(activelyEditing || EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode);

			GUI.enabled = openingAllowed;

			if (GUILayout.Button(activelyEditing ? "Currently Editing" : "Open in Noise Maker")) NoiseMakerWindow.OpenNoiseGraph(assetPath);

			GUI.enabled = true;

			EditorGUILayout.HelpBox("This is a raw noise asset, create an Echo that references this to create instances of it.", MessageType.Info);

			if (UpdatedAt < typedTarget.UpdatedAt)
			{
				Noise = typedTarget.Noise;
				Properties = Noise.PropertyNodes.Select(p => p.Property).ToArray();
				UpdatedAt = typedTarget.UpdatedAt;
			}

			try { DrawProperties(Noise.Seed, Properties); }
			catch (ExitGUIException) {}
			catch (Exception e)
			{
				EditorGUILayout.HelpBox("Unable to draw properties, exception occurred: " + e.Message, MessageType.Error);
				if (GUILayout.Button("Print Exception")) Debug.LogException(e);
			}
		}

		void DrawProperties(int seed, params Property[] properties)
		{
			GUILayout.Label("Default Property Values", EditorStyles.boldLabel);
			if (properties == null) 
			{
				GUILayout.Label("No external properties are defined for this Noise.");
			}

			GUI.enabled = false;

			EditorGUILayout.IntField("Seed", seed);

			foreach (var property in properties)
			{
				var unmodifiedProperty = property;

				var value = unmodifiedProperty.Value;
				var type = unmodifiedProperty.Type;
				var propertyName = StringExtensions.IsNullOrWhiteSpace(unmodifiedProperty.Name) ? "Null name" : unmodifiedProperty.Name;
				var helpboxName = StringExtensions.IsNullOrWhiteSpace(unmodifiedProperty.Name) ? "with a null name" : "\"" + unmodifiedProperty.Name + "\"";

				if (type == typeof(float))
				{
					EditorGUILayout.FloatField(propertyName, (float)value);
				}
				else if (type == typeof(int))
				{
					EditorGUILayout.IntField(propertyName, (int)value);
				}
				else if (type == typeof(bool))
				{
					EditorGUILayout.Toggle(propertyName, (bool)value);
				}
				else if (typeof(Enum).IsAssignableFrom(type))
				{
					EditorGUILayout.EnumPopup(propertyName, (Enum)value);
				}
				else if (type == typeof(Vector3))
				{
					EditorGUILayout.Vector3Field(propertyName, (Vector3)value);
				}
				else if (type == typeof(Color))
				{
					EditorGUILayout.ColorField(propertyName, (Color)value);
				}
				else if (type == typeof(AnimationCurve))
				{
					EditorGUILayout.CurveField(propertyName, (AnimationCurve)value);
				}
				else if (type == typeof(Texture2D))
				{
					EditorGUILayout.ObjectField(propertyName, null, typeof(Texture2D), false);
				}
				else
				{
					GUI.enabled = true;
					GUILayout.BeginHorizontal();
					{
						EditorGUILayout.HelpBox("Property " + helpboxName + " is of unsupported type \"" + type + "\".", MessageType.Error);
						if (GUILayout.Button("Context", EditorStyles.miniButton, GUILayout.Height(40f))) DebugExtensions.OpenFileAtContext();
					}
					GUILayout.EndHorizontal();
					GUI.enabled = false;
				}
			}

			GUI.enabled = true;
		}

		public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
		{
			return Preview ?? (Preview = Instantiate(NoiseMakerConfig.Instance.NoiseIcon));
		}
	}
}