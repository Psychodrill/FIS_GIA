using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GVUZ.DAL.Tests.TestHelpers.L2SGVUZ;

using GVUZEntrant = GVUZ.DAL.Tests.TestHelpers.L2SGVUZ.Entrant;
using GVUZOlympicDiplomant = GVUZ.DAL.Tests.TestHelpers.L2SGVUZ.OlympicDiplomant;

namespace GVUZ.DAL.Tests.TestHelpers
{
    internal static class GVUZDataHelper
    {
        public static void CreateEntrantWithDocument(string lastName, string firstName, string patronymic, string documentNumber, string documentSeries, out int entrantId)
        {
            using (GVUZDataContext gvuzDB = new GVUZDataContext(Config.GVUZConnectionString))
            {
                GVUZEntrant entrant = new GVUZEntrant();
                entrant.LastName = lastName;
                entrant.FirstName = firstName;
                entrant.MiddleName = patronymic;
                entrant.Email = "ivan@mail.ru";
                entrant.MobilePhone = "0(000)-000-00-00";
                entrant.SNILS = "000-000-000 00";
                entrant.GenderID = Constants.GVUZMaleSexId;

                gvuzDB.Entrants.InsertOnSubmit(entrant);
                gvuzDB.SubmitChanges();

                EntrantDocument identityDocument = new EntrantDocument();
                identityDocument.EntrantID = entrant.EntrantID;
                identityDocument.DocumentNumber = documentNumber;
                identityDocument.DocumentSeries = documentSeries;
                identityDocument.DocumentDate = new DateTime(2000, 1, 1);
                identityDocument.DocumentOrganization = "Организация";
                identityDocument.DocumentTypeID = Constants.GVUZIdentityDocumentTypeId;
                identityDocument.DocumentSpecificData = String.Empty;

                gvuzDB.EntrantDocuments.InsertOnSubmit(identityDocument);
                gvuzDB.SubmitChanges();

                entrant.IdentityDocumentID = identityDocument.EntrantDocumentID;
                gvuzDB.SubmitChanges();

                entrantId = entrant.EntrantID;
            }
        } 

        public static int EnsureOlympicTypeProfile()
        {
            using (GVUZDataContext gvuzDB = new GVUZDataContext(Config.GVUZConnectionString))
            {
                OlympicType olympicType = new OlympicType();
                olympicType.Name = "Олимпиада";
                olympicType.OlympicYear = 2016;
                gvuzDB.OlympicTypes.InsertOnSubmit(olympicType);
                gvuzDB.SubmitChanges();

                OlympicTypeProfile olympicTypeProfile = new OlympicTypeProfile();
                olympicTypeProfile.OlympicTypeID = olympicType.OlympicID;
                olympicTypeProfile.OlympicProfileID = Constants.GVUZOlympicProfileAll;

                gvuzDB.OlympicTypeProfiles.InsertOnSubmit(olympicTypeProfile);
                gvuzDB.SubmitChanges();

                return olympicTypeProfile.OlympicTypeProfileID;
            }
        }

        public static void CreateOlympicDiplomantWithDocument(string lastName, string firstName, string patronymic, string documentNumber, string documentSeries, out long olympicDiplomantId)
        {
            using (GVUZDataContext gvuzDB = new GVUZDataContext(Config.GVUZConnectionString))
            {
                GVUZOlympicDiplomant olympicDiplomant = new GVUZOlympicDiplomant();
                olympicDiplomant.OlympicTypeProfileID = EnsureOlympicTypeProfile();
                olympicDiplomant.ResultLevelID = Constants.GVUZOlympicWinnerDiplomType;
                olympicDiplomant.StatusID = null;
                olympicDiplomant.CreateDate = DateTime.Now;

                gvuzDB.OlympicDiplomants.InsertOnSubmit(olympicDiplomant);
                gvuzDB.SubmitChanges();

                OlympicDiplomantDocument identityDocument = new OlympicDiplomantDocument();
                identityDocument.OlympicDiplomantID = olympicDiplomant.OlympicDiplomantID;
                identityDocument.LastName = lastName;
                identityDocument.FirstName = firstName;
                identityDocument.MiddleName = patronymic;
                identityDocument.DocumentNumber = documentNumber;
                identityDocument.DocumentSeries = documentSeries;
                identityDocument.DateIssue = new DateTime(2000, 1, 1);
                identityDocument.OrganizationIssue = "Организация";
                identityDocument.IdentityDocumentTypeID = Constants.GVUZIdentityDocumentTypeId;

                gvuzDB.OlympicDiplomantDocuments.InsertOnSubmit(identityDocument);
                gvuzDB.SubmitChanges();

                olympicDiplomant.OlympicDiplomantIdentityDocumentID = identityDocument.OlympicDiplomantDocumentID;
                gvuzDB.SubmitChanges();

                olympicDiplomantId = olympicDiplomant.OlympicDiplomantID;
            }
        }

        public static void CreateRVIPersonWithDocument(int personId, string lastName, string firstName, string patronymic, string documentNumber, string documentSeries)
        {
            using (GVUZDataContext gvuzDB = new GVUZDataContext(Config.GVUZConnectionString))
            {
                RVIPerson rviPerson = new RVIPerson();
                rviPerson.PersonId = personId;
                rviPerson.NormSurname = StringsHelper.NormalizeNamePart(lastName);
                rviPerson.NormName = StringsHelper.NormalizeNamePart(firstName);
                rviPerson.NormSecondName = StringsHelper.NormalizeNamePart(patronymic);
                rviPerson.Email = "ivan@mail.ru";
                rviPerson.MobilePhone = "0(000)-000-00-00";
                rviPerson.SNILS = "000-000-000 00";
                rviPerson.Sex = Constants.RVIMaleSex;
                rviPerson.UpdateDate = DateTime.Now;
                rviPerson.CreateDate = DateTime.Now;
                rviPerson.IntegralUpdateDate = DateTime.Now;

                gvuzDB.RVIPersons.InsertOnSubmit(rviPerson);
                gvuzDB.SubmitChanges();

                RVIPersonIdentDoc rviDocument = new RVIPersonIdentDoc();
                rviDocument.PersonIdentDocID = personId * 1000;
                rviDocument.PersonId = rviPerson.PersonId;
                rviDocument.DocumentNumber = documentNumber;
                rviDocument.DocumentSeries = documentSeries;
                rviDocument.DocumentDate = new DateTime(2000, 1, 1);
                rviDocument.DocumentOrganization = "Организация";
                rviDocument.DocumentTypeCode = Constants.RVIPassportDocumentTypeCode;
                rviDocument.UpdateDate = DateTime.Now;
                rviDocument.CreateDate = DateTime.Now;

                gvuzDB.RVIPersonIdentDocs.InsertOnSubmit(rviDocument);
                gvuzDB.SubmitChanges();
            }
        }

        public static void ClearRVIPersons()
        {
            using (GVUZDataContext gvuzDB = new GVUZDataContext(Config.GVUZConnectionString))
            {
                gvuzDB.ExecuteCommand("DELETE FROM RVIPersonIdentDocs");
                gvuzDB.ExecuteCommand("DELETE FROM RVIPersons");
            }
        }

        public static bool GetRVIIsEmpty()
        {
            using (GVUZDataContext gvuzDB = new GVUZDataContext(Config.GVUZConnectionString))
            {
                if ((gvuzDB.RVIPersons.Any()) || (gvuzDB.RVIPersonIdentDocs.Any()))
                    return false   ;
                return true;
            }
        }

    }
}
