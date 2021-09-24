using System;
using System.Data.Entity;
using System.Data.SqlClient;
using FogSoft.Helpers;
using GVUZ.Model.Entrants;
using System.Linq;
using GVUZ.ServiceModel.Import.Package;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import
{
	public partial class ImportPackage
	{
		public void ChangeStatus(PackageStatus packageStatus, ImportEntities dbContext)
		{
			StatusID = (int) packageStatus;
			dbContext.SaveChanges();
		}
		public void ChangeCheckStatus(PackageStatus packageStatus, ImportEntities dbContext)
		{
			CheckStatusID = (int)packageStatus;
			dbContext.SaveChanges();
		}
	}

	public partial class CompetitiveGroup
	{
		public CompetitiveGroupItem CreateCompetitiveGroupItem(
			ImportEntities dbContext, int directionID, int competitiveGroupID,
			int budgetO, int budgetOZ, int budgetZ, int paidO, int paidOZ, int paidZ)
		{
			CompetitiveGroupItem cgItem = dbContext.CompetitiveGroupItem.CreateObject();
			cgItem.DirectionID = directionID;
			cgItem.CompetitiveGroupID = competitiveGroupID;
			cgItem.NumberBudgetO = budgetO;
			cgItem.NumberBudgetOZ = budgetOZ;
			cgItem.NumberBudgetZ = budgetZ;
			cgItem.NumberPaidO = paidO;
			cgItem.NumberPaidOZ = paidOZ;
			cgItem.NumberPaidZ = paidZ;
			CompetitiveGroupItem.Add(cgItem);

			return cgItem;
		}

		private static Address GenerateAddress()
		{
			return new Address
			{
				Building = "3",
				BuildingPart = "34",
				CityName = "Саратов",
				CountryID = 1
			};
		}

		//public Application CreateApplication(CompetitiveGroupItem cgItem)
		//{
		//    Application app = new Application();
		//    app.Entrant = new Entrant()
		//    {
		//        AddressFact = GenerateAddress(),
		//        AddressReg = GenerateAddress(),
		//        Email = "dfads@mail.ru",
		//        Person = GeneratePerson()
		//    };
		//    app.RegistrationDate = DateTime.Now;
		//    app.StatusID = ApplicationStatusType.Accepted;
		//    app.NeedHostel = true;
		//    app.ApplicationNumber = "431324";
		//    app.CompetitiveGroup = this;
		//    app.CompetitiveGroupItem = cgItem;
		//    app.InstitutionID = InstitutionID;
		//    app.IsRequiresBudgetO = true;
		//    return app;
		//}
	}

	public static class ConvertExtensions  // TODO Make this generic
	{
		public static bool ParseBool(this string boolStr)
		{
			bool result;
			if(!Boolean.TryParse(boolStr, out result))
			{
				result = boolStr == "1" || boolStr == "-1";
			}

			return result;
		}

     public static short ParseShort(this string shortStr)
     {
            short result = 0;
            if (!short.TryParse(shortStr,out result))
            {
                result = 0;
            }
            return result;

     }

        public static Int32 ParseInt32(this string int32String)
        {
            Int32 result = 0;
            if (!Int32.TryParse(int32String, out result))
            {
                result = 0;
            }
            return result; 
        }


        public static Decimal ParseDecimal(this string decimalString)
        {
            Decimal result = 0;
            if (!Decimal.TryParse(decimalString, out result))
            {
                result = 0;
            }
            return result;
        }




	}

	public static class DateTimeExtensions
	{
		public static string GetCurrentDateAsString()
		{
			return DateTime.Now.GetDateAsString();
		}

		public static string GetDateAsString(this DateTime date)
		{
			return date.ToString("yyyy-MM-dd");
		}

		public static string GetDateTimeAsString(this DateTime date)
		{
			return date.ToString("yyyy-MM-ddTHH:mm:ss");
		}

		public static DateTime GetStringAsDate(this string date)
		{
			DateTime? dateTime = GetStringOrEmptyAsDate(date);
			//TODO: предмет обсуждения на миллисекунды
			if (dateTime.HasValue) return dateTime.Value.AddMilliseconds(-dateTime.Value.Millisecond);

			LogHelper.Log.ErrorFormat("Ошибка при конвертации строки \"{0}\" в дату.", date);
			return DateTime.MinValue;
		}

		public static DateTime? GetStringOrEmptyAsDate(this string dateStr)
		{
			if (string.IsNullOrEmpty(dateStr)) return null;

			DateTime resultDate;
			if(DateTime.TryParse(dateStr, out resultDate))
				return resultDate;

			LogHelper.Log.ErrorFormat("Ошибка при конвертации строки \"{0}\" в дату.", dateStr);
			return null;
		}

		public static string GetNullableDateAsString(this DateTime? date)
		{
			if (date.HasValue) return date.Value.GetDateAsString();
			return null;
		}
	}

    public static class ApplicationExtensions
    {
        //public static string GetRegistrationDateTimeAsString(this Application application)
        //{
        //	return application.RegistrationDate.GetDateTimeAsString();
        //}

        public static ApplicationShortRef GetApplicationShortRef(this Application application)
        {
            return new ApplicationShortRef()
            {
                ApplicationNumber = application.ApplicationNumber,
                RegistrationDateDate = application.RegistrationDate
            };
        }
    }
}
