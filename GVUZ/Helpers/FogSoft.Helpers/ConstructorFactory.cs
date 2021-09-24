using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

namespace FogSoft.Helpers
{
	/// <summary>
	/// Simple constructor creation for use cases like <see cref="Locator"/>.</summary>
	[DebuggerStepThrough]
	public static class ConstructorFactory
	{
		public static DynamicMethod Create(Type type, params Type[] parameters)
		{
			ConstructorInfo constructor = GetConstructor(type, parameters);
			DynamicMethod method = new DynamicMethod(GenerateMethodName(type), type, parameters);

			EmitConstructor(parameters, constructor, method.GetILGenerator());
			return method;
		}

		private static void EmitConstructor(Type[] parameters, ConstructorInfo constructor, ILGenerator generator)
		{
			for (int index = 0; index < parameters.Length; index++)
			{
				generator.Emit(OpCodes.Ldarg, index);
			}

			generator.Emit(OpCodes.Newobj, constructor);
			generator.Emit(OpCodes.Ret);
		}

		public static ConstructorInfo GetConstructor(Type type, params Type[] parameters)
		{
			if (type == null) throw new ArgumentNullException("type");
			if (parameters == null) throw new ArgumentNullException("parameters");

			ConstructorInfo constructor = type.GetConstructor(parameters);
			if (constructor == null)
				throw new TypeLoadException(string.Format("Type {0} should has constructor.", type));
			return constructor;
		}

		private static string GenerateMethodName(Type type)
		{
			// название можно не делать уникальным для разных параметров - метод все равно зависит от параметров
			return "CF_FACTORY_" + type.NormalizeName();
		}
	}
}
