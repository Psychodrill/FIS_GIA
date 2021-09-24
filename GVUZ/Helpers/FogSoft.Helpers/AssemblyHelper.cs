using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;

namespace FogSoft.Helpers
{
	/// <summary>
	/// 	Realizes extension methods to deal with types in the <see cref = "AppDomain" /> or <see cref = "Assembly" />.
	/// </summary>
	//[DebuggerStepThrough]
	public static class AssemblyHelper
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [DebuggerNonUserCode]
        public static IEnumerable<Type> GetTypesWith<TAttribute>(this AppDomain domain, bool ignoreSystem = true, bool inherit = true)
			where TAttribute : Attribute
		{
			var types = new List<Type>();
			foreach (var assembly in domain.GetAssemblies())
			{
                if (IgnoredAssembly(assembly.FullName))
                    continue;
                if (ignoreSystem && IgnoredAssembly(assembly.FullName))
					continue;

				try
				{
					types.AddRange(assembly.GetTypesWith<TAttribute>(inherit));
				}
				catch (ReflectionTypeLoadException rtlex)
				{
					LogTypeLoadException(rtlex, assembly, typeof(TAttribute));
				}
				catch (StackOverflowException)
				{
					throw;
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch (Exception ex)
				{
					LogGeneralException(ex, typeof(TAttribute), assembly);
				}
			}

			return types.AsReadOnly();
		}

		private static void LogGeneralException(Exception ex, Type type, Assembly assembly)
		{
			if (Log.IsErrorEnabled)
			{
				Log.Error("Ignore scanning assembly " + assembly + " for types with " + type + " attribute - " + ex.Message, ex);
			}
		}

		private static void LogTypeLoadException(ReflectionTypeLoadException rtlex, Assembly assembly, Type type)
		{
			if (!Log.IsErrorEnabled) return;

			Log.Error(new StringBuilder()
			          	.Append("Ignore scanning assembly " + assembly + " for types with " + type + " attribute")
			          	.AppendWith(rtlex.LoaderExceptions.Select(x => x.Message).ToArray(), " , ")
			          	.Append("."), rtlex);
		}

		private static bool IgnoredAssembly(string fullName)
		{
			return fullName.StartsWith("System.") || fullName.StartsWith("mscorlib,") ||
			       fullName.StartsWith("Microsoft.") || fullName.StartsWith("nunit.");
		}

		public static IEnumerable<Type> GetTypesWith<TAttribute>(this Assembly assembly, bool inherit = true)
			where TAttribute : Attribute
		{
			return assembly.GetTypes()
				.Where(type => type.GetCustomAttributes(typeof(TAttribute), true).Length > 0).ToArray();
		}

		public static IEnumerable<Type> GetInheritors<T>(this Assembly assembly) where T : class
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");

			return assembly.GetTypes().Where(type => typeof(T).IsAssignableFrom(type));
		}
	}
}
