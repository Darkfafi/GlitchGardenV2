using RaFSM;
using RaTweening;
using UnityEngine;

public class SpawnState : RaGOStateBase<Unit>
{
	[SerializeField]
	private GridModelSO _gridModelSO = null;

	protected UnitVisuals UnitVisuals => Dependency.UnitVisuals;

	protected override void OnInit()
	{

	}

	protected override void OnEnter()
	{
		if(_gridModelSO.Grid.TryGetUnitSpot(Dependency, out ElementUnitSpot unitSpot))
		{
			UnitVisuals.VisualsContainer.gameObject.SetActive(false);
			Dependency.transform.TweenScale(0f, 0.5f)
				.SetEasing(RaEasingType.OutBack).From()
				.OnStart(() =>
				{
					Dependency.transform.position = unitSpot.GetUnitLocation();
					UnitVisuals.VisualsContainer.gameObject.SetActive(true);
				});
		}
	}

	protected override void OnExit(bool isSwitch)
	{

	}

	protected override void OnDeinit()
	{

	}
}
