using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RaFSM;
using RaTweening;

public class SpawnState : RaGOStateBase<Unit>
{
	[SerializeField]
	private GridModelSO _gridModelSO = null;

	protected override void OnInit()
	{

	}

	protected override void OnEnter()
	{
		if(_gridModelSO.Grid.TryGetUnitSpot(Dependency, out ElementUnitSpot unitSpot))
		{
			Vector3 endPos = unitSpot.GetUnitLocation();
			Vector3 startPos = endPos;
			switch(Dependency.Owner.PlayerType)
			{
				case Player.Type.Home:
					startPos = _gridModelSO.Grid.GetUnitSpot(new Vector2Int(0, unitSpot.Element.Position.y), Dependency.Owner).GetUnitLocation();
					break;
				case Player.Type.Away:
					startPos = _gridModelSO.Grid.GetUnitSpot(new Vector2Int(_gridModelSO.Grid.GridData.Size.x - 1, unitSpot.Element.Position.y), Dependency.Owner).GetUnitLocation();
					break;
			}

			Dependency.transform.position = startPos;
			Dependency.gameObject.SetActive(false);
			RaTweenSequence sequence = RaTweenSequence.Create
			(
				Dependency.transform.TweenScale(0f, 0.4f).SetEasing(RaEasingType.OutBack).From().OnStart(() => Dependency.gameObject.SetActive(true)).ToSequenceEntry(),
				Dependency.transform.TweenMove(endPos, (endPos - startPos).magnitude / 2f).ToSequenceEntry()
			);
		}
	}

	protected override void OnExit(bool isSwitch)
	{

	}

	protected override void OnDeinit()
	{

	}
}
