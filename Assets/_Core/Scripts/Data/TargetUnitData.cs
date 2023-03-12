using UnityEngine;

public class TargetUnitData : MonoBehaviour
{
	[field: SerializeField]
	public Unit CurrentTarget
	{
		get; private set;
	}

	public bool SetTarget(Unit unit)
	{
		if(CurrentTarget != unit)
		{
			CurrentTarget = unit;
			return true;
		}
		return false;
	}
}
