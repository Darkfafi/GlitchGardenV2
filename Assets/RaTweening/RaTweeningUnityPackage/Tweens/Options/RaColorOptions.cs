using System;
using UnityEngine;

namespace RaTweening
{
	/// <summary>
	/// This Util class hold the tools to exclude various color channels from tweening calculations
	/// </summary>
	public static class RaColorOptions
	{
		#region Public Methods

		/// <summary>
		/// Returns a <see cref="Channel"/> which only contains the Channels which are not defined within the input
		/// </summary>
		/// <param name="inclAxes">Input which Channels not to define</param>
		/// <returns>The value of all Channels excluding the input</returns>
		public static Channel GetOnlyIncludeChannels(Channel inclChannels)
		{
			Channel returnValue = Channel.None;

			if(!inclChannels.HasFlag(Channel.R))
			{
				returnValue |= Channel.R;
			}

			if(!inclChannels.HasFlag(Channel.G))
			{
				returnValue |= Channel.G;
			}

			if(!inclChannels.HasFlag(Channel.B))
			{
				returnValue |= Channel.B;
			}

			if(!inclChannels.HasFlag(Channel.A))
			{
				returnValue |= Channel.A;
			}

			return returnValue;
		}

		/// <summary>
		/// Resets the final result to the original Color based on which Channels are to be excluded from the final calculation
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="final">The final value</param>
		/// <param name="excludeAxes">The channels which should be reset to the original value within the final value</param>
		/// <returns>The new final value</returns>
		public static Color ApplyExcludeChannels(Color original, Color final, Channel excludeChannels)
		{
			if(excludeChannels.HasFlag(Channel.R))
			{
				final.r = original.r;
			}

			if(excludeChannels.HasFlag(Channel.G))
			{
				final.g = original.g;
			}

			if(excludeChannels.HasFlag(Channel.B))
			{
				final.b = original.b;
			}

			if(excludeChannels.HasFlag(Channel.A))
			{
				final.a = original.a;
			}

			return final;
		}

		#endregion


		#region Nested

		/// <summary>
		/// Flag based enum representing Color Channels
		/// </summary>
		[Flags]
		public enum Channel
		{
			None = 0,
			/// <summary>
			/// The Red Channel
			/// </summary>
			R = 1,
			/// <summary>
			/// The Green Channel
			/// </summary>
			G = 2,
			/// <summary>
			/// The Blue Channel
			/// </summary>
			B = 4,
			/// <summary>
			/// The Alpha Channel
			/// </summary>
			A = 8,
			All = R | G | B | A
		}

		#endregion
	}
}