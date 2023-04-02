using RaDataHolder;
using RaFSM;
using static RaFSM.RaGOStateBase;

public abstract class StateSwitcherBase : RaMonoDataHolderBase<RaGOStateBase>
{
	public RaGOStateEvent SwitcherTrueEvent;
	public RaGOStateEvent SwitcherFalseEvent;

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
			SwitcherTrueEvent.Invoke(state);
		}
		else
		{
			SwitcherFalseEvent.Invoke(state);
		}
	}

	protected abstract bool CheckCondition(RaGOStateBase state);

	public void FSM_GoToNextState(RaGOStateBase state)
	{
		state.GetDependency<IRaFSMCycler>().GoToNextState();
	}
}
