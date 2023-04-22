using Game.Battle;
using RaCollection;

public class Wallet
{
	public delegate void CurrencyHandler(CurrencyConfig currency, int newValue, int oldValue);

	public event CurrencyHandler EarnEvent;
	public event CurrencyHandler SpendEvent;
	public event CurrencyHandler ValueChangedEvent;

	private RaElementCollection<WalletEntry> _entries = new RaElementCollection<WalletEntry>();

	public Wallet()
	{

	}

	public Wallet(Wallet copyFrom)
	{
		copyFrom._entries.ForEach(x => _entries.Add(new WalletEntry(x.CurrencyValue)));
	}

	public int GetAmount(CurrencyConfig currency)
	{
		if(TryGetCurrencyValue(currency, out CurrencyValue value))
		{
			return value.Amount;
		}
		return 0;
	}

	public void Earn(CurrencyValue value)
	{
		int oldValue;
		int newValue;
		if(TryGetWalletEntry(value.Currency, out WalletEntry walletEntry))
		{
			oldValue = walletEntry.CurrencyValue.Amount;
			newValue = walletEntry.CurrencyValue.Add(value.Amount).Amount;
			walletEntry.SetAmount(newValue);
		}
		else
		{
			_entries.Add(new WalletEntry(value));
			oldValue = 0;
			newValue = value.Amount;
		}
		EarnEvent?.Invoke(value.Currency, newValue, oldValue);
		ValueChangedEvent?.Invoke(value.Currency, newValue, oldValue);
	}

	public bool CanSpend(CurrencyValue value, out NotEnoughCurrency notEnoughCurrencyData)
	{
		notEnoughCurrencyData = new NotEnoughCurrency()
		{
			OwnedValue = new CurrencyValue(value.Currency, GetAmount(value.Currency)),
			RequiredValue = value,
		};

		if(value.Amount <= 0)
		{
			return true;
		}

		return notEnoughCurrencyData.OwnedValue.Amount >= notEnoughCurrencyData.RequiredValue.Amount;
	}

	public bool Spend(CurrencyValue value, out NotEnoughCurrency notEnoughCurrencyData)
	{
		if(CanSpend(value, out notEnoughCurrencyData))
		{
			int oldValue = 0;
			int newValue = 0;
			if(TryGetWalletEntry(value.Currency, out WalletEntry walletEntry))
			{
				oldValue = walletEntry.CurrencyValue.Amount;
				newValue = walletEntry.CurrencyValue.Subtract(value.Amount).Amount;
				walletEntry.SetAmount(newValue);
			}
			SpendEvent?.Invoke(value.Currency, newValue, oldValue);
			ValueChangedEvent?.Invoke(value.Currency, newValue, oldValue);
			return true;
		}
		return false;
	}

	public bool TryGetCurrencyValue(CurrencyConfig currency, out CurrencyValue value)
	{
		if(TryGetWalletEntry(currency, out WalletEntry entry))
		{
			value = entry.CurrencyValue;
			return true;
		}
		value = default;
		return false;
	}

	public bool TryGetWalletEntry(CurrencyConfig currency, out WalletEntry walletEntry)
	{
		return _entries.TryGetItem(currency.Id, out walletEntry);
	}

	public void Clear()
	{
		_entries.Clear();
	}

	public class WalletEntry : IRaCollectionElement
	{
		public string Id => CurrencyValue.Id;

		public CurrencyValue CurrencyValue
		{
			get; private set;
		}

		public WalletEntry(CurrencyValue value)
		{
			CurrencyValue = value;
		}

		public void SetAmount(int amount)
		{
			CurrencyValue = new CurrencyValue(CurrencyValue.Currency, amount);
		}
	}

	public struct NotEnoughCurrency
	{
		public CurrencyValue OwnedValue;
		public CurrencyValue RequiredValue;
	}
}
