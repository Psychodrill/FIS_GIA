using System;
using System.Collections;
using System.Diagnostics;

namespace FogSoft.Helpers
{
	/// <summary>
	/// Provides several methods to operate with <see cref="IDisposable"/>.</summary>
	[DebuggerStepThrough]
	public static class DisposableExtensions
	{
		public static void SafeDispose(this IDisposable value)
		{
			if (!ReferenceEquals(value, null))
			{
				value.Dispose();
			}
		}

		public static void DisposeElements(this ICollection collection)
		{
			if (collection.IsNullOrEmpty())
			{
				return;
			}
			foreach (object item in collection)
			{
				SafeDispose(item as IDisposable);
			}
		}
	}
}
