using RaDataHolder;
using System;
using UnityEngine;
using RaFSM;

public class Unit : RaMonoDataHolderBase<Unit.CoreData>
{
	[SerializeField]
	private SpriteRenderer _renderer = null;

	[SerializeField]
	private Transform _statesContainer = null;

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

		_renderer.sprite = Data.Config.Icon;
		
		Health = new Health(Data.Config.HealthPoints);

		_fsm = new RaGOFiniteStateMachine(this, RaGOFiniteStateMachine.GetGOStates(_statesContainer));
	}

	protected override void OnClearData()
	{

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
