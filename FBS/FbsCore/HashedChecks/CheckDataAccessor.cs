using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Fbs.Core.CNEChecks;

namespace Fbs.Core.HashedChecks
{
    /// <summary>
    /// Класс доступа к обезличенным данным (для проверки свидетельств)
    /// </summary>
    public static class CheckDataAccessor
    {
        private const int CmdTimeout = 90;
        /// <summary>
        /// Проверка по номеру свидетельства и ФИО
        /// </summary>
        /// <param name="number"></param>
        /// <param name="lastName"></param>
        /// <param name="firstName"></param>
        /// <param name="patronymicName"></param>
        /// <param name="subjectMarks">Заявленные баллы</param>
        /// <returns></returns>
        public static DataTable CheckByNumber(string number, string lastName, string firstName, string patronymicName, string subjectMarks)
        {
            // return CheckByTGNOrNumber(number, null, lastName, firstName, patronymicName, subjectMarks);
            return CheckByCNENumber(number, lastName, firstName, patronymicName, subjectMarks);
        }

        /// <summary>
        /// Проверка по типографскому номеру
        /// </summary>
        /// <param name="typographicNumber"></param>
        /// <param name="lastName"></param>
        /// <param name="firstName"></param>
        /// <param name="patronymicName"></param>
        /// <param name="subjectMarks">Заявленные баллы</param>
        /// <returns></returns>
        public static DataTable CheckByTypographicNumber(string typographicNumber, string lastName, string firstName, string patronymicName, string subjectMarks)
        {
            return CheckByTGN(typographicNumber, lastName, firstName, patronymicName, subjectMarks);
        }

        static DataTable CheckByTGN(string typographicNumber, string lastName, string firstName, string patronymicName, string subjectMarks)
        {
            DataTable Result = new DataTable();
            using (SqlConnection Conn = new SqlConnection(DBSettings.ConnectionString_ForHashedBase))
            {
                Conn.Open();

                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandTimeout = CmdTimeout;
                Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                Cmd.CommandText = "dbo.CheckCNEByTyphNumber";


                if (!String.IsNullOrEmpty(typographicNumber))
                {
                    SqlParameter TGNumberParam = new SqlParameter("@checkTypographicNumber", System.Data.SqlDbType.NVarChar);
                    TGNumberParam.Value = typographicNumber;
                    Cmd.Parameters.Add(TGNumberParam);
                }

                SqlParameter LastNameParam = new SqlParameter("@checkLastName", System.Data.SqlDbType.NVarChar);
                LastNameParam.Value = lastName;
                Cmd.Parameters.Add(LastNameParam);

                if (!String.IsNullOrEmpty(firstName))
                {
                    SqlParameter FirstNameParam = new SqlParameter("@checkFirstName", System.Data.SqlDbType.NVarChar);
                    FirstNameParam.Value = firstName;
                    Cmd.Parameters.Add(FirstNameParam);
                }

                if (!String.IsNullOrEmpty(patronymicName))
                {
                    SqlParameter PatronymicNameParam = new SqlParameter("@checkPatronymicName", System.Data.SqlDbType.NVarChar);
                    PatronymicNameParam.Value = patronymicName;
                    Cmd.Parameters.Add(PatronymicNameParam);
                }

                if (!String.IsNullOrEmpty(subjectMarks))
                {
                    SqlParameter SubjectMarksParam = new SqlParameter("@checkSubjectMarks", System.Data.SqlDbType.NVarChar);
                    SubjectMarksParam.Value = subjectMarks;
                    Cmd.Parameters.Add(SubjectMarksParam);
                }

                Result.Load(Cmd.ExecuteReader());

                Conn.Close();
            }
            return Result;
        }

        static DataTable CheckByCNENumber(string number, string lastName, string firstName, string patronymicName, string subjectMarks)
        {
            DataTable Result = new DataTable();
            using (SqlConnection Conn = new SqlConnection(DBSettings.ConnectionString_ForHashedBase))
            {
                Conn.Open();

                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandTimeout = CmdTimeout;
                Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                Cmd.CommandText = "dbo.CheckCNEByNumber";

                if (!String.IsNullOrEmpty(number))
                {
                    SqlParameter NumberParam = new SqlParameter("@number", System.Data.SqlDbType.NVarChar);
                    NumberParam.Value = number;
                    Cmd.Parameters.Add(NumberParam);
                }

                SqlParameter LastNameParam = new SqlParameter("@checkLastName", System.Data.SqlDbType.NVarChar);
                LastNameParam.Value = lastName;
                Cmd.Parameters.Add(LastNameParam);

                if (!String.IsNullOrEmpty(firstName))
                {
                    SqlParameter FirstNameParam = new SqlParameter("@checkFirstName", System.Data.SqlDbType.NVarChar);
                    FirstNameParam.Value = firstName;
                    Cmd.Parameters.Add(FirstNameParam);
                }

                if (!String.IsNullOrEmpty(patronymicName))
                {
                    SqlParameter PatronymicNameParam = new SqlParameter("@checkPatronymicName", System.Data.SqlDbType.NVarChar);
                    PatronymicNameParam.Value = patronymicName;
                    Cmd.Parameters.Add(PatronymicNameParam);
                }

                if (!String.IsNullOrEmpty(subjectMarks))
                {
                    SqlParameter SubjectMarksParam = new SqlParameter("@checkSubjectMarks", System.Data.SqlDbType.NVarChar);
                    SubjectMarksParam.Value = subjectMarks;
                    Cmd.Parameters.Add(SubjectMarksParam);
                }

                Result.Load(Cmd.ExecuteReader());

                Conn.Close();
            }
            return Result;
        }

