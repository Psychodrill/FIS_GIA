using Dapper;
using System.Linq;
using System.Collections.Generic;
using GVUZ.DAL.Dapper.ViewModel.Olympics;
using GVUZ.DAL.Dapper.Repository.Interfaces.Olympics;
using System.Data;
using GVUZ.Data.Model;
using System;
using GVUZ.Data.Helpers;
using GVUZ.DAL.Model.Common;
using GVUZ.DAL.Helpers;
using GVUZ.DAL.Model;

namespace GVUZ.DAL.Dapper.Repository.Model.Olympics
{
    public class OlympicsRepository : GvuzRepository, IOlympicsRepository
    {
        public OlympicsRepository()
            : base()
        {
        }

        // основная выборка
        public IEnumerable<OlympicsListViewModel.RowData> GetOlympicsData()
        {
            var sql = "select o.OlympicID,o.Name,o.OlympicNumber,o.OlympicYear, " +
                "(select n.ProfileName from OlympicProfile as n where n.OlympicProfileID = p.OlympicProfileID) ProfileName, " +
                "(select l.Name from OlympicLevel as l where l.OlympicLevelID = p.OlympicLevelID) LevelName, " +
                "p.OrganizerName,p.OlympicProfileID,p.OlympicLevelID,p.OlympicTypeProfileID " +
                "from OlympicTypeProfile as p " +
                "left join OlympicType as o on p.OlympicTypeID = o.OlympicID ";

            return DbConnection(db =>
            {
                return db.Query<OlympicsListViewModel.RowData>(sql);
            });
        }

        // предметы олимпиад
        public IEnumerable<OlympicsListViewModel.RowSubjectData> GetOlympicsSubjectsData()
        {
            return DbConnection(db =>
            {
                return db.Query<OlympicsListViewModel.RowSubjectData>(SQLQuery.GetOlympicsSubjects);
            });
        }

        //public OlympicsCatalogEditViewModel GetOlympic(int? id)
        //{
        //    var record = GetOlympicsData().Where(x => x.OlympicTypeProfileID == id).FirstOrDefault();
        //    if (record == null)
        //        record = new OlympicsListViewModel.RowData { };


        //    var model = new OlympicsCatalogEditViewModel();
        //    model.Data.OlympicTypeProfileID = record.OlympicTypeProfileID;
        //    model.Data.OrganizerName = record.OrganizerName;

        //    return model;
        //}

