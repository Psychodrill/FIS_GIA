using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Objects;
using System.Linq;
using System.Text;
using FogSoft.Helpers;

namespace GVUZ.Model
{
	public static class EntitiesHelper
	{
        public static int CommandTimeout { get { return _commandTimeout; } }
		private static readonly int _commandTimeout = -1;

		static EntitiesHelper()
		{
			if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SqlCommandTimeout"]))
				_commandTimeout = ConfigurationManager.AppSettings["SqlCommandTimeout"].To(0);
		}

		public static void InitCommandTimeout(this ObjectContext context)
		{
			if (_commandTimeout > 0)
				context.CommandTimeout = _commandTimeout;
		}
	}
}
