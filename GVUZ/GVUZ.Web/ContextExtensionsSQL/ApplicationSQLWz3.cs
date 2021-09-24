using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using GVUZ.Web.ViewModels;
using System.Configuration;
using System.Web.Configuration;
using GVUZ.Model.Entrants.Documents;
using GVUZ.Model.Helpers;
using GVUZ.Model.Entrants;
using GVUZ.Model;
using GVUZ.DAL.Dapper.Repository.Model;

namespace GVUZ.Web.ContextExtensionsSQL
{

    public static partial class ApplicationSQL
    {

        public static ApplicationWz2ViewModel GetApplicationWz2(int ApplicationID)
        {
            #region SQL
            string sql = @"
DECLARE @EntrantID int
SELECT @EntrantID=EntrantID FROM [Application] (NOLOCK) WHERE ApplicationID=@ApplicationID
SELECT ed.EntrantDocumentID 
  ,ed.EntrantID 
   ,ed.DocumentTypeID
   ,dt.Name as DocumentTypeName
      , (COALESCE(DocumentSeries,'')+' '+COALESCE(DocumentNumber,'')) as DocumentSeriesNumber
      , DocumentDate
      , DocumentOrganization
   , ed.AttachmentID as AttachmentID, att.Name as AttachmentName, att.FileID as AttachmentFileID
   , aed.ID as ApplicationEntrantDocumentID
   , aed.OriginalReceivedDate as OriginalReceivedDate
   , (SELECT COUNT(aed.id) FROM ApplicationEntrantDocument aed (NOLOCK) WHERE aed.EntrantDocumentID=ed.EntrantDocumentID AND aed.ApplicationID=@ApplicationID) as inThisApp
   , (SELECT COUNT(aed.id) FROM ApplicationEntrantDocument aed (NOLOCK) WHERE aed.EntrantDocumentID=ed.EntrantDocumentID ) as inAppCount
   , (SELECT COUNT(aetd.id) FROM ApplicationEntranceTestDocument aetd (NOLOCK) WHERE aetd.EntrantDocumentID=ed.EntrantDocumentID ) as inTestCount
   , (SELECT COUNT(aetd.id) FROM ApplicationEntranceTestDocument aetd (NOLOCK) WHERE aetd.EntrantDocumentID=ed.EntrantDocumentID AND aed.ApplicationID=@ApplicationID) as inThisAppTestCount
   , (SELECT COUNT(e.IdentityDocumentID) FROM Entrant e (NOLOCK) WHERE e.EntrantID=@EntrantID AND e.IdentityDocumentID=ed.EntrantDocumentID) as  isIdentity
   ,ed.OlympApproved
FROM EntrantDocument ed (NOLOCK)
 INNER JOIN DocumentType dt (NOLOCK) on ed.DocumentTypeID=dt.DocumentID
 LEFT OUTER JOIN Attachment att (NOLOCK) on att.AttachmentID=ed.AttachmentID
 LEFT OUTER JOIN ApplicationEntrantDocument aed (NOLOCK) on aed.EntrantDocumentID=ed.EntrantDocumentID and aed.ApplicationID=@ApplicationID
WHERE ed.EntrantID=@EntrantID
";

            string sqlDocumentTypes = @"
SELECT DocumentID AS TypeID,
Name ,
CategoryID 
FROM DocumentType (NOLOCK)
ORDER BY NAME";
            #endregion

            ApplicationWz2ViewModel appw3 = new ApplicationWz2ViewModel();
            appw3.ApplicationID = ApplicationID;
            //appw3.EntrantID		=EntrantID;

            //appw3.DocumentTypes =
            //    DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.DocumentType)
            //    .Select(c => new ApplicationWz2ViewModel.DocumentType
            //    {
            //        TypeID = c.Key,
            //        Name = c.Value,
            //        CategoryId = c.CategoryId
            //    })
            //    .OrderBy(c => c.Ord).ThenBy(c => c.Name);

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sqlDocumentTypes, con);
                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    ViewModels.DocumentType d = null;
                    #region Read
                    while (r.Read())
                    {
                        d = new ViewModels.DocumentType();
                        d.TypeID = (int)r["TypeID"];
                        d.Name = (string)r["Name"];
                        d.CategoryId = (int)r["CategoryId"];

                        appw3.DocumentTypes.Add(d);
                    }
                    #endregion
                    r.Close();
                    con.Close();
                }
                catch (SqlException e)
                {
                    throw e;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (con.State != ConnectionState.Closed) { con.Close(); }
                }
            }   

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("ApplicationID", ApplicationID));
                //com.Parameters.Add(new SqlParameter("EntrantID", EntrantID));
                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    DocumentListViewModel d = null;
                    object o;
                    int inAppCount;
                    int inTestCount;
                    int isIdentity;
                    bool inThisApp;
                    int inThisAppTestCount;
                    #region Read
                    while (r.Read())
                    {
                        d = new DocumentListViewModel();
                        d.EntrantID = (int)r["EntrantID"];
                        d.EntrantDocumentID = (int)r["EntrantDocumentID"];
                        d.DocumentTypeID = (int)r["DocumentTypeID"];
                        d.DocumentTypeName = r["DocumentTypeName"] as string;
                        d.DocumentSeriesNumber = r["DocumentSeriesNumber"] as string;
                         
                        if (!r.IsDBNull(r.GetOrdinal("DocumentDate")))
                        {
                            if (d.DocumentTypeID == 2)
                            {
                                d.DocumentDate = ((DateTime)r["DocumentDate"]).ToString("yyyy");
                            }
                            else
                            {
                                d.DocumentDate = ((DateTime)r["DocumentDate"]).ToString("dd.MM.yyyy");
                            }
                        }
                        d.DocumentOrganization = r["DocumentOrganization"] as string;
                        d.AttachmentID = r["AttachmentID"] as int?;
                        d.AttachmentName = r["AttachmentName"] as string;
                        d.AttachmentFileID = r["AttachmentFileID"] as Guid?;
                        o = r["OriginalReceivedDate"];
                        if (o != null && o != DBNull.Value)
                        {
                            d.OriginalReceivedDate = ((DateTime)r["OriginalReceivedDate"]).ToString("dd.MM.yyyy");
                            d.OriginalReceived = true;
                        }
                        else
                        {
                            d.OriginalReceivedDate = "";
                            d.OriginalReceived = false;
                        }
                        inThisApp = (1 == (int)r["inThisApp"]);
                        inAppCount = (int)r["inAppCount"];
                        inTestCount = (int)r["inTestCount"];
                        isIdentity = (int)r["isIdentity"];
                        inThisAppTestCount = (int)r["inThisAppTestCount"];
                        //можно редактировать, если не используется в ВИ данного или другого заявления
                        //d.CanBeModified=(inTestCount==0);
                        d.CanBeModified = (inThisAppTestCount == 0);
                        //можно открепить, если не используется в данном заявлении где-нибудь еще
                        //	CanBeDetached=!(x.Entrant_IdentityDocument.Any(u => u.EntrantID==app.EntrantID)||x.ApplicationEntranceTestDocument.Any(y => y.ApplicationID==app.ApplicationID)),
                        d.CanBeDetached = (isIdentity == 0);
                        d.CanBeDeleted = (isIdentity == 0) && !(inAppCount > 0 || inTestCount > 0);
                        d.ShowWarnBeforeModifying = ((inThisApp && inAppCount > 1) || !inThisApp && inAppCount > 0 || inTestCount > 0);

                        bool olympApproved = (r["OlympApproved"] as bool?).GetValueOrDefault();
                        if (d.DocumentTypeID == 9 || d.DocumentTypeID == 10)
                        {
                            d.DocumentTypeName += olympApproved ? " (результаты подтверждены)" : " (результаты не подтверждены)";
                        }


                        if (inThisApp)
                        {
                            appw3.AttachedDocuments.Add(d);
                        }
                        else
                        {
                            appw3.ExistingDocuments.Add(d);
                        }
                    }
                    if (d != null) { appw3.EntrantID = d.EntrantID; }
                    #endregion
                    r.Close();
                    con.Close();

                    appw3.StatusID = new ApplicationRepository().GetStatusApplication(ApplicationID);
                }
                catch (SqlException e)
                {
                    throw e;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (con.State != ConnectionState.Closed) { con.Close(); }
                }
            }
            return appw3;
        }

        public static void Wz2Save(int ApplicationID, int step)
        {
            //#DocumentsCheck - тут была проверка документов, убрана (FIS-1711)
            string sql = @"UPDATE [dbo].[Application] SET [WizardStepID]=@WizardStepID WHERE ApplicationId=@ApplicationID";
             
            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add(new SqlParameter("ApplicationID", SqlDbType.Int) { Value = ApplicationID });
                cmd.Parameters.Add(new SqlParameter("WizardStepID", SqlDbType.Int) { Value = step });

                con.Open();
                cmd.ExecuteNonQuery(); 
            }
        }
    }
}