        // редактирование, добавление олимпиады
        // запись таблицы OlympicTypeProfile, фактически это детализация для таблицы OlympicType
        public bool OlympicUpdate(OlympicsCatalogEditViewModel model)
        {

            // проверка на уникальность олимпиады
            var sql = "select * from OlympicTypeProfile where OlympicTypeID = " + model.Data.OlympicTypeID +
                " and OlympicProfileID = " + model.Data.OlympicProfileID + " and OlympicTypeProfileID != " +
                model.Data.OlympicTypeProfileID;

            var find = DbConnection(db => { return db.Query<OlympicTypeProfile>(sql).FirstOrDefault(); });

            // не уникально
            if (find != null)
                return false;

            sql = "select * from OlympicSubject u where u.OlympicTypeProfileID = @OlympicTypeProfileID";
            var oldSubjects = DbConnection(db =>
            {
                return db.Query<OlympicSubject>(sql, new { OlympicTypeProfileID = model.Data.OlympicTypeProfileID });
            });
            var newSubjects = model.Data.OlympicSubject;

            var newList = newSubjects.Where(p => !oldSubjects.Any(x => x.SubjectID == p.SubjectID));
            var oldList = oldSubjects.Where(p => !newSubjects.Any(x => x.SubjectID == p.SubjectID));

            var keys = oldList.Select(i => new { i.OlympicSubjectID }).ToList();

            var args = new
            {
                OlympicTypeProfileID = model.Data.OlympicTypeProfileID,
                OlympicTypeID = model.Data.OlympicTypeID,       // !!!
                OlympicProfileID = model.Data.OlympicProfileID, // !!!
                OlympicLevelID = model.Data.OlympicLevelID,
                OrganizerName = model.Data.OrganizerName,
                FirstName = model.Data.FirstName,
                LastName = model.Data.LastName,
                MiddleName = model.Data.MiddleName,
                Email = model.Data.Email,
                Position = model.Data.Position,
                OrganizerAddress = model.Data.OrganizerAddress,
                PhoneNumber = model.Data.PhoneNumber,
                OrganizerConnected = model.Data.OrganizerConnected,
                CoOrganizerConnected = model.Data.CoOrganizerConnected,
                OrganizerID = model.Data.OrganizerID,
                CoOrganizerID = model.Data.CoOrganizerID,
                OrgOlympicEnterID = model.Data.OrgOlympicEnterID
            };

            sql = SQLQuery.UpdateOlympic;
            if (model.Data.OlympicTypeProfileID == 0)
                sql = SQLQuery.InsertOlympic;

            WithTransaction(tx =>
            {
                if (model.Data.OlympicTypeProfileID == 0)
                    model.Data.OlympicTypeProfileID = tx.Query<int>(sql, args).Single();
                else
                    tx.Execute(sql, args);

                foreach (var item in newList)
                {
                    if (item.OlympicTypeProfileID == 0)
                        item.OlympicTypeProfileID = model.Data.OlympicTypeProfileID;

                    if (keys.Count > 0)
                    {
                        item.OlympicSubjectID = keys[0].OlympicSubjectID;
                        keys.Remove(keys[0]);

                        sql = "update OlympicSubject set SubjectID = @SubjectID " +
                        "where OlympicTypeProfileID = @OlympicTypeProfileID and OlympicSubjectID = @OlympicSubjectID";

                        tx.Execute(sql, new
                        {
                            OlympicTypeProfileID = item.OlympicTypeProfileID,
                            OlympicSubjectID = item.OlympicSubjectID,
                            SubjectID = item.SubjectID
                        });
                    }
                    else
                    {
                        sql = "insert into OlympicSubject (OlympicTypeProfileID, SubjectID) " +
                        "values (@OlympicTypeProfileID, @SubjectID)";

                        tx.Execute(sql, new
                        {
                            OlympicTypeProfileID = item.OlympicTypeProfileID,
                            SubjectID = item.SubjectID
                        });
                    }
                }

                foreach (var item in keys)
                {
                    sql = "delete from OlympicSubject where OlympicSubjectID = @OlympicSubjectID";

                    tx.Execute(sql, new
                    {
                        OlympicSubjectID = item.OlympicSubjectID,
                    });
                }

            });

            return true;
        }

        // удаление олимпиады
        public void OlympicDelete(int id)
        {
            var args = new
            {
                OlympicTypeProfileID = id,
            };

            WithTransaction(tx =>
            {
                var sql = "delete from OlympicSubject where OlympicTypeProfileID = @OlympicTypeProfileID";
                tx.Execute(sql, args);
                tx.Execute(SQLQuery.DeleteOlympic, args);
            });
        }

        public IEnumerable<OlympicTypeProfile> GetAllOlympicTypeProfile()
        {
            var sql =
                "select p.*, t.*,f.* from OlympicTypeProfile as p " +
                "left join OlympicType as t on p.OlympicTypeID = t.OlympicID " +
                "left join OlympicProfile as f on p.OlympicProfileID = f.OlympicProfileID";

            return DbConnection(db =>
            {
                return db.Query<OlympicTypeProfile, OlympicType, OlympicProfile, OlympicTypeProfile>(
                                sql, (p, t, f) =>
                                {
                                    p.OlympicType = t;
                                    p.OlympicProfile = f;
                                    return p;
                                }, splitOn: "OlympicID,OlympicProfileID");
            });
        }

