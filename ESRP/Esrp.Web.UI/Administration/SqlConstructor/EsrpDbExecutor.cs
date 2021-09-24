using System;
using System.Data.SqlClient;
using Esrp.Core.DataAccess;

namespace Esrp.Web.Administration.SqlConstructor
{
	public class EsrpDbExecutor : DbExecutor
	{
		private readonly SqlConstructor_GetData _sqlConstructor;

		public EsrpDbExecutor(SqlConstructor_GetData sqlConstructor)
		{
			_sqlConstructor = sqlConstructor;
		}

		public override SqlCommand CreateCommand()
		{
			SqlCommand sqlCommand = _sqlConstructor.GetSQL();
			sqlCommand.Connection = _sqlConnection;
			return sqlCommand;
		}

		public SqlCommand CreateSqlCommand()
		{
			return CreateCommand();
		}

		public SqlCommand CreateCountRowsCommand()
		{
			SqlCommand sqlCommand = _sqlConstructor.GetCountOrgsSQL();
			sqlCommand.Connection = _sqlConnection;
			return sqlCommand;
		}
	}
}