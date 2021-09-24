using System;
using System.Diagnostics;

namespace FogSoft.Helpers
{
	/// <summary>
	/// Correct IDisposable implementation.</summary>
	[DebuggerStepThrough]
	public abstract class Disposable : IDisposable
	{
		~Disposable()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// This method to override and dispose managed (see param description) and unmanaged resources.</summary>
		/// <param name="disposing">Whether or not called from <see cref="Dispose"/> method (and managed resources should be disposed too).</param>
		protected abstract void Dispose(bool disposing);
	}
}
