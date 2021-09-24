
namespace Esrp.SelfIntegration
{
    internal static class Hardcoded
    {
        public static string GetESRPIdColumnName(string tableName)
        {
            if (tableName == ESRPTables.License_ISLOD.TableName)
                return ESRPTables.License_ISLOD.Sys_Guid;
            if(tableName == ESRPTables.rpt_ConnectStatV2.TableName)
                return ESRPTables.rpt_ConnectStatV2.ConnectStatID;
            return ESRPTables.Common.IdColumnName;
        }

        public static string GetFISIdColumnName(string tableName)
        {
            if (tableName == FISTables.Institution.TableName)
                return FISTables.Institution.EsrpOrgID;
            return FISTables.Common.EiisIdColumnName;
        }
    }
}
