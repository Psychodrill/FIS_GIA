using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Esrp.SelfIntegration
{
    internal static class FISTables
    {
        public static class Common
        {
            public const string EiisIdColumnName = "EIIS_ID";
        }

        public static class EDU_PROGRAM_TYPES
        {
            public const string TableName = "EDU_PROGRAM_TYPES";
        }

        public static class Institution
        {
            public const string TableName = "Institution";
            public const string EsrpOrgID = "EsrpOrgID";
        }

        public static class ParentDirection
        {
            public const string TableName = "ParentDirection";
        }

        public static class Direction
        {
            public const string TableName = "Direction";
        }

        public static class AllowedDirections
        {
            public const string TableName = "AllowedDirections";
        }

        public static class InstitutionLicense
        {
            public const string TableName = "InstitutionLicense";
        }
    }
}
