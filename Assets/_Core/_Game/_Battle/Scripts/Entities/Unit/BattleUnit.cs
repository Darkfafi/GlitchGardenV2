﻿using RaDataHolder;
using RaFSM;
using System;
using UnityEngine;

namespace Game.Battle
{
	public class BattleUnit : RaMonoDataHolderBase<BattleUnit.CoreData>
	{
		[SerializeField]
		private Transform _statesContainer = null;

		[field: SerializeField]
		public BattleUnitVisuals UnitVisuals = null;

		[field: SerializeField]
		public Collider2D Collider = null;

		public CoreData UnitData => Data;

		public BattlePlayer Owner
		{
			get; private set;
		}

		public Health Health
		{
			get; private set;
		}

		public Vector2Int Position
		{
			get; private set;
		}

		public bool IsActiveUnit => Health.HP > 0 && _fsm.CurrentStateIndex == (int)State.Behaviour;

		private RaGOFiniteStateMachine _fsm;

		protected override void OnInitialization()
		{
			Collider.enabled = false;
		}

		protected override void OnSetData()
		{
			Owner = Data.Owner;
			Health = new Health(Data.BattleUnitData.HealthPoints);

			RaGOStateBase[] states = RaGOFiniteStateMachine.GetGOStates(_statesContainer);
			_fsm = new RaGOFiniteStateMachine(this, states);
			UnitVisuals.SetData(this, false);
		}

		protected override void OnClearData()
		{
			UnitVisuals.ClearData();

			if(_fsm != null)
			{
				_fsm.Dispose();
				_fsm = null;
			}

			Collider.enabled = false;

			Health.Dispose();
			Health = null;
		}

		protected override void OnSetDataResolved()
		{
			UnitVisuals.Resolve();
			SetState(State.Spawn);
		}

		public bool GetAlignment(BattlePlayer.Type playerType)
		{
			return Owner != null && Owner.PlayerType == playerType;
		}

		public void SetStateToDeath()
		{
			if(_fsm.CurrentStateIndex != (int)State.Death)
			{
				SetState(State.Death);
			}
		}

		public void SetStateToBehaviour()
		{
			if(_fsm.CurrentStateIndex != (int)State.Behaviour)
			{
				SetState(State.Behaviour);
			}
		}

		public void SetState(State state)
		{
			_fsm.SwitchState((int)state);
			_statesContainer.name = $"States ({state})";
		}

		public void SetPosition(Vector2Int position)
		{
			Position = position;
		}

		[Serializable]
		public struct CoreData
		{
			public BattlePlayer Owner;
			public UnitModel UnitModel;
			public UnitConfig Config => UnitModel.Config;
			public BattleUnitConfigData BattleUnitData => UnitModel.BattleUnitData;
		}

		public enum State
		{
			Spawn = 0,
			Behaviour = 1,
			Death = 2,
		}
	}
}