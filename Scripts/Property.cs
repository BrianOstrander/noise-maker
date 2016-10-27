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
		string SerializedType;
		[SerializeField, JsonIgnore]
		Object AssetValue;

		Type _Type;

		[JsonIgnore]
		public Type Type 
		{
			get 
			{ 
				return _Type ?? (_Type = string.IsNullOrEmpty(SerializedType) ? null : Type.GetType(SerializedType)); 
			}
			private set 
			{
				_Type = value;
				SerializedType = value == null ? null : value.AssemblyQualifiedName;
			}
		}

		object _Value;

		// todo: this method will override Type sometimes, so we should probably be better about not doing that...
		[JsonIgnore]
		public object Value
		{
			get 
			{
				if (typeof(Object).IsAssignableFrom(Type)) return AssetValue;
				if (_Value == null)
				{
					_Value = Serialization.DeserializeJson(Type, SerializedValue, verbose: true);
					if (_Value is JObject) _Value = (_Value as JObject).ToObject(Type);
				}
				return _Value;
			}
			private set
			{
				_Value = null;
				AssetValue = null;

				if (value == null) return;

				if (typeof(Object).IsAssignableFrom(Type))
				{
					AssetValue = value as Object;
				}
				else SerializedValue = Serialization.SerializeJson(value, true);
			}
		}

		public void SetValue<T>(T value)
		{
			SetValue(value, typeof(T));
		}

		public void SetValue(object value, Type type)
		{
			Value = value;
			Type = type;
		}
	}
}