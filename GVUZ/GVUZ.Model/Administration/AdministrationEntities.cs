using GVUZ.Model.Helpers;

namespace GVUZ.Model.Administration
{
    public partial class AdministrationEntities : IPersonalDataAccessLogger
    {
        public IPersonalDataAccessLog CreatePersonalDataAccessLog()
        {
            var l = new PersonalDataAccessLog();
            PersonalDataAccessLog.AddObject(l);
            return l;
        }

        partial void OnContextCreated()
        {
            this.InitCommandTimeout();
        }
    }
}