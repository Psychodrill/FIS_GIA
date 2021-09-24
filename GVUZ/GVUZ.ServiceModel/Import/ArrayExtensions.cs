using System;
using System.Collections;

namespace GVUZ.ServiceModel.Import
{
	public static class ArrayExtensions
	{
		public static string[] NullOnEmpty(this string[] array)
		{						
			return array.Length == 0 ? null : array;
		}

		public static bool ArraysEqual(Array a1, Array a2)
		{
			if (a1 == a2)
			{
				return true;
			}

			if (a1 == null || a2 == null)
			{
				return false;
			}

			if (a1.Length != a2.Length)
			{
				return false;
			}

			IList list1 = a1, list2 = a2;

			for (int i = 0; i < a1.Length; i++)
			{
				if (!Object.Equals(list1[i], list2[i]))
				{
					return false;
				}
			}
			return true;
		}
	}
}