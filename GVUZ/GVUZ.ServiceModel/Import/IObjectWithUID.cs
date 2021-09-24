using GVUZ.Helper;
using GVUZ.Model;

namespace GVUZ.ServiceModel.Import
{
	public partial class AdmissionVolume : IObjectWithUID
	{
	}

	public partial class Campaign : IObjectWithUID
	{
	}

	public partial class CompetitiveGroup : IObjectWithUID
	{
	}

	public partial class CompetitiveGroupItem : IObjectWithUID
	{
	}

	public partial class CompetitiveGroupTarget : IObjectWithUID
	{
	}

	public partial class CompetitiveGroupTargetItem : IObjectWithUID
	{
	}

	public partial class BenefitItemC : IObjectWithUID
	{
	}

	public partial class EntranceTestItemC : IObjectWithUID
	{
	}

	public partial class Application : IObjectWithUID
	{
	}

    public partial class ApplicationEntranceTestDocument : IObjectWithUID, IApplicationReferenced
	{
	}

    public class ApplicationEntranceTestDocumentShortRef : IObjectWithUID, IApplicationReferenced
    {
        public string UID { get; set; }
        public string ApplicationUID { get; set; }
        public int ApplicationID { get; set; }
    }

    //fake class for import check
	public class ApplicationEntranceTestResult : ApplicationEntranceTestDocument
	{
	}

	//fake class for import check
	public class ApplicationEntranceTestBenefit : ApplicationEntranceTestDocument
	{
	}

	public partial class EntrantDocument : IObjectWithUID
	{
	}

	public partial class OrderOfAdmission : IObjectWithUID
	{
	}	
}