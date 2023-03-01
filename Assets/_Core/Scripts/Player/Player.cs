using RaDataHolder;

public class Player : RaMonoDataHolderBase<Player.CoreData>
{
	public Type PlayerType => Data.Type;
	public PlayerModel Model => Data.PlayerModel;

	protected override void OnSetData()
	{

	}

	protected override void OnClearData()
	{

	}

	public enum Type
	{
		Home, 
		Away
	}

	public struct CoreData
	{
		public PlayerModel PlayerModel;
		public Type Type;
	}
}
