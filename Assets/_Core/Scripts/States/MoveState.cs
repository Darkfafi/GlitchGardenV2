using RaFSM;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MoveState : RaGOStateBase<Unit>
{
	[Header("Events")]
	public UnityEvent MoveEvent;
	[Header("Events")]
	public UnityEvent FailedMoveEvent;

	[Header("Dependencies")]
	[SerializeField]
	private GridModelSO _gridModelSO = null;
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
		
		while(!_unitMechanicSO.MoveUnit(Dependency, newPos).IsSuccess)
		{
			FailedMoveEvent?.Invoke();
			yield return new WaitForSeconds(1f);
		}
		_moveRoutine = null;

		MoveEvent?.Invoke();
	}
}

// When I try to move, I can't 