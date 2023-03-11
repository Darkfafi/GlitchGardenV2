using RaDataHolder;
using RaFSM;
using UnityEngine.Events;

public abstract class StateSwitcherBase : RaMonoDataHolderBase<RaGOStateBase>
{
	public UnityEvent SwitcherTrueEvent;
	public UnityEvent SwitcherFalseEvent;

	protected override void OnSetData()
	{
	}

	protected override void OnClearData()
	{
	}

	public void RunSwitcher(RaGOStateBase state)
	{
		if(CheckCondition(state))
		{
			SwitcherTrueEvent.Invoke();
		}
		else
		{
			SwitcherFalseEvent.Invoke();
		}
	}

	protected abstract bool CheckCondition(RaGOStateBase state);

	[System.Serializable]
	public class SwitcherUnityEvent : UnityEvent<RaGOStateBase>
	{
	
	}
}
