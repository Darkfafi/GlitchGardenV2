using RaDataHolder;

public class Player : RaMonoDataHolderBase<PlayerModel>
{
	public PlayerModel Model => Data;

	protected override void OnSetData()
	{

	}

	protected override void OnClearData()
	{

	}
}
