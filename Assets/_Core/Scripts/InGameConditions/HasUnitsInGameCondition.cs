using RaCollection;
using UnityEngine;

public class HasUnitsInGameCondition : InGameConditionBase
{
	[SerializeField]
	private UnitsMechanicSO _unitsMechanicSO = null;

	private int _unitsCounter = 0;

	protected override void OnStartRunning()
	{
		_unitsMechanicSO.Units.AddedItemEvent += OnUnitAddedEvent;
		_unitsMechanicSO.Units.RemovedItemEvent += OnUnitRemovedEvent;
		_unitsMechanicSO.Units.ForEachReadOnly((item, index) => OnUnitAddedEvent(item, index, null));
	}

	protected override void OnStopRunning()
	{
		_unitsMechanicSO.Units.AddedItemEvent -= OnUnitAddedEvent;
		_unitsMechanicSO.Units.RemovedItemEvent -= OnUnitRemovedEvent;
		_unitsCounter = 0;
	}

	protected override bool CheckCondition() => _unitsCounter > 0;

	private void OnUnitAddedEvent(Unit item, int index, RaCollection<Unit> source)
	{
		if(item.Owner.PlayerType == PlayerSideType)
		{
			_unitsCounter++;
			SetInGame(CheckCondition());
		}
	}

	private void OnUnitRemovedEvent(Unit item, int index, RaCollection<Unit> source)
	{
		if(item.Owner.PlayerType == PlayerSideType)
		{
			_unitsCounter--;
			SetInGame(CheckCondition());
		}
	}
}
