using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LunraGames.NoiseMaker
{
	[Serializable]
	public class Property
	{
		public string Name;
		[JsonProperty]
		object _Value;
		[JsonProperty]
		Type Type;

		[JsonIgnore]
		public object Value
		{
			get 
			{
				if (_Value == null) return null;

				// hack to fix newtonsoft defaulting objects to doubles.
				if (_Value is double) return (_Value = Convert.ToSingle((double)_Value));
				if (_Value is long) return (_Value = Convert.ToInt32((long)_Value));
				// Certain objects have a hard time escaping being JObjects, since _Value's type is just object, 
				// so we explicitely convert them here.
				if (_Value is JObject) return (_Value = (_Value as JObject).ToObject(Type));
				if (typeof(Enum).IsAssignableFrom(Type)) return (_Value = _Value is Enum ? _Value : Enum.Parse(Type, Enum.GetNames(Type)[(int)_Value]));

				return _Value;
			}
			set
			{
				_Value = value;
				Type = value == null ? null : value.GetType();
			}
		}
	}
}