using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GVUZ.Model.Institutions;

namespace GVUZ.Web.Models
{
	public class BaseInstitutionModel
	{
		public static IEnumerable GetInsitutionlist()
		{
			using(InstitutionsEntities dbContext = new InstitutionsEntities())
			{
				return dbContext.Institution.OrderBy(x => x.FullName).Select(x => new {ID = x.InstitutionID, Name = x.FullName}).ToArray();
			}
		}
	}
}