using RaDataHolder;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Battle
{
	public class BattlePlayer : RaMonoDataHolderBase<BattlePlayer.CoreData>
	{
		public Type PlayerType => Data.Type;
		public PlayerModel Model => Data.PlayerModel;

		public Inventory Inventory
		{
			get; private set;
		}

		public Health Health
		{
			get; private set;
		}

		protected override void OnSetData()
		{
			if(Model.Health != null)
			{
				Health = new Health(Model.Health.HP);
			}
			else
			{
				Health = null;
			}

			Inventory = new Inventory(Model.Inventory);
		}

		protected override void OnClearData()
		{
			Inventory.Clear();

			if(Health != null)
			{
				Health.Dispose();
				Health = null;
			}
		}

		public List<UnitModel> GetUnits(Predicate<UnitModel> predicate = null)
		{
			return Inventory.Items.GetItems<UnitModel>(predicate);
		}

		public Vector3 GetOrientation(Vector3 value)
		{
			value.x = GetOrientation(value.x);
			return value;
		}

		public Vector2Int GetOrientation(Vector2Int value)
		{
			value.x = (int)GetOrientation(value.x);
			return value;
		}

		public float GetOrientation(float value)
		{
			if(PlayerType == Type.Away)
			{
				value *= -1;
			}
			return value;
		}

		public Type GetOppositePlayerType()
		{
			return GetOppositePlayerType(PlayerType);
		}

		public static Type GetOppositePlayerType(Type type)
		{
			switch(type)
			{
				case Type.Home:
					return Type.Away;
				case Type.Away:
					return Type.Home;
				default:
					return default;
			}
		}

		public enum Type
		{
			Home,
			Away
		}

		public struct CoreData
		{
			public PlayerModel PlayerModel;
			public Type Type;
		}
	}
}