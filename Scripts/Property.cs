using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LunraGames.NoiseMaker
{
	[Serializable]
	public class Property
	{
		public string Name;
		[SerializeField, JsonProperty]
		string SerializedValue;
		[SerializeField, JsonProperty]
		string TypeName;
		[SerializeField]
		Object AssetValue;

		Type _Type;

		Type Type 
		{
			get { return _Type ?? (_Type = string.IsNullOrEmpty(TypeName) ? null : Type.GetType(TypeName)); }
			set 
			{
				_Type = value;
				TypeName = value == null ? null : value.FullName;
			}
		}

		object _Value;

		[JsonIgnore]
		public object Value
		{
			get 
			{
				if (Type == typeof(Object)) return AssetValue;
				if (string.IsNullOrEmpty(SerializedValue)) return null;
				return _Value ?? (_Value = Serialization.DeserializeJson(Type, SerializedValue, verbose: true));
			}
			set
			{
				Type = value == null ? null : value.GetType();

				_Value = null;
				AssetValue = null;

				if (value == null) return;

				if (Type == typeof(Object)) AssetValue = value as Object;
				else SerializedValue = Serialization.SerializeJson(value, true);
			}
		}
	}
}