using GVUZ.Model.Helpers;

namespace GVUZ.ServiceModel.Import
{
    public static class PDImportLoggerExtension
    {

        public static PersonalDataAccessLogger.AppData CreateAppData(this ImportEntities dbContext, Application app)
        {
            PersonalDataAccessLogger.AppData appdata = new PersonalDataAccessLogger.AppData();

            appdata.ApplicationUID = app.UID;
            appdata.ApplicationID = app.ApplicationID;
            appdata.ApplicationNumber = app.ApplicationNumber;
            appdata.ApplicationDate = app.RegistrationDate;
            if (app.Entrant != null)
            {
                appdata.EntrantDocumentID = app.Entrant.IdentityDocumentID;
                appdata.EntrantID = app.Entrant.EntrantID;
                appdata.EntrantUID = app.Entrant.UID;
            }
            return appdata;
        }

        public static void AddApplicationAccessToLog(this ImportEntities dbContext, PersonalDataAccessLogger.AppData[] appData, string accessMethod, int institutionID, string userLogin, int? applicationID = null)
        {
            PersonalDataAccessLogger.AddAccessRecord(dbContext,
                PersonalDataAccessLogger.MethodRetrieve,
                appData,
                null,
                PersonalDataAccessLogger.ObjectApplication,
                accessMethod,
                applicationID,
                institutionID,
                userLogin);
        }

        public static void AddApplicationAccessToLogDelete(this ImportEntities dbContext, PersonalDataAccessLogger.AppData appData, string accessMethod, int institutionID, string userLogin, bool autoSave = false)
        {
            PersonalDataAccessLogger.AddAccessRecord(dbContext,
                PersonalDataAccessLogger.MethodDelete,
                appData,
                null,
                PersonalDataAccessLogger.ObjectApplication,
                accessMethod,
                null,
                institutionID,
                userLogin,
                autoSave: autoSave);
        }
    }
}
