using System.Linq;
using UnityEditor;
using LunraGamesEditor.PlugIt;
using UnityEngine;
using LunraGames.NoiseMaker;
using System.IO;

namespace LunraGamesEditor.NoiseMaker
{
	public static class MenuItems
	{
		[MenuItem(LunraGamesEditor.Strings.Feedback + Strings.Plugin)]
		static void LaunchFeedback()
		{
			Helper.LaunchFeedback(LunraGamesEditor.Strings.Company, Strings.Plugin);
		}

		[MenuItem("Assets/Create/Noise Draft")]
		static void CreateNoiseDraftAsset()
		{
			var path = "Assets";
			var selection = Selection.GetFiltered(typeof(Object), SelectionMode.Assets).FirstOrDefault();
			if (selection != null)
			{
				path = AssetDatabase.GetAssetPath(selection);
				if (File.Exists(path)) path = Path.GetDirectoryName(path);

			}

			AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<NoiseDraftAsset>(), Path.Combine(path, "NoiseDraft.asset"));
			AssetDatabase.SaveAssets();
		}
	}
}