using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace GVUZ.Web.ViewModels
{
	public class AllowedDirectionAddViewModel
	{
		public IEnumerable EducationLevels { get; set; }
		public IEnumerable ParentDirections { get; set; }

		[DisplayName("Уровень образования")]
		public int EducationLevelID { get; set; }
		
		[DisplayName("Укрупнённая группа специальностей")]
		public int ParentDirectionID { get; set; }

		public int[] DirectionIDs { get; set; }
	}

    /*public class RequestModel
    {
        public Array AddDirection { get; set; }
        public Array DeleteDirection { get; set; }
    }*/
}