        public OlympicTypeProfile GetOlympicTypeProfile(int id)
        {
            var sql =
                "select p.*, t.*,f.* from OlympicTypeProfile as p " +
                "left join OlympicType as t on p.OlympicTypeID = t.OlympicID " +
                "left join OlympicProfile as f on p.OlympicProfileID = f.OlympicProfileID " +
                "where OlympicTypeProfileID = @OlympicTypeProfileID ";

            return DbConnection(db =>
            {
                var subjects = db.Query<OlympicSubject, Subject, OlympicSubject>(
                    "select u.*, s.* from OlympicSubject u, Subject s where u.OlympicTypeProfileID = @OlympicTypeProfileID and u.SubjectID = s.SubjectID",
                    (u, s) =>
                    {
                        u.Subject = s;
                        return u;
                    },
                    splitOn: "SubjectID",
                    param: new
                    {
                        OlympicTypeProfileID = id
                    });


                return db.Query<OlympicTypeProfile, OlympicType, OlympicProfile, OlympicTypeProfile>(
                                sql, (p, t, f) =>
                                {
                                    p.OlympicType = t;
                                    p.OlympicProfile = f;
                                    p.OlympicSubject = subjects;
                                    return p;
                                },
                                splitOn: "OlympicID,OlympicProfileID",
                                param: new
                                {
                                    OlympicTypeProfileID = id
                                }).FirstOrDefault();
            });
        }

        // годы олимпиад
        public IEnumerable<OlympicType> GetOlympicTypeYears()
        {
            var sql = "select t.OlympicId, t.OlympicYear from OlympicType t";

            return DbConnection(db =>
            {
                return db.Query<OlympicType>(sql).DistinctBy(y => y.OlympicYear).OrderByDescending(y => y.OlympicYear);
            });
        }


        // олимпиады в определенный год
        public IEnumerable<OlympicType> GetOlympicTypeNamesByYear(int year)
        {
            var sql = "select t.OlympicId, t.Name from OlympicType t where t.OlympicYear = @OlympicYear";

            return DbConnection(db =>
            {
                return db.Query<OlympicType>(sql, param: new
                {
                    OlympicYear = year
                }).OrderBy(y => y.Name);
            });
        }


        // профили олимпиад
        public IEnumerable<OlympicProfile> GetAllOlympicProfile()
        {
            var sql = "select * from OlympicProfile";

            return DbConnection(db =>
            {
                return db.Query<OlympicProfile>(sql).OrderBy(y => y.ProfileName);
            });
        }

        // уровни олимпиад
        public IEnumerable<OlympicLevel> GetAllOlympicLevel()
        {
            var sql = "select * from OlympicLevel";

            return DbConnection(db =>
            {
                return db.Query<OlympicLevel>(sql).OrderBy(y => y.Name);
            });
        }


        // предметы по олимпиаде
        public IEnumerable<Subject> GetSubjectsByOlympicTypeProfile(int id)
        {
            var sql =
                "select * from Subject s, OlympicSubject u " +
                "where u.OlympicTypeProfileID=@OlympicTypeProfileID and s.SubjectID = u.SubjectID";

            return DbConnection(db =>
            {
                return db.Query<Subject>(sql, param: new
                {
                    OlympicTypeProfileID = id
                }).OrderBy(y => y.Name);
            });
        }

        // все предметы для олимпиады
        public IEnumerable<Subject> GetOlympicSubjects()
        {
            var sql = "select * from Subject s where s.IsOlympic = 1";

            return DbConnection(db =>
            {
                return db.Query<Subject>(sql).OrderBy(y => y.Name);
            });
        }

        // все предметы ЕГЭ
        public IEnumerable<Subject> GetEgeSubjects()
        {
            var sql = "select * from Subject s where s.IsEge = 1";

            return DbConnection(db =>
            {
                return db.Query<Subject>(sql).OrderBy(y => y.Name);
            });
        }


        // институты по фильтру
        public IEnumerable<GVUZ.Data.Model.Institution> GetInstitutionFilter(string filter)
        {
            filter = filter == null ? "a" : filter;

            var sql = "select top(15) InstitutionID, FullName, INN " +
                      "from Institution where " +
                      "(FullName like '%" + filter + "%') or (INN like '%" + filter + "%')";

            return DbConnection(db =>
            {
                return db.Query<GVUZ.Data.Model.Institution>(sql);
            });
        }


