using System;

namespace LunraGamesEditor.NoiseMaker
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class DomainDrawer : Attribute
	{
		public Type Target;
		public string Name;
		public string Description;

		public DomainDrawer(Type target, string name, string description)
		{
			Target = target;
			Name = name;
			Description = description;
		}
	}
}