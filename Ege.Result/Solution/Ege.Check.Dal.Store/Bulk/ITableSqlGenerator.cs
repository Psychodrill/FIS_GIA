namespace Ege.Check.Dal.Store.Bulk
{
    using System.Data;

    public interface ITableSqlGenerator
    {
        string CreateSql(DataTable dt, string tableName);
        string DropSql(string tableName);
    }
}