        // адрес института
        public GVUZ.Data.Model.Institution GetAddressForInstitution(int id)
        {
            var sql = "select Address from Institution where InstitutionID = @InstitutionID";

            return DbConnection(db =>
            {
                return db.Query<GVUZ.Data.Model.Institution>(sql, param: new
                {
                    InstitutionID = id
                }).FirstOrDefault();
            });
        }

        // адрес института
        public GVUZ.Data.Model.Institution GetFullNameInstitution(int id)
        {
            var sql = "select FullName from Institution where InstitutionID = @InstitutionID";

            return DbConnection(db =>
            {
                return db.Query<GVUZ.Data.Model.Institution>(sql, param: new
                {
                    InstitutionID = id
                }).FirstOrDefault();
            });
        }

        //-----------------------------------------------------------------------------------------------------

        public IEnumerable<OlympicTypeProfile> GetOlympicTypeProfileById(int id)
        {
            return DbConnection(db =>
            {
                return db.Query<OlympicTypeProfile, OlympicProfile, OlympicTypeProfile>(
                    "select p.*, f.* from OlympicTypeProfile as p " +
                    "left join OlympicProfile as f on p.OlympicProfileID = f.OlympicProfileID " +
                    "where p.OlympicTypeID = @OlympicTypeID",
                    (p, f) =>
                    {
                        p.OlympicProfile = f;
                        return p;
                    },
                    param: new
                    {
                        OlympicTypeID = id
                    },
                    splitOn: "OlympicID,OlympicProfileID");
            });
        }

        //-----------------------------------------------------------------------------------------------------

        public OlympicType GetOlympicTypeById(int id)
        {
            return DbConnection(db =>
            {
                var olympicType = db.Query<OlympicType>(
                    "select t.* from OlympicType as t where t.OlympicID = @OlympicID",
                    param: new
                    {
                        OlympicID = id
                    }).
                    FirstOrDefault();

                olympicType = olympicType ?? new OlympicType();
                olympicType.OlympicTypeProfile = GetOlympicTypeProfileById(id).AsList();

                return olympicType;
            });
        }

        //-----------------------------------------------------------------------------------------------------

        public IEnumerable<OlympicType> GetOlympicTypeAll(OlympicTypeEnum olympicTypeEnum = OlympicTypeEnum.All)
        {
            return DbConnection(db =>
            {
                var sql = "select * from OlympicType";

                if (olympicTypeEnum == OlympicTypeEnum.NotVosh)
                    sql = sql + " where OlympicNumber is not null";
                else
                if (olympicTypeEnum == OlympicTypeEnum.Vosh)
                    sql = sql + " where OlympicNumber is null";

                return db.Query<OlympicType>(sql);
            });
        }

        //-----------------------------------------------------------------------------------------------------

        public OlympicTypeProfile GetOlympicLevel(int id)
        {
            var sql =
                "select p.*, t.* from OlympicTypeProfile as p " +
                "left join OlympicLevel as t on p.OlympicLevelID = t.OlympicLevelID " +
                " where p.OlympicTypeProfileID = @OlympicTypeProfileID";

            return DbConnection(db =>
            {
                return db.Query<OlympicTypeProfile, OlympicLevel, OlympicTypeProfile>(
                    sql, (p, t) =>
                    {
                        p.OlympicLevel = t;
                        return p;
                    },
                    param: new
                    {
                        OlympicTypeProfileID = id
                    },
                    splitOn: "OlympicLevelID").FirstOrDefault();
            });
        }

        //-----------------------------------------------------------------------------------------------------

        public IEnumerable<CountryType> GetCountryTypeAll()
        {
            return DbConnection(db =>
            {
                var sql = "select * from CountryType";
                return db.Query<CountryType>(sql);
            });
        }

        //-----------------------------------------------------------------------------------------------------

        public IEnumerable<CompatriotCategory> GetCompatriotCategoryAll()
        {
            return DbConnection(db =>
            {
                var sql = "select * from CompatriotCategory";
                return db.Query<CompatriotCategory>(sql);
            });
        }

        //-----------------------------------------------------------------------------------------------------

