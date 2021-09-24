using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Esrp.DB
{
    public interface IEIISIdable:IIdable
    {
        string Eiis_Id { get; set; }
    }
}
