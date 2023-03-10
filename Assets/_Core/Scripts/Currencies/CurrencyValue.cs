using UnityEngine;
using RaCollection;
using System;

[Serializable]
public struct CurrencyValue : IRaCollectionElement, IHaveCurrencyValue
{
	public string Id => Currency.Id;

	[field: SerializeField]
	public CurrencyConfig Currency
	{
		get; private set;
	}

	[field: SerializeField]
	public int Amount
	{
		get; private set;
	}

	CurrencyValue IHaveCurrencyValue.CurrencyValue => this;

	public CurrencyValue(CurrencyConfig config, int amount)
	{
		Currency = config;
		Amount = amount;
	}

	public CurrencyValue Set(int amount)
	{
		Amount = amount;
		return this;
	}

	public CurrencyValue Add(int amount)
	{
		Amount += Mathf.Max(amount, 0);
		return this;
	}

	public CurrencyValue Subtract(int amount)
	{
		Amount -= Mathf.Max(amount, 0);
		return this;
	}

	public CurrencyValue Multiply(int amount)
	{
		Amount *= Mathf.Max(amount, 0);
		return this;
	}

	public bool HasAmount(int amount)
	{
		return Amount >= amount;
	}
}

public interface IHaveCurrencyValue
{
	public CurrencyValue CurrencyValue
	{
		get;
	}
}