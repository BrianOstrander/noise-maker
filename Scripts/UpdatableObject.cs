using System;
using UnityEngine;

namespace LunraGames.NoiseMaker
{
	public abstract class UpdatableObject : ScriptableObject
	{
		[SerializeField]
		string SerializedLastUpdated;
		DateTime? _LastUpdated;

		public DateTime UpdatedAt
		{
			get
			{
				if (_LastUpdated.HasValue) return _LastUpdated.Value;
				if (string.IsNullOrEmpty(SerializedLastUpdated)) return UpdatedAt = DateTime.MinValue;
				long ticks;
				var wasParsed = long.TryParse(SerializedLastUpdated, out ticks);
				return wasParsed ? UpdatedAt = new DateTime(ticks) : UpdatedAt = DateTime.MinValue;
			}
			protected set
			{
				_LastUpdated = value;
				SerializedLastUpdated = value.Ticks.ToString();
			}
		}
	}
}