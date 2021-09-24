using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;

namespace Esrp.SelfIntegration.ReplicationServer
{
    public class ESRPServer
    {
        private string connectionString_;
        public ESRPServer(string connectionString)
        {
            connectionString_ = connectionString;
        }

        public void RunReplication(XmlElement batch)
        {
            lock (ServerLocker.Locker)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString_))
                    {
                        connection.Open();
                        foreach (XmlNode node in batch.ChildNodes)
                        {
                            string tableName = node.Name;
                            Schema schema = new Schema(connection, tableName);

                            string idColumn = Hardcoded.GetESRPIdColumnName(tableName);
                            long uniqueId = Int64.Parse(node.Attributes["id"].Value);
                            string recordId = node.Attributes["recordId"].Value;
                            char commandType = node.Attributes["command"].Value[0];

                            SqlCommand command = connection.CreateCommand();
                            if (commandType == CommandTypes.Delete)
                            {
                                command.CommandText = String.Format("DELETE FROM [{0}] WHERE [{1}] = @id", tableName, idColumn);
                                command.Parameters.AddWithValue("id", recordId);
                            }
                            else
                            {
                                StringBuilder sql = new StringBuilder();
                                if (commandType == CommandTypes.Ensure)
                                {
                                    command.CommandText = String.Format("SELECT TOP 1 [{1}] FROM [{0}] WHERE [{1}] = @id", tableName, idColumn);
                                    command.Parameters.AddWithValue("id", recordId);
                                    using (SqlDataReader existsReader = command.ExecuteReader())
                                    {
                                        if (existsReader.Read())
                                        {
                                            commandType = CommandTypes.Update;
                                        }
                                        else
                                        {
                                            commandType = CommandTypes.Insert;
                                        }
                                    }
                                    command.Parameters.Clear();
                                }

                                if (commandType == CommandTypes.Insert)
                                {
                                    if (schema.HasIdentity)
                                    {
                                        sql.AppendFormat("SET IDENTITY_INSERT [{0}] ON", tableName).AppendLine();
                                    }
                                    sql.AppendFormat("INSERT INTO [{0}] (", tableName).AppendLine();

                                    sql.AppendFormat("[{0}]", idColumn).AppendLine();
                                    foreach (XmlAttribute dataAttribute in node.SelectSingleNode("Data").Attributes)
                                    {
                                        sql.AppendFormat(",[{0}]", dataAttribute.Name).AppendLine();
                                    }
                                    sql.Append(") VALUES (").AppendLine();

                                    sql.AppendFormat("@{0}", idColumn).AppendLine();
                                    command.Parameters.AddWithValue(idColumn, recordId);
                                    foreach (XmlAttribute dataAttribute in node.SelectSingleNode("Data").Attributes)
                                    {
                                        sql.AppendFormat(",@{0}", dataAttribute.Name).AppendLine();
                                        SqlDbType? sqlType;
                                        object value = GetTypedValue(dataAttribute.Value, dataAttribute.Name, schema.GetColumn(dataAttribute.Name).SqlDataType, out sqlType);
                                        if (sqlType.HasValue)
                                        {
                                            command.Parameters.Add(dataAttribute.Name, sqlType.Value).Value = value;
                                        }
                                        else
                                        {
                                            command.Parameters.AddWithValue(dataAttribute.Name, value);
                                        }
                                    }
                                    sql.Append(")").AppendLine();
                                    if (schema.HasIdentity)
                                    {
                                        sql.AppendFormat("SET IDENTITY_INSERT [{0}] OFF", tableName);
                                    }
                                }
                                else if (commandType == CommandTypes.Update)
                                {
                                    sql.AppendFormat("UPDATE [{0}] SET", tableName).AppendLine();
                                    bool first = true;
                                    foreach (XmlAttribute dataAttribute in node.SelectSingleNode("Data").Attributes)
                                    {
                                        if (first)
                                        {
                                            first = false;
                                        }
                                        else
                                        {
                                            sql.Append(",");
                                        }
                                        sql.AppendFormat("[{0}] = @{0}", dataAttribute.Name).AppendLine();
                                        SqlDbType? sqlType;
                                        object value = GetTypedValue(dataAttribute.Value, dataAttribute.Name, schema.GetColumn(dataAttribute.Name).SqlDataType, out sqlType);
                                        if (sqlType.HasValue)
                                        {
                                            command.Parameters.Add(dataAttribute.Name, sqlType.Value).Value = value;
                                        }
                                        else
                                        {
                                            command.Parameters.AddWithValue(dataAttribute.Name, value);
                                        }
                                    }
                                    sql.AppendFormat("WHERE [{0}] = @id", idColumn).AppendLine();
                                    command.Parameters.AddWithValue("id", recordId);
                                }
                                command.CommandText = sql.ToString();
                            }
                            try
                            {
                                command.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                failedIds_.Add(uniqueId,String.Format("{0} ({1})", ex.Message,ex.StackTrace));
                            }
                        }
                    }
                    if (failedIds_.Any())
                    {
                        throw new Exception(String.Format("При обработке некоторых записей возникли ошибки (всего ошибок: {0}; примеры ошибок: {1})", failedIds_.Count,String.Join(":",failedIds_.Values.Take(5).ToArray())));
                    }

                    Success = true;
                }
                catch (Exception ex)
                {
                    Success = false;
                    Exception = ex;
                }
            }
        }

        private object GetTypedValue(string valueStr, string column, string sqlDataTypeName, out SqlDbType? sqlDataType)
        {
            switch (sqlDataTypeName)
            {
                case OtherConstants.SqlDataTypeNames.Image:
                    sqlDataType = SqlDbType.Image;
                    if (valueStr == OtherConstants.Null)
                        return DBNull.Value;
                    else
                        return Convert.FromBase64String(valueStr);
                case OtherConstants.SqlDataTypeNames.VarBinary:
                    sqlDataType = SqlDbType.VarBinary;
                    if (valueStr == OtherConstants.Null)
                        return DBNull.Value;
                    else
                        return Convert.FromBase64String(valueStr);
                case OtherConstants.SqlDataTypeNames.DateTime:
                    sqlDataType = SqlDbType.DateTime;
                    if (valueStr == OtherConstants.Null)
                        return DBNull.Value;
                    else
                        return DateTime.Parse(valueStr, CultureInfo.InvariantCulture);
                default:
                    sqlDataType = null;
                    if (valueStr == OtherConstants.Null)
                        return DBNull.Value;
                    else
                        return valueStr;
            }
        }

        public bool Success { get; private set; }
        public Exception Exception { get; private set; }

        private Dictionary<long, string> failedIds_ = new Dictionary<long, string>();
        public long[] FailedIds { get { return failedIds_.Keys.ToArray(); } }
    }
}