        public IEnumerable<OrphanCategory> GetOrphanCategoryAll()
        {
            return DbConnection(db =>
            {
                var sql = "select * from OrphanCategory";
                return db.Query<OrphanCategory>(sql);
            });
        }

        //-----------------------------------------------------------------------------------------------------

        public IEnumerable<VeteranCategory> GetVeteranCategoryAll()
        {
            return DbConnection(db =>
            {
                var sql = "select * from VeteranCategory";
                return db.Query<VeteranCategory>(sql);
            });
        }


        //-----------------------------------------------------------------------------------------------------

        public IEnumerable<ParentsLostCategory> GetParentsLostCategoryAll()
        {
            return DbConnection(db =>
            {
                var sql = "select * from ParentsLostCategory";
                return db.Query<ParentsLostCategory>(sql);
            });
        }

        //-----------------------------------------------------------------------------------------------------

        public IEnumerable<RadiationWorkCategory> GetRadiationWorkCategoryAll()
        {
            return DbConnection(db =>
            {
                var sql = "select * from RadiationWorkCategory";
                return db.Query<RadiationWorkCategory>(sql);
            });
        }

        //-----------------------------------------------------------------------------------------------------

        public IEnumerable<StateEmployeeCategory> GetStateEmployeeCategoryAll()
        {
            return DbConnection(db =>
            {
                var sql = "select * from StateEmployeeCategory";
                return db.Query<StateEmployeeCategory>(sql);
            });
        }

        //-----------------------------------------------------------------------------------------------------

        public CountryType GetCountryTypeById(int? id)
        {
            //if (id == null)
            //{
            //    id = 0;
            //}
            return DbConnection(db =>
            {
                return db.Query<CountryType>(string.Format("select * from CountryType where CountryID = {0}", id )).FirstOrDefault();
            });
        }

        //-----------------------------------------------------------------------------------------------------

        public CompatriotCategory GetCompatriotCategoryById(int id)
        {
            return DbConnection(db =>
            {
                return db.Query<CompatriotCategory>("select * from CompatriotCategory where CompatriotCategoryID = " + id).FirstOrDefault();
            });
        }

        //-----------------------------------------------------------------------------------------------------

        public OrphanCategory GetOrphanCategoryById(int id)
        {
            return DbConnection(db =>
            {
                return db.Query<OrphanCategory>("select * from OrphanCategory where OrphanCategoryID = " + id).FirstOrDefault();
            });
        }
        
        //-----------------------------------------------------------------------------------------------------

        public VeteranCategory GetVeteranCategoryById(int id)
        {
            return DbConnection(db =>
            {
                return db.Query<VeteranCategory>("select * from VeteranCategory where VeteranCategoryID = " + id).FirstOrDefault();
            });
        }

        //-----------------------------------------------------------------------------------------------------

        public ParentsLostCategory GetParentsLostCategoryById(int id)
        {
            return DbConnection(db =>
            {
                return db.Query<ParentsLostCategory>("select * from ParentsLostCategory where ParentsLostCategoryID = " + id).FirstOrDefault();
            });
        }

        //-----------------------------------------------------------------------------------------------------

        public RadiationWorkCategory GetRadiationWorkCategoryById(int id)
        {
            return DbConnection(db =>
            {
                return db.Query<RadiationWorkCategory>("select * from RadiationWorkCategory where RadiationWorkCategoryID = " + id).FirstOrDefault();
            });
        }

        //-----------------------------------------------------------------------------------------------------

        public StateEmployeeCategory GetStateEmployeeCategoryById(int id)
        {
            return DbConnection(db =>
            {
                return db.Query<StateEmployeeCategory>("select * from StateEmployeeCategory where StateEmployeeCategoryID = " + id).FirstOrDefault();
            });
        }

        //-----------------------------------------------------------------------------------------------------

        public EducationForm GetEducationFormById(byte? id)
        {
            //if (id == null)
            //{
            //    id = 0;
            //}
            return DbConnection(db =>
            {
                return db.Query<EducationForm>(string.Format("select * from EducationForms where EducationFormID = {0}",id)).FirstOrDefault();
            });
        }

        //-----------------------------------------------------------------------------------------------------

