using Dapper;
using System.Linq;
using System.Collections.Generic;
using GVUZ.Data.Model;
using System;
using System.Data;
using GVUZ.Data.Helpers;
using GVUZ.DAL.Helpers;
using System.Diagnostics;

namespace GVUZ.DAL.Dapper.Repository.Model.OlympicDiplomant
{
    public class OlympicDiplomantRepository : GvuzRepository
    {
        public Stopwatch SWSPFindOlympicDiplomantRVIPersons { get; private set; }

        public OlympicDiplomantRepository()
            : base()
        {
            SWSPFindOlympicDiplomantRVIPersons = new Stopwatch();
        }

        public GVUZ.Data.Model.OlympicDiplomant GetOlympicDiplomantById(long id)
        {
            var sql =
                "select p.*, t.*, s.*, r.* from OlympicDiplomant as p " +
                "left join OlympicDiplomantDocument as t on p.OlympicDiplomantIdentityDocumentID = t.OlympicDiplomantDocumentID " +
                "left join OlympicDiplomantStatus as s on p.StatusID = s.OlympicDiplomantStatusID " +
                "left join RVIPersons as r on p.PersonId = r.PersonId " +
                "where p.OlympicDiplomantID=" + id;

            return DbConnection(db =>
            {

                return db.Query<Data.Model.OlympicDiplomant, OlympicDiplomantDocument,
                    OlympicDiplomantStatus, RVIPersons, Data.Model.OlympicDiplomant>(
                    sql, (p, t, s, r) =>
                    {
                        p.OlympicDiplomantStatus = s;
                        p.OlympicDiplomantDocument = t;

                        r = r ?? new RVIPersons();

                        p.RVIPersons = r;

                        r.RVIPersonIdentDocs =
                            p.PersonId == null ?
                                new List<RVIPersonIdentDocs>() :
                                GetRVIPersonIdentDocsById(p.PersonId).AsList();

                        p.OlympicDiplomantDocumentCanceled = GetOlympicDiplomantDocumentById(id).Where(c => c.OlympicDiplomantDocumentID != p.OlympicDiplomantIdentityDocumentID).AsList();
                        return p;
                    },
                    splitOn: "OlympicDiplomantID,OlympicDiplomantStatusID,PersonId").FirstOrDefault();
            });
        }

        public IEnumerable<RVIPersonIdentDocs> GetRVIPersonIdentDocsById(int? id)
        {
            return DbConnection(db =>
            {
                return db.Query<RVIPersonIdentDocs, RVIDocumentTypes, RVIPersonIdentDocs>(
                    "select d.*, t.* from RVIPersonIdentDocs as d " +
                    "left join RVIDocumentTypes as t on d.DocumentTypeCode = t.DocumentTypeCode " +
                    "where d.PersonId = " + id + " order by d.DocumentTypeCode asc, ISNULL(d.DocumentDate,d.CreateDate) desc",
                    (d, t) =>
                    {
                        d.RVIDocumentTypes = t;
                        return d;
                    },
                    splitOn: "PersonIdentDocID,DocumentTypeCode");
            });
        }

        public IEnumerable<GVUZ.Data.Model.OlympicDiplomant> GetOlympicDiplomantAll(int? orgOlympicEnterID)
        {
            var sql =
                "select p.*, t.*, s.*,l.*,y.*,r.* from OlympicDiplomant as p " +
                "left join OlympicDiplomantDocument as t on p.OlympicDiplomantIdentityDocumentID = t.OlympicDiplomantDocumentID " +
                "left join OlympicDiplomantStatus as s on p.StatusID = s.OlympicDiplomantStatusID " +
                "left join OlympicTypeProfile as l on p.OlympicTypeProfileID = l.OlympicTypeProfileID " +
                "left join OlympicType as y on l.OlympicTypeID = y.OlympicID " +
                "left join OlympicDiplomType as r on p.ResultLevelID = r.OlympicDiplomTypeID " +
                "where l.OrgOlympicEnterID = @OrgOlympicEnterID";



            var args = new
            {
                OrgOlympicEnterID = orgOlympicEnterID
            };

            return DbConnection(db =>
            {
                return db.Query<Data.Model.OlympicDiplomant, OlympicDiplomantDocument,
                    OlympicDiplomantStatus, OlympicTypeProfile, OlympicType, OlympicDiplomType,
                    Data.Model.OlympicDiplomant>(
                    sql, (p, t, s, l, y, r) =>
                    {
                        p.OlympicDiplomantStatus = s;
                        p.OlympicDiplomantDocument = t;
                        p.OlympicTypeProfile = l;
                        l.OlympicType = y;
                        p.OlympicDiplomType = r;
                        return p;
                    },
                    param: args,
                    splitOn: "OlympicDiplomantID,OlympicDiplomantStatusID,OlympicTypeProfileID,OlympicID,OlympicDiplomTypeID");
            });
        }

