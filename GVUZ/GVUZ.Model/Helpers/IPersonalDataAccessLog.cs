using System;

namespace GVUZ.Model.Helpers
{
	public interface IPersonalDataAccessLog
	{
		Int32 ID { get; set; }
		String Method { get; set; }
		String OldData { get; set; }
		String NewData { get; set; }
		String ObjectType { get; set; }
		int? InstitutionID { get; set; }
		String UserLogin { get; set; }
		String AccessMethod { get; set; }
		int? ObjectID { get; set; }
		DateTime AccessDate { get; set; }
	}

    public interface IPersonalDataAccessLogger
    {
        IPersonalDataAccessLog CreatePersonalDataAccessLog();
    }
}