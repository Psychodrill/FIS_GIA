using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GVUZ.Web.Models
{
	public enum ApplicationStepType
	{
		NotApplication = 0,
		Begin = 1,
		PersonalData = 2,
		Address = 100,//Невалиден, оставлен для совместимости
		ParentData = 101,//Невалиден, оставлен для совместимости
		EntranceTest = 3,
		Languages = 102,//Невалиден, оставлен для совместимости
		Documents = 4,
        IndividualAchivements = 5,
		//AdditionalInfo = 8,
		Sending = 6
	}
}