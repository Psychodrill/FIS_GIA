using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using FogSoft.Helpers;

namespace GVUZ.Model.Entrants
{
	public class GenderType
	{
        [Description("Мужской")]
		public static readonly int Male = 1;
        
        [Description("Женский")]
		public static readonly int Female = 2;

	    public static string GetName(int genderTypeId)
	    {
	        return typeof (GenderType).GetEnumDescription(genderTypeId);
	    }
	}
}
