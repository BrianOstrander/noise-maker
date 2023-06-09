﻿using UnityEngine;
using UnityEditor;
using System;

namespace LunraGames.NoiseMaker
{
	[CustomEditor(typeof(NoiseFilter), true)]
	public class NoiseFilterEditor : Editor
	{
		SerializedProperty GenerateOnAwake;
		SerializedProperty OverrideSeed;
		SerializedProperty Seed;
		SerializedProperty Echo;
		SerializedProperty MercatorMap;
		SerializedProperty MapWidth;
		SerializedProperty MapHeight;
		SerializedProperty Translation;
		SerializedProperty Rotation;
		SerializedProperty Scale;
		SerializedProperty Filtering;
		SerializedProperty Datum;
		SerializedProperty Deviation;

		void OnEnable()
		{
			GenerateOnAwake = serializedObject.FindProperty("GenerateOnAwake");
			OverrideSeed = serializedObject.FindProperty("OverrideSeed");
			Seed = serializedObject.FindProperty("Seed");
			Echo = serializedObject.FindProperty("Echo");
			MercatorMap = serializedObject.FindProperty("MercatorMap");
			MapWidth = serializedObject.FindProperty("MapWidth");
			MapHeight = serializedObject.FindProperty("MapHeight");
			Translation = serializedObject.FindProperty("Translation");
			Rotation = serializedObject.FindProperty("Rotation");
			Scale = serializedObject.FindProperty("Scale");
			Filtering = serializedObject.FindProperty("Filtering");
			Datum = serializedObject.FindProperty("Datum");
			Deviation = serializedObject.FindProperty("Deviation");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			var changed = false;

			// todo: implement filtering
			//Filtering.enumValueIndex = Deltas.DetectDelta(Filtering.enumValueIndex, GUILayout.Toolbar(Filtering.enumValueIndex, Enum.GetNames(typeof(NoiseMaker.Filtering))), ref changed);
			var filtering = (Filtering)Filtering.enumValueIndex;

			GUI.enabled = Application.isPlaying && Echo.objectReferenceValue != null && MercatorMap.objectReferenceValue != null;
			if (GUILayout.Button("Regenerate")) (target as NoiseFilter).Regenerate();
			GUI.enabled = true;

			EditorGUILayout.PropertyField(GenerateOnAwake);

			EditorGUILayout.PropertyField(OverrideSeed);

			if (OverrideSeed.boolValue) EditorGUILayout.PropertyField(Seed);

			if (GenerateOnAwake.boolValue && Echo.objectReferenceValue == null) EditorGUILayout.HelpBox("An Echo asset must be specified before the gameobject is enabled for the first time, or an error will occur.", MessageType.Warning);

			EditorGUILayout.PropertyField(Echo);

			Datum.floatValue = Deltas.DetectDelta(Datum.floatValue, EditorGUILayout.DelayedFloatField(new GUIContent("Datum", "Datum is similar to a \"sea level\" that all values are relative to."), Datum.floatValue), ref changed);
			if (changed && Datum.floatValue <= 0f)
			{
				Datum.floatValue = NoiseFilter.DefaultDatum;
				EditorUtility.DisplayDialog("Invalid", "Datum must remain greater than zero.", "Okay");
			}
			changed = false;

			Deviation.floatValue = Deltas.DetectDelta(Deviation.floatValue, EditorGUILayout.DelayedFloatField(new GUIContent("Deviation", "Deviation is the scalar of the datam to multiply the values by."), Deviation.floatValue), ref changed);
			if (changed && Deviation.floatValue < 0f)
			{
				Deviation.floatValue = NoiseFilter.DefaultDeviation;
				EditorUtility.DisplayDialog("Invalid", "Deviation can't be negative.", "Okay");
			}
			changed = false;

			EditorGUILayout.PropertyField(Translation);
			EditorGUILayout.PropertyField(Rotation);
			EditorGUILayout.PropertyField(Scale);

			if (GenerateOnAwake.boolValue && MercatorMap.objectReferenceValue == null) EditorGUILayout.HelpBox("A Mercator Map must be specified before the gameobject is enabled for the first time, or an error will occur.", MessageType.Warning);

			EditorGUILayout.PropertyField(MercatorMap);

			MapWidth.intValue = Deltas.DetectDelta(MapWidth.intValue, EditorGUILayout.DelayedIntField(new GUIContent("Texture Width", "The height in pixels of the texture to apply the specified Mercator to."), MapWidth.intValue), ref changed);
			if (changed)
			{
				if (MapWidth.intValue < 1)
				{
					MapWidth.intValue = 1;
					EditorUtility.DisplayDialog("Invalid", "Width must remain greater than 1.", "Okay");
				}
				if (filtering == NoiseMaker.Filtering.Sphere) MapHeight.intValue = Mathf.CeilToInt((float)MapWidth.intValue * 0.5f);
			}
			changed = false;

			MapHeight.intValue = Deltas.DetectDelta(MapHeight.intValue, EditorGUILayout.DelayedIntField(new GUIContent("Texture Height", "The height in pixels of the texture to apply the specified Mercator to."), MapHeight.intValue), ref changed);
			if (changed)
			{
				if (MapHeight.intValue < 1)
				{
					MapHeight.intValue = 1;
					EditorUtility.DisplayDialog("Invalid", "Height must remain greater than 1.", "Okay");
				}
				if (filtering == NoiseMaker.Filtering.Sphere) MapWidth.intValue = Mathf.CeilToInt(MapHeight.intValue * 2);
			}
			changed = false;

			serializedObject.ApplyModifiedProperties();
		}
	}
}