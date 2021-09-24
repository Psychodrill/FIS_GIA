using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Esrp.SelfIntegration
{
    internal static class ESRPTables
    {
        public static class License_ISLOD
        {
            public const string TableName = "License_ISLOD";
            public const string Sys_Guid = "sys_guid";
        }

        public static class Common
        {
            public const string IdColumnName = "Id";
        }

        public static class EducationalDirectionType
        {
            public const string TableName = "EducationalDirectionType";
        }

        public static class Organization2010
        {
            public const string TableName = "Organization2010";
        }

        public static class EducationalDirectionGroup
        {
            public const string TableName = "EducationalDirectionGroup";
        }

        public static class EducationalDirection
        {
            public const string TableName = "EducationalDirection";
        }

        public static class AllowedEducationalDirection
        {
            public const string TableName = "AllowedEducationalDirection";
        }

        public static class License
        {
            public const string TableName = "License";
        }

        public static class rpt_ConnectStatV2
        {
            public const string TableName = "rpt_ConnectStatV2";
            public const string ConnectStatID = "ConnectStatID";
        }
    }
}
