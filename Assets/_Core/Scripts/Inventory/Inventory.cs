using RaCollection;

public class Inventory
{
	public Wallet Wallet
	{
		get; private set;
	}

	public RaElementCollection<IInventoryItem> Items
	{
		get; private set;
	}

	public Inventory()
	{
		Wallet = new Wallet();
		Items = new RaElementCollection<IInventoryItem>();
	}

	public Inventory(Inventory copyFrom)
	{
		Wallet = new Wallet(copyFrom.Wallet);
		Items = new RaElementCollection<IInventoryItem>(copyFrom.Items);
	}

	public void Clear()
	{
		Wallet.Clear();
		Items.Clear();
	}
}
