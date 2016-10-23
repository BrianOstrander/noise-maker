using System.Collections.Generic;
using LunraGames.NoiseMaker;

namespace LunraGamesEditor.NoiseMaker
{
	public class NodeEditorEntry
	{
		public NodeDrawer Details;
		public NodeEditor Editor;
		public List<NodeLinker> Linkers;
		public bool IsEditable;
	}
}