        public IdentityDocumentType GetIdentityDocumentTypeById(int? id)
        {
            return DbConnection(db =>
            {
                return db.Query<IdentityDocumentType>(
                    "select * from IdentityDocumentType where IdentityDocumentTypeID = " + id).FirstOrDefault();
            });
        }

        public IEnumerable<IdentityDocumentType> GetIdentityDocumentTypeAll()
        {
            return DbConnection(db =>
            {
                return db.Query<IdentityDocumentType>(
                    "select * from IdentityDocumentType");
            });
        }

        public IEnumerable<RegionType> GetRegionTypeAll()
        {
            return DbConnection(db =>
            {
                return db.Query<RegionType>(
                    "select * from RegionType");
            });
        }

        public IEnumerable<OlympicDiplomType> GetOlympicDiplomTypeAll()
        {
            return DbConnection(db =>
            {
                return db.Query<OlympicDiplomType>(
                    "select * from OlympicDiplomType");
            });
        }

        public IEnumerable<OlympicDiplomantStatus> GetOlympicDiplomantStatusAll()
        {
            return DbConnection(db =>
            {
                return db.Query<OlympicDiplomantStatus>(
                    "select * from OlympicDiplomantStatus");
            });
        }

        public IEnumerable<OlympicType> GetOlympicTypeAll()
        {
            return DbConnection(db =>
            {
                return db.Query<OlympicType>(
                    "select * from OlympicType");
            });
        }

        public IEnumerable<OlympicProfile> GetOlympicProfileAll()
        {
            return DbConnection(db =>
            {
                return db.Query<OlympicProfile>(
                    "select * from OlympicProfile");
            });
        }

        public OlympicTypeProfile GetOlympicTypeProfileById(int id)
        {
            var sql =
                "select p.*, t.*, s.* from OlympicTypeProfile as p " +
                "left join OlympicProfile as t on p.OlympicProfileId = t.OlympicProfileId " +
                "left join OlympicType as s on p.OlympicTypeId = s.OlympicId " +
                "where p.OlympicTypeProfileID=" + id;

            return DbConnection(db =>
            {
                return db.Query<OlympicTypeProfile, OlympicProfile, OlympicType, OlympicTypeProfile>(
                    sql, (p, t, s) =>
                    {
                        p.OlympicProfile = t;
                        p.OlympicType = s;
                        return p;
                    },
                    splitOn: "OlympicProfileId,OlympicId").FirstOrDefault();
            });
        }

        public OlympicDiplomantStatus GetOlympicDiplomantStatusById(int? id)
        {
            return DbConnection(db =>
            {
                return db.Query<OlympicDiplomantStatus>(
                    "select * from OlympicDiplomantStatus where OlympicDiplomantStatusID = " + id).FirstOrDefault();
            });
        }

        public bool DeleteOlympicDiplomant(long id)
        {
            WithTransaction(tx =>
            {
                var sql =
                    "update OlympicDiplomant set " +
                    "OlympicDiplomantIdentityDocumentID = null " +
                    " where OlympicDiplomantID = " + id;
                tx.Execute(sql);

                sql = "delete from OlympicDiplomantDocument where OlympicDiplomantID = " + id;
                tx.Execute(sql);

                sql = "delete from OlympicDiplomant where OlympicDiplomantID = " + id;
                tx.Execute(sql);
            });
            return true;
        }

