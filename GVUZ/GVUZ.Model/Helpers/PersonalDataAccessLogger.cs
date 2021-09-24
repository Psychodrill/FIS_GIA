using System;
using System.Configuration;
using System.Data.Objects;
using System.Web.Script.Serialization;
using FogSoft.Helpers;

namespace GVUZ.Model.Helpers
{
    public static class PersonalDataAccessLogger
    {
        public const char MethodCreate = 'C';
        public const char MethodRetrieve = 'R';
        public const char MethodUpdate = 'U';
        public const char MethodDelete = 'D';

        public const string ObjectApplication = "Application";
        public const string ObjectPerson = "Person";
        public const string ObjectIdentityDocument = "IdentityDocument";
        public const string ObjectUserPolicy = "UserPolicy";
        public const string ObjectEntrant = "Entrant";

        private static readonly bool _enableLogging = true;
        public static bool Enabled {
            get { return _enableLogging; }
        }

        static PersonalDataAccessLogger()
        {
            _enableLogging = ConfigurationManager.AppSettings["EnablePersonalRecordsAccessLog"].To(true);
        }

        private static IPersonalDataAccessLog CreatePersonalDataAccessLog(this ObjectContext dbContext)
        {
            var loggingContext = dbContext as IPersonalDataAccessLogger;
            return loggingContext != null ? loggingContext.CreatePersonalDataAccessLog() : null;
        }

        public static void AddAccessRecord(this ObjectContext dbContext, char method, object oldData, object newData, string objectName, string accessMethod, int? objectID, int? institutionID, string userLogin = null, bool autoSave = true)
        {
            if (!_enableLogging) 
                return;
            
            var l = dbContext.CreatePersonalDataAccessLog();
            if (l == null)
            {
                LogHelper.Log.Error(string.Format("Не найден IPersonalDataAccessLogger"));
                return;
            }

            l.UserLogin = userLogin ?? UserHelper.GetAuthenticatedUserName();
            l.InstitutionID = institutionID;
            l.Method = method.ToString();
            l.AccessMethod = accessMethod;
            l.ObjectType = objectName;
            l.OldData = oldData == null ? null : Serialize(oldData);
            l.NewData = newData == null ? null : Serialize(newData);
            l.ObjectID = objectID;
            l.AccessDate = DateTime.Now;
            if (autoSave)
                dbContext.SaveChanges();

        }

        public static string Serialize(object data)
        {
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue; //8388608;
            return serializer.Serialize(data);
        }

        public static void AddToLog(this Administration.AdministrationEntities dbContext, string oldName, string newName, int? institutionID, string login)
        {
            // не логгируем
            //AddAccessRecord(dbContext,
            //	oldName == null ? MethodCreate : MethodUpdate,
            //	oldName,
            //	newName,
            //	ObjectUserPolicy,
            //	"Auth",
            //	null,
            //	institutionID,
            //	login);
        }

        public class AppData
        {
            public string ApplicationUID { get; set; }
            public string ApplicationNumber { get; set; }
            public DateTime ApplicationDate { get; set; }
            public int ApplicationID { get; set; }

            public string EntrantUID { get; set; }
            public int? EntrantDocumentID { get; set; }
            public int EntrantID { get; set; }

            [ScriptIgnore]
            public bool HasPerson { get; set; }

            public AppData()
            {
            }

            public AppData(Entrants.Entrant entrant)
            {
                if (entrant == null) return;

                EntrantDocumentID = entrant.IdentityDocumentID;
                EntrantID = entrant.EntrantID;
                EntrantUID = entrant.UID;
            }

            public AppData(Entrants.Application app)
            {
                ApplicationUID = app.UID;
                ApplicationID = app.ApplicationID;
                ApplicationNumber = app.ApplicationNumber;
                ApplicationDate = app.RegistrationDate;
                if (app.Entrant != null)
                {
                    EntrantDocumentID = app.Entrant.IdentityDocumentID;
                    EntrantID = app.Entrant.EntrantID;
                    EntrantUID = app.Entrant.UID;
                }
            }

            //public AppData(Import.Application app)
            //{
            //    ApplicationUID = app.UID;
            //    ApplicationID = app.ApplicationID;
            //    ApplicationNumber = app.ApplicationNumber;
            //    ApplicationDate = app.RegistrationDate;
            //    if (app.Entrant != null)
            //    {
            //        EntrantDocumentID = app.Entrant.IdentityDocumentID;
            //        EntrantID = app.Entrant.EntrantID;
            //        EntrantUID = app.Entrant.UID;
            //    }
            //}
        }

