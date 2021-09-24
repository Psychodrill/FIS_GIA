using System;

namespace FBS.Replicator.DB
{
    public enum Tables { A, B }

    public static class TablesHelper
    {
        public static Tables Reverse(Tables tables)
        {
            if (tables == Tables.A)
                return Tables.B;
            return Tables.A;
        }

        public static string ToString(Tables? tables)
        {
            if (!tables.HasValue)
                return String.Empty;
            if (tables == Tables.A)
                return "A";
            return "B";
        }
    }
}