        public IEnumerable<OlympicDiplomantDocument> GetOlympicDiplomantDocumentById(long id)
        {
            var sql =
                "select p.*, t.* from OlympicDiplomantDocument as p " +
                "left join IdentityDocumentType as t on p.IdentityDocumentTypeID = t.IdentityDocumentTypeID " +
                "where p.OlympicDiplomantID = " + id;

            return DbConnection(db =>
            {
                return db.Query<OlympicDiplomantDocument, IdentityDocumentType, OlympicDiplomantDocument>(
                    sql, (p, t) =>
                    {
                        p.IdentityDocumentType = t;
                        return p;
                    },
                    splitOn: "IdentityDocumentTypeID");
            });
        }

        //=====================================================================================================

        public bool UpdateOlympicDiplomant(GVUZ.Data.Model.OlympicDiplomant data)
        {
            var id = data.OlympicDiplomantID;
            var self = data.OlympicDiplomantIdentityDocumentID;
            var oldc = GetOlympicDiplomantDocumentById(id).Where(c => c.OlympicDiplomantDocumentID != self);

            var find = false;
            if (data.CreateDate.Year > 2001)
            {
                find = true;
            }

            WithTransaction(tx =>
            {
                var sql =
                    "update OlympicDiplomant set " +
                    "ResultLevelID = @ResultLevelID, SchoolRegionID = @SchoolRegionID, FormNumber = @FormNumber, " +
                    "EndingDate = @EndingDate, SchoolEgeCode = @SchoolEgeCode, SchoolEgeName = @SchoolEgeName, " +
                    "DiplomaSeries = @DiplomaSeries, DiplomaNumber = @DiplomaNumber, DiplomaDateIssue = @DiplomaDateIssue, " +
                    "OlympicTypeProfileID = @OlympicTypeProfileID, " +
                    "StatusID = @StatusID, PersonID = @PersonID, PersonLinkDate = @PersonLinkDate, Comment = @Comment, " +
                    "AdoptionUnfoundedComment = @AdoptionUnfoundedComment " +
                    "where OlympicDiplomantID = @OlympicDiplomantID";


                if (data.PersonId != null)
                    data.PersonId = data.PersonId == 0 ? null : data.PersonId;

                var args = new
                {
                    OlympicDiplomantID = data.OlympicDiplomantID,
                    ResultLevelID = data.ResultLevelID,
                    SchoolRegionID = data.SchoolRegionID == 0 ? null : data.SchoolRegionID,
                    FormNumber = data.FormNumber,
                    EndingDate = data.EndingDate,
                    SchoolEgeCode = data.SchoolEgeCode,
                    SchoolEgeName = data.SchoolEgeName,
                    DiplomaSeries = data.DiplomaSeries,
                    DiplomaNumber = data.DiplomaNumber,
                    DiplomaDateIssue = data.DiplomaDateIssue,
                    OlympicTypeProfileID = data.OlympicTypeProfileID,
                    StatusID = data.StatusID,
                    PersonID = data.PersonId,
                    PersonLinkDate = data.PersonLinkDate,
                    Comment = data.Comment,
                    AdoptionUnfoundedComment = data.AdoptionUnfoundedComment,
                };

                tx.Execute(sql, args);

                sql =
                    "update OlympicDiplomantDocument set " +
                    "FirstName = @FirstName,  LastName = @LastName,  MiddleName = @MiddleName, " +
                    "BirthDate = @BirthDate, IdentityDocumentTypeID = @IdentityDocumentTypeID, " +
                    "DocumentSeries = @DocumentSeries,  " +
                    "DocumentNumber = @DocumentNumber, " +
                    "OrganizationIssue = @OrganizationIssue,  " +
                    "DateIssue = @DateIssue  " +
                    "where OlympicDiplomantDocumentID = @OlympicDiplomantIdentityDocumentID";

                var arg = new
                {
                    OlympicDiplomantIdentityDocumentID = data.OlympicDiplomantIdentityDocumentID,
                    OlympicDiplomantID = data.OlympicDiplomantID,
                    FirstName = data.OlympicDiplomantDocument.FirstName,
                    LastName = data.OlympicDiplomantDocument.LastName,
                    MiddleName = data.OlympicDiplomantDocument.MiddleName,
                    BirthDate = data.OlympicDiplomantDocument.BirthDate,
                    IdentityDocumentTypeID = data.OlympicDiplomantDocument.IdentityDocumentTypeID,
                    DocumentSeries = data.OlympicDiplomantDocument.DocumentSeries,
                    DocumentNumber = data.OlympicDiplomantDocument.DocumentNumber,
                    OrganizationIssue = data.OlympicDiplomantDocument.OrganizationIssue,
                    DateIssue = data.OlympicDiplomantDocument.DateIssue,
                };

                tx.Execute(sql, arg);

                var newc = data.OlympicDiplomantDocumentCanceled;

                var updateList = newc.Where(p => oldc.Any(x => x.OlympicDiplomantDocumentID == p.OlympicDiplomantDocumentID));
                var deleteList = oldc.Where(p => !newc.Any(x => x.OlympicDiplomantDocumentID == p.OlympicDiplomantDocumentID));
                var insertList = newc.Where(p => p.OlympicDiplomantDocumentID == 0);

                // delete
                foreach (var item in deleteList)
                {
                    sql = "delete from OlympicDiplomantDocument " +
                    "where OlympicDiplomantDocumentID = " + item.OlympicDiplomantDocumentID;
                    tx.Execute(sql);
                }

                // insert
                foreach (var item in insertList)
                {
                    sql =
                        "insert into OlympicDiplomantDocument (OlympicDiplomantID, IdentityDocumentTypeID, " +
                        "FirstName, LastName, MiddleName, BirthDate, DocumentSeries, DocumentNumber, " +
                        "OrganizationIssue, DateIssue) " +

                        "values (@OlympicDiplomantID, @IdentityDocumentTypeID, " +
                        "@FirstName, @LastName, @MiddleName, @BirthDate, @DocumentSeries, @DocumentNumber, " +
                        "@OrganizationIssue, @DateIssue)";

                    var argi = new
                    {
                        OlympicDiplomantID = id,
                        IdentityDocumentTypeID = item.IdentityDocumentTypeID,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        MiddleName = item.MiddleName,
                        BirthDate = item.BirthDate,
                        DocumentSeries = item.DocumentSeries,
                        DocumentNumber = item.DocumentNumber,
                        OrganizationIssue = item.OrganizationIssue,
                        DateIssue = item.DateIssue,
                    };

                    tx.Execute(sql, argi);
                }

                // update
                foreach (var item in updateList)
                {

                    sql =
                        "update OlympicDiplomantDocument set " +
                        "FirstName = @FirstName,  LastName = @LastName,  MiddleName = @MiddleName, " +
                        "BirthDate = @BirthDate, IdentityDocumentTypeID = @IdentityDocumentTypeID, " +
                        "DocumentSeries = @DocumentSeries,  " +
                        "DocumentNumber = @DocumentNumber, " +
                        "OrganizationIssue = @OrganizationIssue,  " +
                        "DateIssue = @DateIssue  " +
                        "where OlympicDiplomantDocumentID = @OlympicDiplomantDocumentID";

                    var argu = new
                    {
                        OlympicDiplomantDocumentID = item.OlympicDiplomantDocumentID,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        MiddleName = item.MiddleName,
                        BirthDate = item.BirthDate,
                        IdentityDocumentTypeID = item.IdentityDocumentTypeID,
                        DocumentSeries = item.DocumentSeries,
                        DocumentNumber = item.DocumentNumber,
                        OrganizationIssue = item.OrganizationIssue,
                        DateIssue = item.DateIssue,
                    };

                    tx.Execute(sql, argu);
                }

                //===========================================================================
                // FindOlympicDiplomantRVIPersonsMultiple
                //===========================================================================

                if (find)
                {

                    var docs = updateList.Concat(insertList).
                        Concat(new List<OlympicDiplomantDocument> { data.OlympicDiplomantDocument });

                    var result = DoFindOlympicDiplomantRVIPersonsMultiple(tx, docs);

                    sql = "update OlympicDiplomant set " +
                    "StatusID = @StatusID, PersonID = @PersonID, PersonLinkDate = @PersonLinkDate " +
                    "where OlympicDiplomantID = @OlympicDiplomantID";

                    if (result.Status == 1)
                    {
                        var argp = new
                        {
                            StatusID = result.Status,
                            OlympicDiplomantID = id,
                            PersonID = result.Persons.FirstOrDefault().PersonId,
                            PersonLinkDate = DateTime.Now
                        };
                        tx.Execute(sql, argp);
                    }
                    else
                    {
                        var argp = new
                        {
                            StatusID = result.Status,
                            OlympicDiplomantID = id,
                            PersonID = (int?)null,
                            PersonLinkDate = (DateTime?)null
                        };
                        tx.Execute(sql, argp);
                    }

                }

                //===========================================================================

            });
            return true;
        }

