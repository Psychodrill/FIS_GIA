using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GVUZ.Model.Entrants
{
    public partial class ApplicationStatusType
    {
        public const int Draft = 1;
        public const int New = 2;
        public const int Failed = 3;
        public const int Accepted = 4;
        public const int Removed = 5;
        public const int Denied = 6;
        public const int InOrder = 8;

        public static bool IsEditable(int statusID)
        {
            return statusID == Draft || statusID == Denied || statusID == Failed || statusID == Accepted ||
                   statusID == Removed;
        }
    }
}