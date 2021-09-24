using GVUZ.Helper;

namespace GVUZ.Model.Entrants
{
    public partial class Application : IObjectWithUID
    {}

    public partial class Campaign : IObjectWithUID
    {}

    public partial class ApplicationEntranceTestDocument : IObjectWithUID, IApplicationReferenced
    {}

    public partial class ApplicationEntrantDocument : IObjectWithUID, IApplicationReferenced
    {}
}
