using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Esrp.Utility;

namespace Esrp.Core.Common
{
		public class DbException : Exception
		{
			public DbException()
			{
				LogManager.Error("Ошибка в бд.");
			}

			///<summary>
			///</summary>		
			public DbException(string message) : base(message)
			{
				LogManager.Error(message);
			}

			///<summary>
			///</summary>
			public DbException(string message, Exception innerExc) : base(message, innerExc)
			{
				LogManager.Error(this);
			}

			///<summary>
			///</summary>
			public DbException(string formattedException, params string[] args) : 
				base(String.Format(formattedException, args))
			{
				LogManager.Error(this);
			}
		}


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
