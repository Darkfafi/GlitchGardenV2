using RaDataHolder;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyValueUIElement : RaMonoDataHolderBase<IHaveCurrencyValue>
{
	[SerializeField]
	private Image _icon = null;

	[SerializeField]
	private TMP_Text _label = null;

	protected override void OnSetData()
	{
		_icon.sprite = Data.CurrencyValue.Currency.Icon;
		_label.text = Data.CurrencyValue.Amount.ToString();
	}

	protected override void OnClearData()
	{

	}
}
