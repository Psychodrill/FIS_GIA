using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using Fbs.UploadModule;

namespace Fbs.Web.Personal.Profile
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class UploadState : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string currentFileID = context.Request.QueryString["PostID"];
            if (!String.IsNullOrEmpty(currentFileID))
            {
                UploadStatus status = context.Application[currentFileID] as UploadStatus;
                if (status == null)
                    status = new UploadStatus(currentFileID, "", "");
                else
                {
                    if (context.Request.QueryString["cancel"] != null)
                    {
                        status.IsCanceled = true;
                        context.Application[currentFileID] = status;
                    }

                    if (status.IsDone || status.ErrorLevel != UploadStatus.ErrorEnum.None)
                        context.Application.Remove(status.FileId);
                }

                string json = "";

                if (status.ErrorLevel == UploadStatus.ErrorEnum.None)
                    json = String.Format("{{totalBytes:{0},transBytes:{1}}}", status.TotalBytes,
                        status.CurrentBytesTransfered);
                else
                    json = String.Format("{{errorLevel:{0},errorMsg:'{1}'}}", (int)status.ErrorLevel,
                        status.ErrorMsg);

                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                context.Response.Write(json);
            }

            // Эмуляция подвисшей закачки. Специально оставлено закомментированным.
            //string json = "";
            //json = String.Format("{{totalBytes:{0},transBytes:{1}}}", 100, 0);
            //context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //context.Response.Write(json);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
