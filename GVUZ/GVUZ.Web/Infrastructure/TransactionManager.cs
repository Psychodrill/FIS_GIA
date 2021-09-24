namespace GVUZ.Web.Infrastructure
{
    public static class TransactionManager
    {
        public static ITransactionManager Current
        {
            get { return SqlTransactionManager.Instance; }
        }
    }
}