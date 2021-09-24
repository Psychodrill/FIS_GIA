using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Esrp.DB
{
    public interface IEntityWithNaturalKey : IIdable
    {
        string NaturalKey { get; }
    }
}
