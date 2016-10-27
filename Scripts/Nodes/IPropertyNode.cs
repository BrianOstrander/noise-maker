namespace LunraGames.NoiseMaker
{
	public interface IPropertyNode : INode
	{
		string Name { get; set; }
		bool IsEditable { get; set; }
		object RawPropertyValue { get; set; }
		Property Property { get; }
	}
}