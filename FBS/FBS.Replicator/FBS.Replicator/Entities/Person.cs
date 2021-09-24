using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FBS.Replicator.Entities
{
    public static class PersonNameHasher
    {
        public static int GetDataHashCode(string surname, string name, string secondName)
        {
            return String.Concat(
                surname
                , name
                , secondName).GetHashCode();
        }
    }
}