        public static void AddApplicationAccessToLog(this Entrants.EntrantsEntities dbContext, Entrants.Application app, string accessMethod)
        {
            AddAccessRecord(dbContext,
                MethodRetrieve,
                new AppData(app),
                null,
                ObjectApplication,
                accessMethod,
                app.ApplicationID,
                app.InstitutionID);
        }

        public static void AddApplicationAccessToLog(this Entrants.EntrantsEntities dbContext, AppData[] appData, string accessMethod, int institutionID, int? applicationID = null)
        {
            AddAccessRecord(dbContext,
                MethodRetrieve,
                appData,
                null,
                ObjectApplication,
                accessMethod,
                applicationID,
                institutionID);
        }

        //public static void AddApplicationAccessToLog(this Import.ImportEntities dbContext, AppData[] appData, string accessMethod, int institutionID, string userLogin, int? applicationID = null)
        //{
        //    AddAccessRecord(dbContext,
        //        MethodRetrieve,
        //        appData,
        //        null,
        //        ObjectApplication,
        //        accessMethod,
        //        applicationID,
        //        institutionID,
        //        userLogin);
        //}

        //public static void AddApplicationAccessToLogDelete(this Import.ImportEntities dbContext, AppData appData, string accessMethod, int institutionID, string userLogin, bool autoSave = false)
        //{
        //    AddAccessRecord(dbContext,
        //        MethodDelete,
        //        appData,
        //        null,
        //        ObjectApplication,
        //        accessMethod,
        //        null,
        //        institutionID,
        //        userLogin,
        //        autoSave: autoSave);
        //}

        public static void AddApplicationAccessToLog(this Entrants.EntrantsEntities dbContext, AppData oldData, AppData newData, string accessMethod, int institutionID, int? applicationID = null, bool autoSave = true)
        {
            AddAccessRecord(dbContext,
                oldData == null ? MethodCreate : MethodUpdate,
                oldData,
                newData,
                ObjectApplication,
                accessMethod,
                applicationID,
                institutionID,
                autoSave: autoSave);
        }

        public static void AddEntrantAccessToLog(this Entrants.EntrantsEntities dbContext, Entrants.Entrant entrant, string accessMethod)
        {
            AddAccessRecord(dbContext,
                MethodRetrieve,
                new AppData(entrant),
                null,
                ObjectEntrant,
                accessMethod,
                entrant.EntrantID,
                entrant.InstitutionID);
        }

        public static void AddEntrantAccessToLog(this Entrants.EntrantsEntities dbContext, AppData[] entrantData, string accessMethod, int institutionID, int? entrantID = null)
        {
            AddAccessRecord(dbContext,
                MethodRetrieve,
                entrantData,
                null,
                ObjectEntrant,
                accessMethod,
                entrantID,
                institutionID);
        }

        public static AppData PrepareEntrantOldData(this Entrants.EntrantsEntities dbContext, Entrants.Entrant entrant)
        {
            return new AppData(entrant);
        }

        public static void AddEntrantAccessToLog(this Entrants.EntrantsEntities dbContext, AppData oldData, Entrants.Entrant entrant, string accessMethod, bool autoSave = true)
        {
            AppData oldPersonData = oldData;
            bool hasPerson = (oldPersonData != null && oldPersonData.HasPerson) || (oldPersonData == null && oldData != null);
            AddAccessRecord(dbContext,
                hasPerson ? MethodUpdate : MethodCreate,
                oldData,
                new AppData(entrant),
                ObjectEntrant,
                accessMethod,
                entrant.EntrantID,
                entrant.InstitutionID,
                autoSave: autoSave);
        }

        public static void AddDocumentAccessToLog(this Entrants.EntrantsEntities dbContext, Entrants.Entrant oldData, Entrants.Entrant newData, string accessMethod, int entrantID, int? institutionID, bool autoSave = true)
        {
            AddAccessRecord(dbContext,
                oldData != null ? (newData != null ? MethodUpdate : MethodDelete) : MethodCreate,
                new AppData(oldData),
                new AppData(newData),
                ObjectIdentityDocument,
                accessMethod,
                entrantID,
                institutionID,
                autoSave: autoSave);
        }

        public static void AddDocumentAccessToLog(this Entrants.EntrantsEntities dbContext, Entrants.Entrant entrant, string accessMethod, int entrantID, int? institutionID, bool autoSave = true)
        {
            AddAccessRecord(dbContext,
                MethodRetrieve,
                new AppData(entrant),
                null,
                ObjectIdentityDocument,
                accessMethod,
                entrantID,
                institutionID,
                autoSave: autoSave);
        }
    }
}
