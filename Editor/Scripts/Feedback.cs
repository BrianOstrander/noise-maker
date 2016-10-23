using UnityEditor;
using LunraGamesEditor.PlugIt;

namespace LunraGamesEditor.NoiseMaker
{
	public static class Feedback
	{
		[MenuItem(LunraGamesEditor.Strings.Feedback + Strings.Plugin)]
		static void LaunchFeedback()
		{
			PlugIt.Helper.LaunchFeedback(LunraGamesEditor.Strings.Company, Strings.Plugin);
		}
	}
}