        public IEnumerable<EducationForm> GetEducationFormsAll()
        {
            return DbConnection(db =>
            {
                var sql = "select * from EducationForms";
                return db.Query<EducationForm>(sql);
            });
        }

        //-----------------------------------------------------------------------------------------------------


        public SPResult CheckBenefitOlympic(int? entranceTestItemID, int? groupID, int? olympicTypeProfileID,
                int? diplomaTypeID, int? olympicID, int? classNumber, int? docID, out bool docUsedAsBenefit)
        {
            var res = new SPResult();

            var args = new DynamicParameters();

            args.Add("@entranceTestItemID", entranceTestItemID, DbType.Int32);
            args.Add("@groupID", groupID, DbType.Int32);
            args.Add("@olympicTypeProfileID", olympicTypeProfileID, DbType.Int32);
            args.Add("@diplomaTypeID", diplomaTypeID, DbType.Int32);
            args.Add("@olympicID", olympicID, DbType.Int32);
            args.Add("@formNumberID", classNumber, DbType.Int32);
            args.Add("@docID", docID, DbType.Int32);

            args.Add("@errorMessage", null, DbType.AnsiStringFixedLength, size: 4000, direction: ParameterDirection.Output);
            args.Add("@violationMessage", null, DbType.AnsiStringFixedLength, size: 4000, direction: ParameterDirection.Output);
            args.Add("@violationId", null, DbType.Int32, ParameterDirection.Output);
            args.Add("@docUsedAsBenefit", null, DbType.Boolean, ParameterDirection.Output);

            WithTransaction(tx =>
            {
                res.returnValue = tx.Query<bool>("ChectBenefitOlympic", args, commandType: CommandType.StoredProcedure).SingleOrDefault();
            });

            res.errorMessage = args.Get<string>("@errorMessage");
            res.violationMessage = args.Get<string>("@violationMessage");
            res.violationId = args.Get<int>("@violationId");

            docUsedAsBenefit = args.Get<bool>("@docUsedAsBenefit"); 

            if (!String.IsNullOrEmpty(res.errorMessage))
            {
                res.errorMessage = res.errorMessage.Trim();
            }

            return res;
        }

        //-----------------------------------------------------------------------------------------------------

        public SPResult CheckBenefitOlympic(int applicationId)
        {
            var res = new SPResult();

            var sql =
                "select edo.*, aetd.EntranceTestItemID, aetd.CompetitiveGroupID " +
                "from ApplicationEntrantDocument aed (NOLOCK) " +
                "inner join EntrantDocument ed (NOLOCK) on ed.EntrantDocumentId = aed.EntrantDocumentID " +
                "inner join EntrantDocumentOlympic edo (NOLOCK) on ed.EntrantDocumentId = edo.EntrantDocumentId " +
                "inner join ApplicationEntranceTestDocument aetd (NOLOCK) on aetd.EntrantDocumentID = ed.EntrantDocumentID " +
                "where aed.ApplicationId = @ApplicationId";

            var docs = DbConnection(db =>
            {
                return db.Query(sql, param: new { applicationId = applicationId });
            });

            foreach (var doc in docs)
            {
                bool temp;
                var er = CheckBenefitOlympic(doc.EntranceTestItemID, doc.CompetitiveGroupID, doc.OlympicTypeProfileID,
                        doc.DiplomaTypeID, doc.OlympicID, doc.ClassNumber, doc.EntrantDocumentID, out temp);

                if(er.violationId > 0)
                    return er;
            }

            return res;
        }


        //-----------------------------------------------------------------------------------------------------

        public DisabilityType GetDisabilityTypeById(int disabilityTypeID)
        {
            return DbConnection(db =>
            {
                return db.Query<DisabilityType>("select * from DisabilityType where DisabilityID = " + disabilityTypeID).FirstOrDefault();
            });
        }
        public IEnumerable<DisabilityType> GetDisabilityTypeAll()
        {
            return DbConnection(db =>
            {
                var sql = "select * from DisabilityType";
                return db.Query<DisabilityType>(sql);
            });
        }
    }
}