        /// <summary>
        /// Проверка по серии и номеру паспотра и ФИО
        /// </summary>
        /// <param name="seria"></param>
        /// <param name="number"></param>
        /// <param name="lastName"></param>
        /// <param name="firstName"></param>
        /// <param name="patronymicName"></param>
        /// <returns></returns>
        public static DataTable CheckByPassport(string seria, string number, string lastName, string firstName, string patronymicName)
        {
            DataTable Result = new DataTable();
            using (SqlConnection Conn = new SqlConnection(DBSettings.ConnectionString_ForHashedBase))
            {
                Conn.Open();

                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandTimeout = CmdTimeout;
                Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                Cmd.CommandText = "dbo.CheckCNEByPassport";

                if (!String.IsNullOrEmpty(number))
                {
                    SqlParameter NumberParam = new SqlParameter("@passportNumber", System.Data.SqlDbType.NVarChar);
                    NumberParam.Value = number;
                    Cmd.Parameters.Add(NumberParam);
                }

                if (!String.IsNullOrEmpty(seria))
                {
                    SqlParameter SeriaParam = new SqlParameter("@passportSeria", System.Data.SqlDbType.NVarChar);
                    SeriaParam.Value = seria;
                    Cmd.Parameters.Add(SeriaParam);
                }

                SqlParameter LastNameParam = new SqlParameter("@lastName", System.Data.SqlDbType.NVarChar);
                LastNameParam.Value = lastName;
                Cmd.Parameters.Add(LastNameParam);

                if (!String.IsNullOrEmpty(firstName))
                {
                    SqlParameter FirstNameParam = new SqlParameter("@firstName", System.Data.SqlDbType.NVarChar);
                    FirstNameParam.Value = firstName;
                    Cmd.Parameters.Add(FirstNameParam);
                }

                if (!String.IsNullOrEmpty(patronymicName))
                {
                    SqlParameter PatronymicNameParam = new SqlParameter("@patronymicName", System.Data.SqlDbType.NVarChar);
                    PatronymicNameParam.Value = patronymicName;
                    Cmd.Parameters.Add(PatronymicNameParam);
                }

                Result.Load(Cmd.ExecuteReader());

                Conn.Close();
            }
            return Result;
        }

        public static KeyValuePair<int,CheckByMarkSumResult> CheckByMarkSum(string firstName, string lastName, string patronymicName, string subjects, int sum, int orgId)
        {
            using (SqlConnection Conn = new SqlConnection(DBSettings.ConnectionString_ForHashedBase))
            {
                Conn.Open();

                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandTimeout = CmdTimeout;
                Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                Cmd.CommandText = "dbo.AddCheckByMarkSumEvent";
                SqlParameter LastNameParam = new SqlParameter("@LastName", System.Data.SqlDbType.NVarChar);
                LastNameParam.Value = lastName;
                Cmd.Parameters.Add(LastNameParam);


                SqlParameter FirstNameParam = new SqlParameter("@FirstName", System.Data.SqlDbType.NVarChar);
                FirstNameParam.Value = firstName;
                Cmd.Parameters.Add(FirstNameParam);


                if (!String.IsNullOrEmpty(patronymicName))
                {
                    SqlParameter PatronymicNameParam = new SqlParameter("@PatronymicName", System.Data.SqlDbType.NVarChar);
                    PatronymicNameParam.Value = patronymicName;
                    Cmd.Parameters.Add(PatronymicNameParam);
                }

                SqlParameter MarkSum = new SqlParameter("@SumMark", System.Data.SqlDbType.NVarChar);
                MarkSum.Value = sum;
                Cmd.Parameters.Add(MarkSum);

                SqlParameter ListSubject = new SqlParameter("@ListSubject", System.Data.SqlDbType.NVarChar);
                ListSubject.Value = subjects;
                Cmd.Parameters.Add(ListSubject);
                SqlParameter OrgId = new SqlParameter("@OrgId", System.Data.SqlDbType.Int);
                OrgId.Value = orgId;
                Cmd.Parameters.Add(OrgId);
               
                int checkId=Int32.Parse(Cmd.ExecuteScalar().ToString());

                Cmd.Dispose();
                Cmd = Conn.CreateCommand();
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = "dbo.CheckCNEByMarkSum";
                Cmd.Parameters.Add(new SqlParameter("@EventId", checkId));
                return new KeyValuePair<int, CheckByMarkSumResult>(checkId, (CheckByMarkSumResult)Cmd.ExecuteScalar());
            }
        }
    }
}