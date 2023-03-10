using RaDataHolder;

public class Player : RaMonoDataHolderBase<Player.CoreData>
{
	public Type PlayerType => Data.Type;
	public PlayerModel Model => Data.PlayerModel;

	public Wallet Wallet
	{
		get; private set;
	}

	protected override void OnSetData()
	{
		Wallet = new Wallet();
	}

	protected override void OnClearData()
	{
		Wallet.Clear();
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
