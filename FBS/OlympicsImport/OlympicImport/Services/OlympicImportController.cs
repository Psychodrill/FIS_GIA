using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using LumenWorks.Framework.IO.Csv;

namespace OlympicImport.Services
{
    public class OlympicImportController
    {
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly FileInfo _importFile;
        private readonly OlympicRepository _olympicRepository;
        private readonly OlympicSubjectRepository _subjectRepository;
        private SqlTransaction _tx;
        private SqlCommand _insertDiplomant;
        private SqlCommand _insertParticipant;
        private readonly HashSet<long> _diplomantId = new HashSet<long>(); 

        public OlympicImportController(IConnectionStringProvider connectionStringProvider, FileInfo importFile)
        {
            _connectionStringProvider = connectionStringProvider;
            _importFile = importFile;
            _olympicRepository = new OlympicRepository(_connectionStringProvider);
            _subjectRepository = new OlympicSubjectRepository(_connectionStringProvider);
        }

        public void CleanTables(bool removeSubjects, bool removeDiplomants, bool removeOlympics)
        {
            using (SqlConnection cn = new SqlConnection(_connectionStringProvider.ConnectionString))
            {
                cn.Open();

                using (SqlTransaction tx = cn.BeginTransaction())
                {
                    try
                    {
                        if (removeOlympics || removeDiplomants)
                        {
                            using (SqlCommand cmd = cn.CreateCommand())
                            {
                                cmd.Transaction = tx;
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = "delete from [dbo].[OlympicParticipants]";
                                cmd.ExecuteNonQuery();
                            }

                            if (removeDiplomants)
                            {
                                using (SqlCommand cmd = cn.CreateCommand())
                                {
                                    cmd.Transaction = tx;
                                    cmd.CommandType = CommandType.Text;
                                    cmd.CommandText = "delete from [dbo].[OlympicDiplomants]";
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }

                        if (removeOlympics)
                        {
                            using (SqlCommand cmd = cn.CreateCommand())
                            {
                                cmd.Transaction = tx;
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = "delete from [dbo].[Olympics]";
                                cmd.ExecuteNonQuery();
                            }
                        }

                        if (removeSubjects)
                        {
                            using (SqlCommand cmd = cn.CreateCommand())
                            {
                                cmd.Transaction = tx;
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = "delete from [dbo].[OlympicSubjects]";
                                cmd.ExecuteNonQuery();
                            }
                        }

                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Run()
        {
            using (var fileReader = _importFile.OpenText())
            {
                using (SqlConnection cn = new SqlConnection(_connectionStringProvider.ConnectionString))
                {
                    cn.Open();

                    // load existing diplomants
                    _diplomantId.Clear();
                    using (SqlTransaction tx = cn.BeginTransaction())
                    {
                        using (SqlCommand cmd = cn.CreateCommand())
                        {
                            cmd.Transaction = tx;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = @"select id from [dbo].[OlympicDiplomants]";

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    _diplomantId.Add(reader.GetInt64(0));
                                }
                            }
                        }

                        tx.Commit();
                    }

                    using (var csv = new CsvReader(fileReader, true, '#', '"', '\\', '#', ValueTrimmingOptions.None))
                    {
                        int lineCount = 0;
                        const int batchSize = 100;

                        //string[] hdr = csv.GetFieldHeaders();

                        while (!csv.EndOfStream)
                        {    
                            if (csv.ReadNextRecord())
                            {
                                if ((lineCount%batchSize) == 0)
                                {
                                    if (_insertDiplomant != null)
                                    {
                                        _insertDiplomant.Dispose();
                                        _insertDiplomant = null;
                                    }

                                    if (_insertParticipant != null)
                                    {
                                        _insertParticipant.Dispose();
                                        _insertParticipant = null;
                                    }

                                    if (_tx != null)
                                    {
                                        _tx.Commit();
                                        _tx.Dispose();
                                        _tx = null;
                                    }
                                }

                                if (_tx == null)
                                {
                                    _tx = cn.BeginTransaction(IsolationLevel.ReadCommitted);
                                    _insertDiplomant = CreateInsertDiplomantCommand();
                                    _insertDiplomant.Connection = cn;
                                    _insertDiplomant.Transaction = _tx;
                                    _insertDiplomant.Prepare();
                                    _insertParticipant = CreateInsertParticipantCommand();
                                    _insertParticipant.Transaction = _tx;
                                    _insertParticipant.Connection = cn;
                                    _insertParticipant.Prepare();
                                }

                                ProcessCsvRecord(csv);

                                lineCount++;
                            }
                        }

                        if (_insertDiplomant != null)
                        {
                            _insertDiplomant.Dispose();
                            _insertDiplomant = null;
                        }

                        if (_insertParticipant != null)
                        {
                            _insertParticipant.Dispose();
                            _insertParticipant = null;
                        }

                        if (_tx != null)
                        {
                            _tx.Commit();
                            _tx.Dispose();
                            _tx = null;
                        }
                    }    
                }
            }
        }

        private void ProcessCsvRecord(CsvReader csv)
        {
            string olympicCodeName = csv[OlympicsImportSchema.CodeName];
            OlympicsCsvRecord olympic = _olympicRepository.GetOrAdd(olympicCodeName, s => CreateOlympicRecord(csv));
            OlympicDiplomantCsvRecord diplomant = OlympicDiplomantCsvRecord.ParseCsv(csv);

            if (_diplomantId.Contains(diplomant.Id))
            {
                return;
            }
            
            //_diplomantId.Add(diplomant.Id);

            _insertDiplomant.Parameters["@id"].Value = diplomant.Id;
            _insertDiplomant.Parameters["@olympicsId"].Value = olympic.Id;
            _insertDiplomant.Parameters["@lastName"].Value = diplomant.LastName ?? (object)DBNull.Value;
            _insertDiplomant.Parameters["@firstName"].Value = diplomant.FirstName ?? (object) DBNull.Value;
            _insertDiplomant.Parameters["@middleName"].Value = diplomant.MiddleName ?? (object)DBNull.Value;
            _insertDiplomant.Parameters["@birthDate"].Value = diplomant.BirthDate.HasValue ? diplomant.BirthDate.Value.Date : (object)DBNull.Value;
            _insertDiplomant.Parameters["@schoolRegion"].Value = diplomant.SchoolRegion ?? (object) DBNull.Value;
            _insertDiplomant.Parameters["@schoolEgeCode"].Value = diplomant.SchoolEgeCode.HasValue ? diplomant.SchoolEgeCode.Value : (object)DBNull.Value;
            _insertDiplomant.Parameters["@schoolEgeName"].Value = diplomant.SchoolEgeName ?? (object) DBNull.Value;
            _insertDiplomant.Parameters["@formNumber"].Value = diplomant.FormNumber.HasValue ? diplomant.FormNumber.Value : (object)DBNull.Value;
            _insertDiplomant.Parameters["@regCode"].Value = diplomant.RegCode ?? (object) DBNull.Value;
            _insertDiplomant.Parameters["@resultLevel"].Value = diplomant.ResultLevel.HasValue ? diplomant.ResultLevel.Value : (object)DBNull.Value;
            _insertDiplomant.Parameters["@egeId"].Value = diplomant.EgeId ?? (object) DBNull.Value;

            _insertDiplomant.ExecuteNonQuery();

            foreach (Guid participantId in diplomant.ParticipantId)
            {
                _insertParticipant.Parameters["@diplomantId"].Value = diplomant.Id;
                _insertParticipant.Parameters["@participantId"].Value = participantId;
                _insertParticipant.ExecuteNonQuery();
            }
        }

        private OlympicsCsvRecord CreateOlympicRecord(CsvReader csv)
        {
            OlympicsCsvRecord record = OlympicsCsvRecord.ParseCsv(csv);
            
            for (int i = 0; i < record.OlympicThemeSubjects.Count; i++)
            {
                record.OlympicThemeSubjects[i] = _subjectRepository.GetOrCreate(record.OlympicThemeSubjects[i].SubjectName);
            }

            for (int i = 0; i < record.OlympicProfileSubjects.Count; i++)
            {
                record.OlympicProfileSubjects[i] = _subjectRepository.GetOrCreate(record.OlympicProfileSubjects[i].SubjectName);
            }

            return record;
        }

        private static SqlCommand CreateInsertDiplomantCommand()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"insert into [dbo].[OlympicDiplomants](id, olympics_id, last_name, first_name, middle_name, birth_date, school_region, school_ege_code, school_ege_name, form_number, reg_code, result_level, egeid) values(@id, @olympicsId, @lastName, @firstName, @middleName, @birthDate, @schoolRegion, @schoolEgeCode, @schoolEgeName, @formNumber, @regCode, @resultLevel, @egeId)";

            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.BigInt, 64)).Value = DBNull.Value;
            cmd.Parameters.Add(new SqlParameter("@olympicsId", SqlDbType.BigInt, 64)).Value = DBNull.Value;
            cmd.Parameters.Add(new SqlParameter("@lastName", SqlDbType.VarChar, 64)).Value = DBNull.Value;
            cmd.Parameters.Add(new SqlParameter("@firstName", SqlDbType.VarChar, 64)).Value = DBNull.Value;
            cmd.Parameters.Add(new SqlParameter("@middleName", SqlDbType.VarChar, 64)).Value = DBNull.Value;
            cmd.Parameters.Add(new SqlParameter("@birthDate", SqlDbType.Date, 10)).Value = DBNull.Value;
            cmd.Parameters.Add(new SqlParameter("@schoolRegion", SqlDbType.VarChar, 32)).Value = DBNull.Value;
            cmd.Parameters.Add(new SqlParameter("@schoolEgeCode", SqlDbType.BigInt, 64)).Value = DBNull.Value;
            cmd.Parameters.Add(new SqlParameter("@schoolEgeName", SqlDbType.VarChar, 512)).Value = DBNull.Value;
            cmd.Parameters.Add(new SqlParameter("@formNumber", SqlDbType.Int, 32)).Value = DBNull.Value;
            cmd.Parameters.Add(new SqlParameter("@regCode", SqlDbType.VarChar, 32)).Value = DBNull.Value;
            cmd.Parameters.Add(new SqlParameter("@resultLevel", SqlDbType.Int, 32)).Value = DBNull.Value;
            cmd.Parameters.Add(new SqlParameter("@egeId", SqlDbType.VarChar, 1024)).Value = DBNull.Value;

            return cmd;
        }

        private static SqlCommand CreateInsertParticipantCommand()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"insert into [dbo].[OlympicParticipants](diplomant_id, participant_id) values(@diplomantId, @participantId)";

            cmd.Parameters.Add(new SqlParameter("@diplomantId", SqlDbType.BigInt, 64)).Value = DBNull.Value;
            cmd.Parameters.Add(new SqlParameter("@participantId", SqlDbType.UniqueIdentifier, 40)).Value = DBNull.Value;

            return cmd;
        }
    }
}