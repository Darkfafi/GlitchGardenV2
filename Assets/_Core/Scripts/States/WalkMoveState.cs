using RaFSM;
using RaTweening;
using System.Collections;
using UnityEngine;

public class WalkMoveState : RaGOStateBase<Unit>
{
	[Header("Events")]
	public StateEvent AppliedMoveEvent;
	public StateEvent StartedMoveVisualEvent;
	public StateEvent CompletedMoveVisualEvent;
	public StateEvent FailedMoveEvent;

	[Header("Options")]
	[SerializeField]
	private float _speed = 1f;

	[Header("Dependencies")]
	[SerializeField]
	private UnitsMechanicSO _unitMechanicSO = null;

	private IEnumerator _moveRoutine = null;

	protected override void OnInit()
	{

	}

	protected override void OnEnter()
	{
		StartCoroutine(_moveRoutine = DoMoveRoutine());
	}

	protected override void OnExit(bool isSwitch)
	{
		if(_moveRoutine != null)
		{
			StopCoroutine(_moveRoutine);
			_moveRoutine = null;
		}
	}

	protected override void OnDeinit()
	{

	}

	private IEnumerator DoMoveRoutine()
	{
		Vector2Int newPos = Dependency.Position;
		newPos += Dependency.Owner.GetOrientation(Vector2Int.right);

		ElementUnitSpot newSpot = null;
		while(!_unitMechanicSO.MoveUnit(Dependency, newPos, out newSpot).IsSuccess)
		{
			FailedMoveEvent?.Invoke(this);
			yield return new WaitForSeconds(1f);
		}

		AppliedMoveEvent?.Invoke(this);

		Vector3 destination = newSpot.GetUnitLocation();
		Vector3 delta = destination - Dependency.transform.position;
		Dependency.transform.TweenMove(destination, delta.magnitude / _speed)
			.OnStart(() =>
			{
				StartedMoveVisualEvent.Invoke(this);
			}).OnComplete(() =>
			{
				CompletedMoveVisualEvent.Invoke(this);
			});

		_moveRoutine = null;
	}
}