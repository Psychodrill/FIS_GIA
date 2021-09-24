using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using AutoMapper;
using FogSoft.Helpers;
using GVUZ.Helper.Import;
using GVUZ.Model;
using GVUZ.Model.Helpers;
using GVUZ.ServiceModel.Import.Bulk.Extensions;
using GVUZ.ServiceModel.Import.Bulk.Model.Results;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import
{
	/// <summary>
	/// Экстеншены к датамодели импорта. По факту не используются. Оставлены для тестов.
	/// </summary>
    public partial class ImportEntities : IPersonalDataAccessLogger
	{
        public List<TResult> InsertAsXml<M, TResult>(IEnumerable<M> dto, string sql, int step, params SqlParameter[] parms) 
            where M : class
            where TResult : class, IEmptyResult, new()
        {
            var result = new List<TResult>();
            var sw = new Stopwatch();
            
            try
            {
                //var _connectionString = ((EntityConnection) this.Connection).StoreConnection.ConnectionString;
                var _connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;

                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        sw.Start();
                        LogHelper.Log.InfoFormat(
                            "========================================================================================");

                        var items = new List<M>(dto);

                        int i = 0;
                        while (items.Count > 0)
                        {
                            M[] otherCollectionPlaces = items.Take(step).ToArray();
                            i += otherCollectionPlaces.Length;

                            string xmlstring = new Serializer().Serialize(otherCollectionPlaces);
                            items = items.Except(otherCollectionPlaces).ToList();

                            var command = new SqlCommand(sql, connection, transaction);
                            command.CommandTimeout = EntitiesHelper.CommandTimeout;

                            foreach (var parm in parms)
                                command.Parameters.Add(new SqlParameter(parm.ParameterName, parm.SqlValue));
                            command.Parameters.Add(new SqlParameter("xml", xmlstring));

                            var res = command.ExecuteXmlToObject<TResult>();
                            if (!res.IsEmpty)
                                result.Add(res);

                            LogHelper.Log.InfoFormat("Обработано {0} заявлений", i);
                        }
                        transaction.Commit();

                        sw.Stop();
                        LogHelper.Log.InfoFormat("Время транзакции удаления заявлений: {0} сек, кол-во: {1}",
                            sw.Elapsed.TotalSeconds, i);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message, ex);
                throw;
            }

            return result;
        }

		partial void OnContextCreated()
		{
			this.InitCommandTimeout();
		}

	    /// <summary>
        /// Сброс закешированных объектов EF
        /// </summary>
        public void Flush()
        {
            foreach (var entry in this.ObjectStateManager.GetObjectStateEntries(System.Data.EntityState.Added |
                System.Data.EntityState.Deleted | System.Data.EntityState.Modified | System.Data.EntityState.Unchanged))
            {
                Detach(entry.Entity);
            }
        }

        /// <summary>
        /// Аттачим объект к контексту - так как его там может не быть
        /// + перегоняем Dto в Entity после этого
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="from"></param>
        /// <param name="entity"></param>
        public void AttachAndMap<TFrom, T>(TFrom from, T entity) where T : IEntityWithKey
        {
            this.AttachToOrGet(entity.GetType().Name, ref entity);
            Mapper.Map(from, entity);
        }

        public void AttachToOrGet<T>(string entitySetName, ref T entity)
            where T : IEntityWithKey
        {
            ObjectStateEntry entry;
            bool attach = false;
            if (this.ObjectStateManager.TryGetObjectStateEntry(this.CreateEntityKey(entitySetName, entity), out entry))
            {
                attach = entry.State == System.Data.EntityState.Detached;
                entity = (T)entry.Entity;
            }
            else
            {
                attach = true;
            }
            if (attach)
                this.AttachTo(entitySetName, entity);
        }

	    public static void ClearInstitutionStructure(int institutionID)
		{
			using (var dbContext = new ImportEntities())
			{
				dbContext.ExecuteStoreCommand(
                        @"
			BEGIN TRAN

			DELETE FROM AdmissionVolume WHERE InstitutionID = @instID

			DELETE FROM InstitutionStructure WHERE InstitutionItemID IN 
			(SELECT InstitutionItemID FROM InstitutionItem WHERE InstitutionID = @InstID)
			DELETE InstitutionItem WHERE InstitutionID = @InstID

			DELETE FROM BenefitItemCOlympicType 
			from BenefitItemCOlympicType bicot JOIN BenefitItemC bic ON bic.BenefitItemID = bicot.BenefitItemID
			 JOIN CompetitiveGroup cg ON cg.CompetitiveGroupID = bic.CompetitiveGroupID
			WHERE cg.CompetitiveGroupID IN (SELECT cg.CompetitiveGroupID FROM CompetitiveGroup cg (NOLOCK) WHERE cg.InstitutionID = @InstID)

			DELETE FROM BenefitItemC WHERE CompetitiveGroupID IN (SELECT cg.CompetitiveGroupID FROM CompetitiveGroup cg WHERE cg.InstitutionID = @InstID)

			DELETE FROM ApplicationEntranceTestDocument WHERE ApplicationID IN (SELECT a.ApplicationID FROM [Application] a (NOLOCK) WHERE a.InstitutionID = @InstID
			 OR a.OrderCompetitiveGroupID IN (SELECT cg.CompetitiveGroupID FROM CompetitiveGroup cg (NOLOCK) WHERE cg.InstitutionID = @InstID))
			DELETE FROM ApplicationEntrantDocument WHERE ApplicationID IN (SELECT a.ApplicationID FROM [Application] a (NOLOCK) WHERE a.InstitutionID = @InstID
			 OR a.OrderCompetitiveGroupID IN (SELECT cg.CompetitiveGroupID FROM CompetitiveGroup cg (NOLOCK) WHERE cg.InstitutionID = @InstID))
			DELETE FROM [APPLICATION] WHERE InstitutionID = @InstID OR 
				OrderCompetitiveGroupID IN (SELECT cg.CompetitiveGroupID FROM CompetitiveGroup cg (NOLOCK) WHERE cg.InstitutionID = @InstID)
			DELETE FROM CompetitiveGroupTargetItem WHERE CompetitiveGroupItemID IN 
(SELECT CompetitiveGroupItemID FROM CompetitiveGroupItem (NOLOCK) WHERE CompetitiveGroupID IN (SELECT CompetitiveGroupID FROM CompetitiveGroup (NOLOCK) WHERE InstitutionID=@InstID))
			
			DELETE FROM CompetitiveGroupItem WHERE CompetitiveGroupID IN (SELECT CompetitiveGroupID FROM CompetitiveGroup (NOLOCK) WHERE InstitutionID=@InstID)
			DELETE FROM CompetitiveGroupTarget WHERE InstitutionID=@InstID
			DELETE FROM CompetitiveGroup WHERE InstitutionID = @InstID
			DELETE FROM OrderOfAdmission WHERE InstitutionID = @InstID
			DELETE FROM InstitutionStructure WHERE InstitutionItemID IN (SELECT ii.InstitutionItemID FROM InstitutionItem ii WHERE ii.InstitutionID = @InstID)
			DELETE FROM InstitutionItem WHERE InstitutionID = @InstID
COMMIT
			", new SqlParameter { ParameterName = "@InstID", Value = institutionID });
			}
		}

		public static void ClearInstitutionApplicationStructure(int institutionID)
		{
			using (ImportEntities dbContext = new ImportEntities())
			{
				dbContext.ExecuteStoreCommand(
                        @"
BEGIN TRAN

UPDATE Entrant SET IdentityDocumentID = NULL
FROM Entrant ent JOIN [Application] a ON a.EntrantID = ent.EntrantID
WHERE a.InstitutionID = @institutionID

DELETE FROM ApplicationEntrantDocument 
FROM ApplicationEntrantDocument aed JOIN [Application] a ON a.ApplicationID = aed.ApplicationID
WHERE a.InstitutionID = @institutionID

UPDATE Entrant SET IdentityDocumentID=NULL WHERE (SELECT COUNT(*) FROM Application WHERE (OrigEntrantID=Entrant.EntrantID OR EntrantID=Entrant.EntrantID) AND InstitutionID=@institutionID) > 0


DELETE FROM EntrantDocument 
FROM EntrantDocument ed JOIN ApplicationEntrantDocument aed ON aed.EntrantDocumentID = ed.EntrantDocumentID
JOIN [Application] a ON a.ApplicationID = aed.ApplicationID
WHERE a.InstitutionID = @institutionID

DELETE FROM EntrantDocument 
FROM EntrantDocument ed JOIN Entrant e ON ed.EntrantDocumentID = e.IdentityDocumentID
JOIN [Application] a ON a.EntrantID = e.EntrantID
WHERE a.InstitutionID = @institutionID

DELETE FROM ApplicationEntranceTestDocument
FROM ApplicationEntranceTestDocument aetd JOIN [Application] a ON a.ApplicationID = aetd.ApplicationID
WHERE a.InstitutionID = @institutionID

DELETE FROM EntrantDocument 
FROM EntrantDocument ed JOIN Entrant e ON ed.EntrantID = e.EntrantID
JOIN [Application] a ON a.EntrantID = e.EntrantID
WHERE a.InstitutionID = @institutionID

DELETE FROM EntrantLanguage 
FROM EntrantLanguage el JOIN Entrant e ON e.EntrantID = el.EntrantID
 JOIN [Application] a ON a.EntrantID = e.EntrantID OR a.OrigEntrantID = e.EntrantID
WHERE a.InstitutionID = @institutionID

DELETE FROM OrderOfAdmissionHistory
FROM OrderOfAdmissionHistory aetd JOIN [Application] a ON a.ApplicationID = aetd.ApplicationID
WHERE a.InstitutionID = @institutionID

DELETE FROM [Application]
WHERE InstitutionID = @institutionID

DELETE FROM EntrantDocument
FROM EntrantDocument ed JOIN
(SELECT e.entrantID
FROM Entrant e 
WHERE NOT EXISTS(SELECT * FROM [Application] a WHERE a.EntrantID = e.EntrantID) AND 
 NOT EXISTS(SELECT * FROM [Application] a WHERE a.OrigEntrantID = e.EntrantID) AND 
 NOT EXISTS(SELECT * FROM EntrantLanguage el WHERE el.EntrantID = e.EntrantID)) a1 ON ed.EntrantID = a1.entrantID

DELETE FROM Entrant
FROM Entrant e
WHERE NOT EXISTS(SELECT * FROM [Application] a WHERE a.EntrantID = e.EntrantID) AND 
 NOT EXISTS(SELECT * FROM [Application] a WHERE a.OrigEntrantID = e.EntrantID) AND 
 NOT EXISTS(SELECT * FROM EntrantLanguage el WHERE el.EntrantID = e.EntrantID)

DELETE FROM PersonParent
FROM PersonParent pp 
WHERE NOT EXISTS(SELECT * FROM Entrant e WHERE e.FatherID = pp.PersonParentID) AND 
	NOT EXISTS(SELECT * FROM Entrant e WHERE e.MotherID = pp.PersonParentID)

DELETE FROM Person 
FROM Person p 
WHERE NOT EXISTS(SELECT * FROM Entrant e WHERE e.PersonID = p.PersonID) AND 
	NOT EXISTS(SELECT * FROM Entrant e WHERE e.FatherID = p.PersonID) AND 
	NOT EXISTS(SELECT * FROM Entrant e WHERE e.MotherID = p.PersonID) AND
	NOT EXISTS(SELECT * FROM PersonParent pp WHERE pp.PersonID = p.PersonID)
COMMIT

			", new SqlParameter { ParameterName = "@institutionID", Value = institutionID });
			}
		}

		/// <summary>
		/// Загружает ОУ по идентификатору.</summary>
		public Institution LoadInstitution(int instituionID)
		{
			return Institution
				.Include(x => x.AdmissionVolume)
				.Include(x => x.CompetitiveGroup).First(x => x.InstitutionID == instituionID);
		}

        public IPersonalDataAccessLog CreatePersonalDataAccessLog()
	    {
            var l = new PersonalDataAccessLog();
            PersonalDataAccessLog.AddObject(l);
            return l;	        
	    }
	}
}
