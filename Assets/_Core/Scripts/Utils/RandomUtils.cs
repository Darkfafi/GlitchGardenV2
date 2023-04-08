using System;
using System.Collections.Generic;

public static class RandomUtils
{
	public static Random CreateRandom(int? seed = null)
	{
		return seed.HasValue ? new Random(seed.Value) : new Random(UnityEngine.Random.Range(0, int.MaxValue));
	}

	public static T GetRandomEntry<T>(this IList<T> weights)
		where T : IHasWeight
	{
		if(weights.Count == 0)
		{
			return default;
		}

		if(weights.Count == 1)
		{
			return weights[0];
		}

		int maxRoll = 1;

		List<T> sortedWeights = new List<T>(weights);
		sortedWeights.Sort((a, b) => b.Weight - a.Weight);
		for(int i = 0; i < sortedWeights.Count; i++)
		{
			maxRoll += sortedWeights[i].Weight;
		}

		int roll = UnityEngine.Random.Range(0, maxRoll);
		int currentValue = 0;

		for(int i = 0; i < sortedWeights.Count; i++)
		{
			T currentEntry = sortedWeights[i];
			if(roll > currentValue && roll <= currentValue + currentEntry.Weight)
			{
				return currentEntry;
			}
			currentValue += currentEntry.Weight;
		}

		return sortedWeights[0];
	}

	public interface IHasWeight
	{
		int Weight
		{
			get;
		}
	}
}