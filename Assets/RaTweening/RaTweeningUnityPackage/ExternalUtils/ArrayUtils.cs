using System.Collections;
using UnityEngine;

namespace RaTweening.Tools
{
	internal static class ArrayUtils
	{
		public static int GetWrappedIndex(this IList self, int index)
		{
			int count = self.Count;
			return (index % count + count) % count;
		}

		public static int GetIndexByNormalizedValue(this IList self, float normalizedValue)
		{
			int count = self.Count;
			int value = Mathf.FloorToInt(count * normalizedValue);
			int index = value % count;

			if(index == 0 && Mathf.Approximately(1f, normalizedValue))
			{
				index = count - 1;
			}

			return GetWrappedIndex(self, index);
		}
	}
}