using System;
using System.Data;
using System.Data.Objects;
using System.Linq;
using GVUZ.Model.Helpers;
using Rdms.Communication.Exceptions;

namespace GVUZ.Model.Entrants
{
    public partial class EntrantsEntities : IPersonalDataAccessLogger
	{
		partial void OnContextCreated()
		{
			this.InitCommandTimeout();
		}
	
		private readonly EntitySimpleCache<Application> _appCache = new EntitySimpleCache<Application>();

		public override int SaveChanges(SaveOptions options)
		{
			_appCache.Clear();
			return base.SaveChanges(options);
		}

        public Application GetApplication(int applicationID)
		{
			var application = _appCache.Get(applicationID, a => Application.SingleOrDefault(x => x.ApplicationID == a));
            if (application == null)
                throw new UICustomException("Заявление не найдено! Заявление было удалено другим пользователем или сервисом из БД");

            return application;
		}

		public IQueryable<Application> ApplicationWhere(int applicationID)
		{
			return Application.Where(x => x.ApplicationID == applicationID);
		}

        public IPersonalDataAccessLog CreatePersonalDataAccessLog()
        {
            var l = new PersonalDataAccessLog();
            PersonalDataAccessLog.AddObject(l);
            return l;
        }

	}
}
