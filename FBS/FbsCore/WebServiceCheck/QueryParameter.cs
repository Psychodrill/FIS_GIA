using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fbs.Core.WebServiceCheck
{
    public class QueryParameter
    {
        public QueryParameter(string parameterName, string nodeName)
        {
            ParameterName = parameterName;
            NodeName = nodeName;
        }

        public string ParameterName { get; private set; }
        public string NodeName { get; private set; }
        public bool NodeExists { get; set; }

        public bool ValueExists
        {
            get
            {
                if (!NodeExists)
                    return false;
                if (String.IsNullOrEmpty(Value))
                    return false;
                return true;
            }
        }

        public string Value { get; set; }
    }
}
