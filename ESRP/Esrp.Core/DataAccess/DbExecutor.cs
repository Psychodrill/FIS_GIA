using System;
using System.Data.SqlClient;

namespace Esrp.Core.DataAccess
{
	public class DbExecutor : IDisposable
	{
		protected readonly SqlConnection _sqlConnection;

		public DbExecutor()
		{
			_sqlConnection = new SqlConnection(DBSettings.ConnectionString);
			_sqlConnection.Open();
		}

		public virtual SqlCommand CreateCommand()
		{			
			return _sqlConnection.CreateCommand();
		}

		public void Dispose()
		{
			if (_sqlConnection != null)
			{
				_sqlConnection.Close();
				_sqlConnection.Dispose();
			}
		}
	}
}
