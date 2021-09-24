namespace Ege.Dal.Common.Repositories
{
    using System.Data;
    using System.Data.Common;
    using JetBrains.Annotations;

    internal abstract class Repository
    {
        [NotNull]
        protected DbCommand StoredProcedureCommand([NotNull] DbConnection connection, string name)
        {
            var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = name;
            return command;
        }

        [NotNull]
        protected DbParameter AddParameter<T>([NotNull] DbCommand command, string parameterName, T parameterValue)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            command.Parameters.Add(parameter);
            return parameter;
        }
    }
}
