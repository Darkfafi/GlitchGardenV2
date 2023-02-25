using RaBehaviourSO;

public abstract class ModelSOBase : RaBehaviourSOBase
{
	protected Game Game
	{
		get; private set;
	}

	public bool HasSetData
	{
		get; private set;
	}

	public void SetData(Game game)
	{
		if(IsInitialized)
		{
			return;
		}

		Game = game;
		HasSetData = true;
	}

	public void ClearData()
	{
		if(!IsInitialized)
		{
			return;
		}

		HasSetData = false;
		Game = null;
	}

	protected override void OnDispose()
	{
		Game = null;
	}
}
