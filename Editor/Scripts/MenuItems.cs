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

		[MenuItem("Assets/Create/Noise Maker Echo")]
		static void CreateEchoAsset()
		{
			var path = "Assets";
			var selection = Selection.GetFiltered(typeof(Object), SelectionMode.Assets).FirstOrDefault();
			if (selection != null)
			{
				path = AssetDatabase.GetAssetPath(selection);
				if (File.Exists(path)) path = Path.GetDirectoryName(path);

			}

			AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<EchoAsset>(), Path.Combine(path, "Echo.asset"));
			AssetDatabase.SaveAssets();
		}
	}
}