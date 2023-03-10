using RaDataHolder;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WalletCurrencyUIElement : RaMonoDataHolderBase<Wallet>
{
	[SerializeField]
	private CurrencyConfig _currencyDisplaying = null;

	[SerializeField]
	private TMP_Text _label = null;

	[SerializeField]
	private Image _icon = null;

#if UNITY_EDITOR
	protected void OnValidate()
	{
		SetCurrencyConfig(_currencyDisplaying);
	}
#endif

	protected override void OnSetData()
	{
		Data.ValueChangedEvent += OnValueChangedEvent;
		SetCurrencyConfig(_currencyDisplaying);
	}

	protected override void OnClearData()
	{
		Data.ValueChangedEvent -= OnValueChangedEvent;
	}

	public void SetCurrencyConfig(CurrencyConfig currency)
	{
		_currencyDisplaying = currency;
		if(_currencyDisplaying != null)
		{
			if(_icon != null)
			{
				_icon.sprite = _currencyDisplaying.Icon;
			}

			int amount = Data != null ? Data.GetAmount(_currencyDisplaying) : 0;
			OnValueChangedEvent(_currencyDisplaying, amount, amount);
		}
		else
		{
			if(_icon != null)
			{
				_icon.sprite = null;
			}

			if(_label != null)
			{
				_label.text = 0.ToString();
			}
		}
	}

	private void OnValueChangedEvent(CurrencyConfig currency, int newValue, int oldValue)
	{
		if(currency == _currencyDisplaying)
		{
			if(_label != null)
			{
				_label.text = newValue.ToString();
			}
		}
	}
}