        public bool InsertOlympicDiplomant(GVUZ.Data.Model.OlympicDiplomant data)
        {
            WithTransaction(tx =>
            {
                var sql =
                    "insert into OlympicDiplomant " +
                    "(OlympicTypeProfileID, ResultLevelID, CreateDate, StatusID, " +
                    "SchoolRegionID, FormNumber, EndingDate, SchoolEgeCode, SchoolEgeName, DiplomaSeries, " +
                    "DiplomaNumber, DiplomaDateIssue) " +
                    "values " +
                    "(@OlympicTypeProfileID, @ResultLevelID, @CreateDate, @StatusID, " +
                    "@SchoolRegionID, @FormNumber, @EndingDate, @SchoolEgeCode, @SchoolEgeName, @DiplomaSeries, " +
                    "@DiplomaNumber, @DiplomaDateIssue); " +
                    "select SCOPE_IDENTITY();";

                var args = new
                {
                    OlympicTypeProfileID = data.OlympicTypeProfileID,
                    ResultLevelID = data.ResultLevelID,
                    CreateDate = DateTime.Now,
                    StatusID = 2,
                    SchoolRegionID = data.SchoolRegionID == 0 ? null : data.SchoolRegionID,
                    FormNumber = data.FormNumber,
                    EndingDate = data.EndingDate,
                    SchoolEgeCode = data.SchoolEgeCode,
                    SchoolEgeName = data.SchoolEgeName,
                    DiplomaSeries = data.DiplomaSeries,
                    DiplomaNumber = data.DiplomaNumber,
                    DiplomaDateIssue = data.DiplomaDateIssue,
                };

                var id = tx.Query<long>(sql, args).Single();

                sql =
                    "insert into OlympicDiplomantDocument (OlympicDiplomantID, IdentityDocumentTypeID, " +
                    "FirstName, LastName, MiddleName, BirthDate, DocumentSeries, DocumentNumber, " +
                    "OrganizationIssue, DateIssue) " +

                    "values (@OlympicDiplomantID, @IdentityDocumentTypeID, " +
                    "@FirstName, @LastName, @MiddleName, @BirthDate, @DocumentSeries, @DocumentNumber, " +
                    "@OrganizationIssue, @DateIssue); " +
                    "select SCOPE_IDENTITY();";

                var arg = new
                {
                    OlympicDiplomantID = id,
                    IdentityDocumentTypeID = data.OlympicDiplomantDocument.IdentityDocumentTypeID,

                    FirstName = data.OlympicDiplomantDocument.FirstName,
                    LastName = data.OlympicDiplomantDocument.LastName,
                    MiddleName = data.OlympicDiplomantDocument.MiddleName,
                    BirthDate = data.OlympicDiplomantDocument.BirthDate,

                    DocumentSeries = data.OlympicDiplomantDocument.DocumentSeries,
                    DocumentNumber = data.OlympicDiplomantDocument.DocumentNumber,
                    OrganizationIssue = data.OlympicDiplomantDocument.OrganizationIssue,
                    DateIssue = data.OlympicDiplomantDocument.DateIssue,
                };

                var idd = tx.Query<long>(sql, arg).Single();

                sql =
                    "update OlympicDiplomant set " +
                    "OlympicDiplomantIdentityDocumentID = @OlympicDiplomantIdentityDocumentID " +
                    "where OlympicDiplomantID = @OlympicDiplomantID";

                var argd = new
                {
                    OlympicDiplomantID = id,
                    OlympicDiplomantIdentityDocumentID = idd,
                };

                tx.Execute(sql, argd);

                // сохранение недейстительных документов

                IEnumerable<OlympicDiplomantDocument> oldc = new List<OlympicDiplomantDocument>();
                var newc = data.OlympicDiplomantDocumentCanceled;

                var updateList = newc.Where(p => oldc.Any(x => x.OlympicDiplomantDocumentID == p.OlympicDiplomantDocumentID));
                var deleteList = oldc.Where(p => !newc.Any(x => x.OlympicDiplomantDocumentID == p.OlympicDiplomantDocumentID));
                var insertList = newc.Where(p => p.OlympicDiplomantDocumentID == 0);

                foreach (var item in deleteList)
                {
                    sql = "delete from OlympicDiplomantDocument " +
                    "where OlympicDiplomantDocumentID = " + item.OlympicDiplomantDocumentID;
                    tx.Execute(sql);
                }

                foreach (var item in insertList)
                {
                    sql =
                        "insert into OlympicDiplomantDocument (OlympicDiplomantID, IdentityDocumentTypeID, " +
                        "FirstName, LastName, MiddleName, BirthDate, DocumentSeries, DocumentNumber, " +
                        "OrganizationIssue, DateIssue) " +

                        "values (@OlympicDiplomantID, @IdentityDocumentTypeID, " +
                        "@FirstName, @LastName, @MiddleName, @BirthDate, @DocumentSeries, @DocumentNumber, " +
                        "@OrganizationIssue, @DateIssue)";

                    var argi = new
                    {
                        OlympicDiplomantID = id,
                        IdentityDocumentTypeID = item.IdentityDocumentTypeID,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        MiddleName = item.MiddleName,
                        BirthDate = item.BirthDate,
                        DocumentSeries = item.DocumentSeries,
                        DocumentNumber = item.DocumentNumber,
                        OrganizationIssue = item.OrganizationIssue,
                        DateIssue = item.DateIssue,
                    };

                    tx.Execute(sql, argi);
                }

                foreach (var item in updateList)
                {
                    sql =
                        "update OlympicDiplomantDocument set " +
                        "FirstName = @FirstName,  LastName = @LastName,  MiddleName = @MiddleName, " +
                        "BirthDate = @BirthDate, IdentityDocumentTypeID = @IdentityDocumentTypeID, " +
                        "DocumentSeries = @DocumentSeries,  " +
                        "DocumentNumber = @DocumentNumber, " +
                        "OrganizationIssue = @OrganizationIssue,  " +
                        "DateIssue = @DateIssue  " +
                        "where OlympicDiplomantDocumentID = @OlympicDiplomantDocumentID";

                    var argu = new
                    {
                        OlympicDiplomantDocumentID = item.OlympicDiplomantDocumentID,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        MiddleName = item.MiddleName,
                        BirthDate = item.BirthDate,
                        IdentityDocumentTypeID = item.IdentityDocumentTypeID,
                        DocumentSeries = item.DocumentSeries,
                        DocumentNumber = item.DocumentNumber,
                        OrganizationIssue = item.OrganizationIssue,
                        DateIssue = item.DateIssue,
                    };

                    tx.Execute(sql, argu);
                }

                //===========================================================================
                // FindOlympicDiplomantRVIPersonsMultiple
                //===========================================================================

                var docs = updateList.Concat(insertList).
                    Concat(new List<OlympicDiplomantDocument> { data.OlympicDiplomantDocument });


                var result = DoFindOlympicDiplomantRVIPersonsMultiple(tx, docs);

                sql = "update OlympicDiplomant set " +
                "StatusID = @StatusID, PersonID = @PersonID, PersonLinkDate = @PersonLinkDate " +
                "where OlympicDiplomantID = @OlympicDiplomantID";

                if (result.Status == 1)
                {
                    var argp = new
                    {
                        StatusID = result.Status,
                        OlympicDiplomantID = id,
                        PersonID = result.Persons.FirstOrDefault().PersonId,
                        PersonLinkDate = DateTime.Now
                    };
                    tx.Execute(sql, argp);
                }
                else
                {
                    var argp = new
                    {
                        StatusID = result.Status,
                        OlympicDiplomantID = id,
                        PersonID = (int?)null,
                        PersonLinkDate = (DateTime?)null
                    };
                    tx.Execute(sql, argp);
                }
                //===========================================================================

            });
            return true;
        }

