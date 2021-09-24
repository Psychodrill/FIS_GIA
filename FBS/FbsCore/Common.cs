using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fbs.Core.Common
{
   public  sealed class ParameterDefinition
    {
       public string Name;
       public object Value;
       public ComparsionType Comparsion;

       private  ParameterDefinition()
       {
       }

       public ParameterDefinition(string name, object value, ComparsionType comparsion)
       {
           Name = name;
           Value = value;
           Comparsion = comparsion;
       }
    }

    public enum ComparsionType
    {
        Equals,Like
    }
}
