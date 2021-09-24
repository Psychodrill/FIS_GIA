using System;
using System.Data.Objects;
using System.IO;
using System.Linq;
using System.Web;
using FogSoft.Helpers;
using GVUZ.Web.Portlets;

namespace GVUZ.Web.ContextExtensions
{
	/// <summary>
	/// Методы для работы с аттачментами
	/// </summary>
	public static class AttachmentExtensions
	{
		/// <summary>
		/// Создайм аттачмент в нужном контексте
		/// </summary>
		public static int CreateAttachment<T>(this T factory, HttpPostedFileBase file)
			where T : ObjectContext//, IAttachmentFactory
		{
			//некорректный файл, не вставляем
			if (file == null || file.ContentLength == 0)
				return 0;
			return CreateAttachment(factory, file.ContentType, Path.GetFileName(file.FileName), new BinaryReader(file.InputStream).ReadBytes(file.ContentLength));
		}

        public static int CreateAttachment1<T>(this T factory, HttpPostedFileBase file, int Year, int Inst)
            where T : ObjectContext//, IAttachmentFactory
        {
            //некорректный файл, не вставляем
            if (file == null || file.ContentLength == 0)
                return 0;
            return CreateAttachment1(factory, Year, Inst, file.ContentType, file.FileName, new BinaryReader(file.InputStream).ReadBytes(file.ContentLength));
        }

		/// <summary>
		/// Записываем его в базу
		/// </summary>
		public static int CreateAttachment<T>(this T dbContext, string mimeType, string fileName, byte[] body)
			where T : ObjectContext
		{
			return dbContext.ExecuteStoreQuery<Decimal>(@"INSERT INTO Attachment(MimeType, [Name], [Body]) VALUES({0}, {1}, {2})
							SELECT SCOPE_IDENTITY()", mimeType, fileName, body).Single().To(0);
		}

        public static int CreateAttachment1<T>(this T dbContext, int Year, int Inst, string mimetype, string name, byte[] body)
            where T : ObjectContext
        {
            dbContext.ExecuteStoreQuery<Decimal?>(@"DELETE FROM [AdmissionRules] WHERE [InstitutionID]={0} AND [Year]={1} SELECT SCOPE_IDENTITY()", Inst, Year).Single();
            return dbContext.ExecuteStoreQuery<Decimal>(@"INSERT INTO AdmissionRules([InstitutionID] ,[Year] , [MimeType], [FileName],[File]) VALUES({0}, {1}, {2}, {3} ,{4})
							SELECT SCOPE_IDENTITY()", Inst, Year, mimetype, name, body).Single().To(0);
        }

        public static void DeleteAttachment1<T>(this T dbContext, int Year, int Inst)
            where T : ObjectContext
        {
            dbContext.ExecuteStoreQuery<Decimal?>(@"DELETE FROM [AdmissionRules] WHERE [InstitutionID]={0} AND [Year]={1} SELECT SCOPE_IDENTITY()", Inst, Year).Single();
        }

		/// <summary>
		/// Вытаскиваем содержимое аттачмента по ID
		/// </summary>
		public static byte[] GetAttachmentBody<T>(this T dbContext, int attachmentID)
			where T : ObjectContext
		{
			return dbContext.ExecuteStoreQuery<byte[]>(@"SELECT Body FROM Attachment WHERE AttachmentID={0}", attachmentID).Single();
		}

        public static byte[] GetRulesBody<T>(this T dbContext, int recordid)
            where T : ObjectContext
        {
            return dbContext.ExecuteStoreQuery<byte[]>(@"SELECT [File] FROM AdmissionRules WHERE RecordID={0}", recordid).Single();
        }

		/// <summary>
		/// Создаём аттачмент, пришедший из портлетов
		/// </summary>
		public static int CreatePortletAttachment<T>(this T factory, PortletFileHelper.UploadFile file)
			where T : ObjectContext//, IAttachmentFactory
		{
			//некорректный файл, не вставляем
			if (file == null || file.Content == null || file.Content.Length == 0)
				return 0;
			return CreateAttachment(factory, "application/octet-stream", Path.GetFileName(file.FileName), file.Content);
		}
	}
}