        //=====================================================================================================

        public FindPersonResultModel SyncOlympicDiplomant(IEnumerable<OlympicDiplomantDocument> documents)
        {
            FindPersonResultModel result = new FindPersonResultModel();

            WithTransaction(tx =>
            {
                result = DoFindOlympicDiplomantRVIPersonsMultiple(tx, documents);
            });

            return result;
        }

        public FindPersonResultModel DoFindOlympicDiplomantRVIPersonsMultiple(IDbTransaction tx,
            IEnumerable<OlympicDiplomantDocument> documents)
        {

            FindPersonResultModel result = new FindPersonResultModel();

            DataTable olympicDiplomantDataTable = new DataTable();
            olympicDiplomantDataTable.Columns.Add("Id", typeof(long));
            olympicDiplomantDataTable.Columns.Add("LastName", typeof(string));
            olympicDiplomantDataTable.Columns.Add("FirstName", typeof(string));
            olympicDiplomantDataTable.Columns.Add("Patronymic", typeof(string));
            olympicDiplomantDataTable.Columns.Add("DocumentNumber", typeof(string));
            olympicDiplomantDataTable.Columns.Add("DocumentSeries", typeof(string));
            olympicDiplomantDataTable.Columns.Add("BirthDate", typeof(DateTime));
            foreach (var document in documents)
            {
                olympicDiplomantDataTable.Rows.Add(0,
                    document.LastName, document.FirstName, document.MiddleName,
                    document.DocumentNumber, document.DocumentSeries, document.BirthDate);
            }

            var args = new DynamicParameters();

            args.Add("@olympicDiplomantData", olympicDiplomantDataTable.AsTableValuedParameter());
            args.Add("@statusId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            args.Add("@statusName", dbType: DbType.StringFixedLength, size: 50, direction: ParameterDirection.Output);

            SWSPFindOlympicDiplomantRVIPersons.Start();
            result.Persons = tx.Query<FindPerson>("FindOlympicDiplomantRVIPersonsMultiple",
                args, commandType: CommandType.StoredProcedure);
            SWSPFindOlympicDiplomantRVIPersons.Stop();
            result.Status = args.Get<int>("@statusId");
            result.Text = args.Get<string>("@statusName");

            return result;
        }

        //=====================================================================================================

        public IEnumerable<RVIPersonIdentDocs> FindPersons(FindPerson model)
        {
            var sql = "select top 10 p.*,d.*,t.* from RVIPersons as d " +
                       "inner join RVIPersonIdentDocs as p on p.PersonId = d.PersonId " +
                       "inner join RVIDocumentTypes as t on t.DocumentTypeCode = p.DocumentTypeCode ";

            var f1 = model.LastName != null ? "d.NormSurname like '%" + 
                model.LastName.Trim().ToUpper().Replace("Ё", "Е").Replace("Й", "И") + "%'" : "";

            var f2 = model.FirstName != null ? "d.NormName like '%" + 
                model.FirstName.Trim().ToUpper().Replace("Ё", "Е").Replace("Й", "И") + "%'" : "";

            var f3 = model.Patronymic != null ? "d.NormSecondName like '%" + 
                model.Patronymic.Trim().ToUpper().Replace("Ё", "Е").Replace("Й", "И") + "%'" : "";

            var f4 = model.BirthDay != null ? "d.BirthDay = @birthDay" : "";
            var f5 = model.DocumentSeries != null ? "p.DocumentSeries like '%" + model.DocumentSeries + "%'" : "";
            var f6 = model.DocumentNumber != null ? "p.DocumentNumber like '%" + model.DocumentNumber + "%'" : "";
            var f7 = model.PersonIdentDocID > 0 ? "p.DocumentTypeCode = " + model.PersonIdentDocID + "" : "";

            var f = f1;

            f = (f != "" && f2 != "") ? f + " and " + f2 : f + f2;
            f = (f != "" && f3 != "") ? f + " and " + f3 : f + f3;
            f = (f != "" && f4 != "") ? f + " and " + f4 : f + f4;
            f = (f != "" && f5 != "") ? f + " and " + f5 : f + f5;
            f = (f != "" && f6 != "") ? f + " and " + f6 : f + f6;
            f = (f != "" && f7 != "") ? f + " and " + f7 : f + f7;

            if (f != "")
                f = " where " + f;

            sql = sql + f;

            return DbConnection(db => db.Query<RVIPersonIdentDocs, RVIPersons, RVIDocumentTypes, RVIPersonIdentDocs >(
                    sql, (p, d, t) =>
                    {
                        p.RVIPersons = d;
                        p.RVIDocumentTypes = t;
                        return p;
                    },
                    splitOn: "PersonId, DocumentTypeCode",
                    param: new { birthDay = model.BirthDay}));
        }

        //=====================================================================================================

        public RVIPersons GetRVIPersonsById(int id)
        {
            var sql = "select * from RVIPersons where PersonId = " + id;

            return DbConnection(db =>
            {
                var p = db.Query<RVIPersons>(sql).FirstOrDefault();
                if (p != null)
                    p.RVIPersonIdentDocs = GetRVIPersonIdentDocsById(p.PersonId).AsList();
                return p;
            });
        }

        public OlympicType GetOlympicTypeByNameAndYear(string name, int year)
        {
            var sql = "select * from OlympicType where Name = @Name and OlympicYear = @OlympicYear";
            var arg = new
            {
                Name = name,
                OlympicYear = year
            };
            return DbConnection(db =>
            {
                return db.Query<OlympicType>(sql, arg).FirstOrDefault();
            });
        }

        public OlympicProfile GetOlympicProfileByName(string name)
        {
            var sql = "select * from OlympicProfile where ProfileName = @ProfileName";
            var arg = new
            {
                ProfileName = name,
            };
            return DbConnection(db =>
            {
                return db.Query<OlympicProfile>(sql, arg).FirstOrDefault();
            });
        }

        public OlympicTypeProfile GetOlympicTypeProfileByTypeAndProfile(int typeId, int profileId, int orgOlympicEnterID)
        {
            var sql = "select * from OlympicTypeProfile " +
                "where OlympicTypeID = @OlympicTypeID and OlympicProfileID = @OlympicProfileID and OrgOlympicEnterID = @OrgOlympicEnterID";
            var arg = new
            {
                OlympicTypeID = typeId,
                OlympicProfileID = profileId,
                OrgOlympicEnterID = orgOlympicEnterID
            };
            return DbConnection(db =>
            {
                return db.Query<OlympicTypeProfile>(sql, arg).FirstOrDefault();
            });
        }

        public long? FindOlympicDiplomant(string lastName, string firstName,
            int identityDocumentTypeId, string documentNumber, int olympicTypeProfileId,
            string diplomaNumber)
        {
            var sql =
                "select o.OlympicDiplomantID from OlympicDiplomant as o " +
                "left join OlympicDiplomantDocument as d on o.OlympicDiplomantIdentityDocumentID = d.OlympicDiplomantDocumentID " +
                "where " +
                "o.OlympicTypeProfileID = @OlympicTypeProfileID and " +
                "d.IdentityDocumentTypeID = @IdentityDocumentTypeID and " +
                "d.FirstName = @FirstName and d.LastName = @LastName and d.DocumentNumber = @DocumentNumber";

            if (!String.IsNullOrEmpty(diplomaNumber))
            {
                sql += " and o.DiplomaNumber = @DiplomaNumber";
            }

            var arg = new
            {
                OlympicTypeProfileID = olympicTypeProfileId,
                IdentityDocumentTypeID = identityDocumentTypeId,
                FirstName = firstName,
                LastName = lastName,
                DocumentNumber = documentNumber,
                DiplomaNumber = diplomaNumber,
            };

            return DbConnection(db =>
            {
                return db.Query<long?>(sql, arg).FirstOrDefault();
            });
        }

        public IEnumerable<OlympicTypeProfile> GetOlympicsData()
        {
            var sql = "select p.*,t.*,r.* " +
                "from OlympicTypeProfile as p " +
                "left join OlympicType as t on p.OlympicTypeID = t.OlympicID " +
                "left join OlympicProfile as r on p.OlympicProfileID = r.OlympicProfileID ";

            return DbConnection(db =>
            {
                return db.Query<OlympicTypeProfile, OlympicType, OlympicProfile, OlympicTypeProfile>(
                    sql, (p, t, r) =>
                    {
                        p.OlympicType = t;
                        p.OlympicProfile = r;
                        return p;
                    },
                    splitOn: "OlympicID,OlympicProfileID");
            });
        }

        public bool SyncOlympicDiplomantMultiple(long[] list)
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(long));
            foreach (var item in list)
                table.Rows.Add(item);

            var args = new DynamicParameters();
            args.Add("@diplomantIds", table.AsTableValuedParameter());

            WithTransaction(tx =>
            {
                return tx.Query<bool>("SyncOlympicDiplomantMultiple", args, commandType: CommandType.StoredProcedure);
            });

            return true;
        }

    }
}