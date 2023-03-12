using RaDataHolder;
using RaFSM;
using System;
using UnityEngine;

public class Unit : RaMonoDataHolderBase<Unit.CoreData>
{
	[SerializeField]
	private Transform _statesContainer = null;

	[field: SerializeField]
	public UnitVisuals UnitVisuals = null;

	public CoreData UnitData => Data;

	public Player Owner
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

	private RaGOFiniteStateMachine _fsm;

	protected override void OnSetData()
	{
		Owner = Data.Owner;
		Health = new Health(Data.Config.HealthPoints);

		RaGOStateBase[] states = RaGOFiniteStateMachine.GetGOStates(_statesContainer);
		_fsm = new RaGOFiniteStateMachine(this, states);

		UnitVisuals.SetData(this, false);
	}

	protected override void OnClearData()
	{
		UnitVisuals.ClearData();
	}

	protected override void OnSetDataResolved()
	{
		UnitVisuals.Resolve();

		SetState(State.Spawn);
	}

	public bool GetAlignment(Player.Type playerType)
	{
		return Owner != null && Owner.PlayerType == playerType;
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
		public Player Owner;
		public UnitConfig Config;
	}

	public enum State
	{
		Spawn = 0,
		Behaviour = 1,
		Death = 2,
	}
}
