using System.Linq;
using GVUZ.Model.Cache;
using GVUZ.Model.Helpers;

namespace GVUZ.Model.Entrants
{
	public class EntrantKey {
		private readonly int _entrantID;
		private readonly int _applicationID;
		private readonly UserInfo _userInfo;

		public UserInfo UserInfo {
			get { return _userInfo; }
		}

		public int ApplicationID { get { return _applicationID; } }

		public EntrantKey(int entrantID, int applicationID, UserInfo userInfo) {
			_entrantID = entrantID;
			_applicationID = applicationID;
			_userInfo = userInfo;
		}

		public EntrantKey(int applicationID, UserInfo userInfo)
		{
			_applicationID = applicationID;
			_userInfo = userInfo;
		}

		public EntrantKey(UserInfo userInfo)
		{
			_userInfo = userInfo;
		}

        public Entrant GetEntrant(EntrantsEntities dbContext, bool isForSave)
        {
            return GetEntrant(dbContext, _userInfo, _applicationID, _entrantID, isForSave);
        }

        private static Entrant GetEntrant(EntrantsEntities dbContext, UserInfo userInfo, int applicationID, int entrantID, bool isForSave)
        {
            //if (!isForSave)
            {
                if (entrantID > 0) //уже есть данные
                {
                  //  var entrant = EntrantCacheManager.GetFirst<Entrant>(x => x.EntrantID == entrantID);
//                    if (entrant != null) return entrant;

                   var entrant = dbContext.Entrant.FirstOrDefault(x => x.EntrantID == entrantID);
                    if (entrant != null)
                        EntrantCacheManager.Add(entrantID, entrant);

                    return entrant;
                }
                if (applicationID > 0) //application
                {
                   // var application = EntrantCacheManager.GetFirst<Application>(x => x.ApplicationID == applicationID && (userInfo.SNILS == null || x.Entrant.SNILS == userInfo.SNILS));
                    //if (application == null)
                    {
                        var application = dbContext.Application.FirstOrDefault(x => x.ApplicationID == applicationID);
                        if (application == null)
                            return null;
                        
                        EntrantCacheManager.Add(applicationID, application);
                    }

                    //var entrant = EntrantCacheManager.GetFirst<Entrant>(x => x.EntrantID == application.EntrantID);
                    //if (entrant != null) return entrant;

                    var entrant = dbContext.Application
                        .Where(x => x.ApplicationID == applicationID && (userInfo.SNILS == null || x.Entrant.SNILS == userInfo.SNILS))
                        .Select(x => x.Entrant).FirstOrDefault();

                    if (entrant != null)
                        EntrantCacheManager.Add(entrant.EntrantID, entrant);

                    return entrant;
                }
                if (userInfo.SNILS != null)
                    return dbContext.Entrant.FirstOrDefault(x => x.SNILS == userInfo.SNILS);
            }
            if (isForSave)
            {
                if (applicationID > 0) //application
                {
                    int cnt = dbContext.Application
                        .Count(x => x.ApplicationID == applicationID
                            && x.ApplicationStatusType.StatusID == ApplicationStatusType.Draft
                            && x.ApplicationStatusType.StatusID == ApplicationStatusType.New
                            && x.ApplicationStatusType.StatusID == ApplicationStatusType.Failed
                            && x.ApplicationStatusType.StatusID == ApplicationStatusType.Accepted
                            && x.ApplicationStatusType.StatusID == ApplicationStatusType.Removed
                            );
                    if (cnt == 0) //нельзя редактировать в этих статусах
                        return null;
                }
            }

            return null;
        }

		public int GetEntrantID(EntrantsEntities dbContext, bool isForSave)
		{
			return GetEntrantID(dbContext, _userInfo, _applicationID, _entrantID, isForSave);
		}

		private static int GetEntrantID(EntrantsEntities dbContext, UserInfo userInfo, int applicationID, int entrantID, bool isForSave)
		{
			//if (!isForSave)
			{
                if (entrantID > 0) //уже есть данные
                {
                    var entrant = EntrantCacheManager.GetFirst<Entrant>(x => x.EntrantID == entrantID);
                    if (entrant != null) return entrant.EntrantID;

                    return  dbContext.Entrant
                        .Where(x => x.EntrantID == entrantID && (userInfo.SNILS == null || x.SNILS == userInfo.SNILS))
                        .Select(x => x.EntrantID).FirstOrDefault();
                }
			    if (applicationID > 0) //application
				{
                    var application = EntrantCacheManager.GetFirst<Application>(x => x.ApplicationID == applicationID && (userInfo.SNILS == null || x.Entrant.SNILS == userInfo.SNILS));

                    if (application != null) return application.EntrantID;

					return dbContext.Application
						.Where(x => x.ApplicationID == applicationID && (userInfo.SNILS == null || x.Entrant.SNILS == userInfo.SNILS))
						.Select(x => x.EntrantID).FirstOrDefault();
				}
				if (userInfo.SNILS != null)
					return dbContext.Entrant.Where(x => x.SNILS == userInfo.SNILS).Select(x => x.EntrantID).FirstOrDefault();
			}
			if (isForSave)
			{
				if (applicationID > 0) //application
				{
					int cnt = dbContext.Application
                        .Count(x => x.ApplicationID == applicationID
							&& x.ApplicationStatusType.StatusID == ApplicationStatusType.Draft
							&& x.ApplicationStatusType.StatusID == ApplicationStatusType.New
							&& x.ApplicationStatusType.StatusID == ApplicationStatusType.Failed
							&& x.ApplicationStatusType.StatusID == ApplicationStatusType.Accepted
							&& x.ApplicationStatusType.StatusID == ApplicationStatusType.Removed
							);
					if (cnt == 0) //нельзя редактировать в этих статусах
						return 0;
				}
			}

			return 0;
		}
	}
}