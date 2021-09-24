using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;

namespace GVUZ.Model.Entrants
{
	public class EntitySimpleCache<T> where T : EntityObject
	{
		private readonly Dictionary<int, T> _objectList = new Dictionary<int, T>();

		public void Clear()
		{
			lock(_objectList)
				_objectList.Clear();
		}

		public T Get(int key, Func<int, T> selector)
		{
			lock (_objectList)
			{
				T app;
				if (!_objectList.TryGetValue(key, out app))
				{
					app = selector(key);
					_objectList[key] = app;
				}
				return app;
			}
		}
	}
}
