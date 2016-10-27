using System;
using Newtonsoft.Json;

namespace LunraGames.NoiseMaker
{
	public abstract class PropertyNode<T> : Node<T>, IPropertyNode
	{
		public string Name { get; set; }
		public bool IsEditable { get; set; }
		public virtual T PropertyValue { get; set; }

		[JsonIgnore]
		public object RawPropertyValue 
		{ 
			get { return PropertyValue; } 
			set 
			{ 
				PropertyValue = (T)value; 
				Value = PropertyValue;
			}
		}

		public override T GetValue (Noise graph)
		{
			Value = PropertyValue;
			return Value;
		}

		[JsonIgnore]
		protected virtual T DefaultValue { get { return default(T); } }
		[JsonIgnore]
		public Property Property
		{
			get
			{
				var property = new Property();
				property.Name = Name;
				property.SetValue(PropertyValue);
				return property;
			}
		}

		public PropertyNode() 
		{
			// This may give you a warning, but it should be safe... since it's just getting a default value.
			PropertyValue = DefaultValue;
		}
	}
}