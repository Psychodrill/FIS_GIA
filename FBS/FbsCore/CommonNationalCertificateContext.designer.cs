using System.Xml.Linq;

#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Fbs.Core
{
    using System.Data.Linq;
    using System.Data.Linq.Mapping;
    using System.Data;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Linq;
    using System.Linq.Expressions;
    using System.ComponentModel;
    using System;


    [global::System.Data.Linq.Mapping.DatabaseAttribute(Name = "fbs_db")]
    public partial class CommonNationalCertificateContext : System.Data.Linq.DataContext
    {

        private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();

        #region Extensibility Method Definitions
        partial void OnCreated();
        partial void InsertSubject(Subject instance);
        partial void UpdateSubject(Subject instance);
        partial void DeleteSubject(Subject instance);
        #endregion

        public CommonNationalCertificateContext() :
            base(global::Fbs.Core.Properties.Settings.Default.FbsConnectionString, mappingSource)
        {
            OnCreated();
        }

        public CommonNationalCertificateContext(string connection) :
            base(connection, mappingSource)
        {
            OnCreated();
        }

        public CommonNationalCertificateContext(System.Data.IDbConnection connection) :
            base(connection, mappingSource)
        {
            OnCreated();
        }

        public CommonNationalCertificateContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
            base(connection, mappingSource)
        {
            OnCreated();
        }

        public CommonNationalCertificateContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
            base(connection, mappingSource)
        {
            OnCreated();
        }

        public System.Data.Linq.Table<Subject> Subjects
        {
            get
            {
                return this.GetTable<Subject>();
            }
        }

        public System.Data.Linq.Table<CheckCommonNationalExamCertificateByNumberResult> CheckCommonNationalExamCertificateByNumberResults
        {
            get
            {
                return this.GetTable<CheckCommonNationalExamCertificateByNumberResult>();
            }
        }


        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.CheckCommonNationalExamCertificateByNumberForXml")]
        internal int CheckCommonNationalExamCertificateByNumberForXml(
            [Parameter(DbType = "NVarChar(255)")] string number,
            [Parameter(DbType = "NVarChar(4000)")] string checkSubjectMarks,
            [Parameter(DbType = "uniqueidentifier")] string ParticipantID,
            [Parameter(DbType = "NVarChar(255)")] string login,
            [Parameter(DbType = "NVarChar(255)")] string ip,
            [Parameter(DbType = "Bit")] bool shouldCheckMarks,
            [Parameter(DbType = "Xml")] ref XElement xml)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                number,
                checkSubjectMarks,
                ParticipantID,
                login,
                ip,
                shouldCheckMarks,
                xml);
            xml = (XElement)(result.GetParameterValue(6));
            return ((int)(result.ReturnValue));
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.CheckCommonNationalExamCertificateByPassportForXml")]
        internal int CheckCommonNationalExamCertificateByPassportForXml(
            [Parameter(DbType = "NVarChar(255)")] string passportSeria,
            [Parameter(DbType = "NVarChar(255)")] string passportNumber,
            [Parameter(DbType = "NVarChar(4000)")] string subjectMarks,
            [Parameter(DbType = "NVarChar(255)")] string login,
            [Parameter(DbType = "NVarChar(255)")] string ip,
            [Parameter(DbType = "Bit")] bool shouldWriteLog,
            [Parameter(DbType = "Xml")] ref XElement xml)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                passportSeria,
                passportNumber,
                subjectMarks,
                login,
                ip,                
                shouldWriteLog,
                xml);
            xml = (XElement)(result.GetParameterValue(6));
            return ((int)(result.ReturnValue));
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.SearchCommonNationalExamCertificateCheckByOuterId")]
        internal int SearchCommonNationalExamCertificateCheckByOuterId([Parameter(DbType = "BigInt")] long? batchId, [Parameter(DbType = "Xml")] ref XElement xml)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), batchId, xml);
            xml = (XElement)(result.GetParameterValue(1));
            return ((int)(result.ReturnValue));
        }


        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.UpdateCommonNationalExamCertificateCheckBatch")]
        internal int UpdateCommonNationalExamCertificateCheckBatch([Parameter(DbType = "BigInt")] ref long? id, [Parameter(DbType = "NVarChar(255)")] string login, [Parameter(DbType = "NVarChar(255)")] string ip, [Parameter(DbType = "NText")] string batch, [Parameter(DbType = "NVarChar(255)")] string filter, [Parameter(DbType = "Int")] int type, [Parameter(DbType = "BigInt")] long? outerId, [Parameter(DbType = "Int")] int? year)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id, login, ip, batch, filter, type, outerId, year);
            id = ((System.Nullable<long>)(result.GetParameterValue(0)));
            return ((int)(result.ReturnValue));
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.UpdateCommonNationalExamCertificateRequestBatch")]
        internal int UpdateCommonNationalExamCertificateRequestBatch([global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "BigInt")] ref System.Nullable<long> id, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "NVarChar(255)")] string login, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "NVarChar(255)")] string ip, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "NText")] string batch, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "NVarChar(255)")] string filter, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "Bit")] System.Nullable<bool> IsTypographicNumber, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "NVarChar(10)")] string year)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id, login, ip, batch, filter, IsTypographicNumber, year);
            id = ((System.Nullable<long>)(result.GetParameterValue(0)));
            return ((int)(result.ReturnValue));
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.ExecuteCommonNationalExamCertificateLoadingTask")]
        internal int ExecuteCommonNationalExamCertificateLoadingTask([global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "BigInt")] System.Nullable<long> id, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "NVarChar(255)")] string editorLogin, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "NVarChar(255)")] string editorIp)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id, editorLogin, editorIp);
            return ((int)(result.ReturnValue));
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.ExecuteCommonNationalExamCertificateDenyLoadingTask")]
        internal int ExecuteCommonNationalExamCertificateDenyLoadingTask([global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "BigInt")] System.Nullable<long> id, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "NVarChar(255)")] string editorLogin, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "NVarChar(255)")] string editorIp)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id, editorLogin, editorIp);
            return ((int)(result.ReturnValue));
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.UpdateEntrantCheckBatch")]
        internal int UpdateEntrantCheckBatch([global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "BigInt")] ref System.Nullable<long> id, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "NVarChar(255)")] string login, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "NVarChar(255)")] string ip, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "NText")] string batch)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id, login, ip, batch);
            id = ((System.Nullable<long>)(result.GetParameterValue(0)));
            return ((int)(result.ReturnValue));
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.UpdateSchoolLeavingCertificateCheckBatch")]
        internal int UpdateSchoolLeavingCertificateCheckBatch([global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "BigInt")] ref System.Nullable<long> id, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "NVarChar(255)")] string login, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "NVarChar(255)")] string ip, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "NText")] string batch)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id, login, ip, batch);
            id = ((System.Nullable<long>)(result.GetParameterValue(0)));
            return ((int)(result.ReturnValue));
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.UpdateCompetitionCertificateRequestBatch")]
        internal int UpdateCompetitionCertificateRequestBatch([global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "BigInt")] ref System.Nullable<long> id, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "NVarChar(255)")] string login, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "NVarChar(255)")] string ip, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "NText")] string batch)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id, login, ip, batch);
            id = ((System.Nullable<long>)(result.GetParameterValue(0)));
            return ((int)(result.ReturnValue));
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.GetSubject")]
        internal ISingleResult<Subject> GetSubject()
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())));
            return ((ISingleResult<Subject>)(result.ReturnValue));
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.CheckCommonNationalExamCertificateByNumber")]
        internal ISingleResult<CheckCommonNationalExamCertificateByNumberResult> CheckCommonNationalExamCertificateByNumber([global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "NVarChar(255)")] string number, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "Bit")] System.Nullable<bool> isOriginal, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "NVarChar(255)")] string checkLastName, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "NVarChar(255)")] string checkFirstName, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "NVarChar(255)")] string checkPatronymicName, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "NVarChar(4000)")] string checkSubjectMarks, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "NVarChar(255)")] string login, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "NVarChar(255)")] string ip)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), number, isOriginal, checkLastName, checkFirstName, checkPatronymicName, checkSubjectMarks, login, ip);
            return ((ISingleResult<CheckCommonNationalExamCertificateByNumberResult>)(result.ReturnValue));
        }
    }

    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.Subject")]
    public partial class Subject : INotifyPropertyChanging, INotifyPropertyChanged
    {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private int _Id;

        private string _Code;

        private string _Name;

        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnIdChanging(int value);
        partial void OnIdChanged();
        partial void OnCodeChanging(string value);
        partial void OnCodeChanged();
        partial void OnNameChanging(string value);
        partial void OnNameChanged();
        #endregion

        public Subject()
        {
            OnCreated();
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                if ((this._Id != value))
                {
                    this.OnIdChanging(value);
                    this.SendPropertyChanging();
                    this._Id = value;
                    this.SendPropertyChanged("Id");
                    this.OnIdChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Code", DbType = "NVarChar(255) NOT NULL", CanBeNull = false)]
        public string Code
        {
            get
            {
                return this._Code;
            }
            set
            {
                if ((this._Code != value))
                {
                    this.OnCodeChanging(value);
                    this.SendPropertyChanging();
                    this._Code = value;
                    this.SendPropertyChanged("Code");
                    this.OnCodeChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Name", DbType = "NVarChar(255) NOT NULL", CanBeNull = false)]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                if ((this._Name != value))
                {
                    this.OnNameChanging(value);
                    this.SendPropertyChanging();
                    this._Name = value;
                    this.SendPropertyChanged("Name");
                    this.OnNameChanged();
                }
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [global::System.Data.Linq.Mapping.TableAttribute(Name = "")]
    public partial class CheckCommonNationalExamCertificateByNumberResult
    {

        private string _Number;

        private bool _IsOriginal;

        private string _CheckLastName;

        private string _LastName;

        private bool _LastNameIsCorrect;

        private string _CheckFirstName;

        private string _FirstName;

        private bool _FirstNameIsCorrect;

        private string _CheckPatronymicName;

        private string _PatronymicName;

        private bool _PatronymicNameIsCorrect;

        private bool _IsExist;

        private int _SubjectCode;

        private string _SubjectName;

        private System.Nullable<int> _CheckSubjecMark;

        private System.Nullable<int> _SubjectMark;

        private bool _SubjectMarkIsCorrect;

        private System.Nullable<bool> _HasAppeal;

        private bool _IsDeny;

        private string _DenyComment;

        private string _DenyNewCertificateNumber;

        private string _PassportSeria;

        private string _PassportNumber;

        private System.Nullable<int> _RegionId;

        private string _RegionName;

        public CheckCommonNationalExamCertificateByNumberResult()
        {
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Number", DbType = "NVarChar(255)", CanBeNull = false)]
        public string Number
        {
            get
            {
                return this._Number;
            }
            set
            {
                if ((this._Number != value))
                {
                    this._Number = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_IsOriginal", DbType = "bit")]
        public bool IsOriginal
        {
            get
            {
                return this._IsOriginal;
            }
            set
            {
                if ((this._IsOriginal != value))
                {
                    this._IsOriginal = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CheckLastName", DbType = "NVarChar(255)")]
        public string CheckLastName
        {
            get
            {
                return this._CheckLastName;
            }
            set
            {
                if ((this._CheckLastName != value))
                {
                    this._CheckLastName = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LastName", DbType = "NVarChar(255)")]
        public string LastName
        {
            get
            {
                return this._LastName;
            }
            set
            {
                if ((this._LastName != value))
                {
                    this._LastName = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LastNameIsCorrect", DbType = "bit")]
        public bool LastNameIsCorrect
        {
            get
            {
                return this._LastNameIsCorrect;
            }
            set
            {
                if ((this._LastNameIsCorrect != value))
                {
                    this._LastNameIsCorrect = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CheckFirstName", DbType = "NVarChar(255)")]
        public string CheckFirstName
        {
            get
            {
                return this._CheckFirstName;
            }
            set
            {
                if ((this._CheckFirstName != value))
                {
                    this._CheckFirstName = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FirstName", DbType = "NVarChar(255)")]
        public string FirstName
        {
            get
            {
                return this._FirstName;
            }
            set
            {
                if ((this._FirstName != value))
                {
                    this._FirstName = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FirstNameIsCorrect", DbType = "Bit")]
        public bool FirstNameIsCorrect
        {
            get
            {
                return this._FirstNameIsCorrect;
            }
            set
            {
                if ((this._FirstNameIsCorrect != value))
                {
                    this._FirstNameIsCorrect = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CheckPatronymicName", DbType = "NVarChar(255)")]
        public string CheckPatronymicName
        {
            get
            {
                return this._CheckPatronymicName;
            }
            set
            {
                if ((this._CheckPatronymicName != value))
                {
                    this._CheckPatronymicName = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PatronymicName", DbType = "NVarChar(255)")]
        public string PatronymicName
        {
            get
            {
                return this._PatronymicName;
            }
            set
            {
                if ((this._PatronymicName != value))
                {
                    this._PatronymicName = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PatronymicNameIsCorrect", DbType = "Bit")]
        public bool PatronymicNameIsCorrect
        {
            get
            {
                return this._PatronymicNameIsCorrect;
            }
            set
            {
                if ((this._PatronymicNameIsCorrect != value))
                {
                    this._PatronymicNameIsCorrect = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_IsExist", DbType = "Bit")]
        public bool IsExist
        {
            get
            {
                return this._IsExist;
            }
            set
            {
                if ((this._IsExist != value))
                {
                    this._IsExist = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SubjectCode", DbType = "int")]
        public int SubjectId
        {
            get
            {
                return this._SubjectCode;
            }
            set
            {
                if ((this._SubjectCode != value))
                {
                    this._SubjectCode = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SubjectName", DbType = "NVarChar(255)", CanBeNull = false)]
        public string SubjectName
        {
            get
            {
                return this._SubjectName;
            }
            set
            {
                if ((this._SubjectName != value))
                {
                    this._SubjectName = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CheckSubjecMark", DbType = "int")]
        public System.Nullable<int> CheckSubjectMark
        {
            get
            {
                return this._CheckSubjecMark;
            }
            set
            {
                if ((this._CheckSubjecMark != value))
                {
                    this._CheckSubjecMark = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SubjectMark", DbType = "int")]
        public System.Nullable<int> SubjectMark
        {
            get
            {
                return this._SubjectMark;
            }
            set
            {
                if ((this._SubjectMark != value))
                {
                    this._SubjectMark = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SubjectMarkIsCorrect", DbType = "Bit")]
        public bool SubjectMarkIsCorrect
        {
            get
            {
                return this._SubjectMarkIsCorrect;
            }
            set
            {
                if ((this._SubjectMarkIsCorrect != value))
                {
                    this._SubjectMarkIsCorrect = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_HasAppeal", DbType = "Bit")]
        public System.Nullable<bool> HasAppeal
        {
            get
            {
                return this._HasAppeal;
            }
            set
            {
                if ((this._HasAppeal != value))
                {
                    this._HasAppeal = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_IsDeny", DbType = "Bit")]
        public bool IsDeny
        {
            get
            {
                return this._IsDeny;
            }
            set
            {
                if ((this._IsDeny != value))
                {
                    this._IsDeny = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_DenyComment", DbType = "NText")]
        public string DenyComment
        {
            get
            {
                return this._DenyComment;
            }
            set
            {
                if ((this._DenyComment != value))
                {
                    this._DenyComment = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_DenyNewCertificateNumber", DbType = "NVarChar(255)")]
        public string DenyNewCertificateNumber
        {
            get
            {
                return this._DenyNewCertificateNumber;
            }
            set
            {
                if ((this._DenyNewCertificateNumber != value))
                {
                    this._DenyNewCertificateNumber = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PassportSeria", DbType = "NVarChar(255)")]
        public string PassportSeria
        {
            get
            {
                return this._PassportSeria;
            }
            set
            {
                if ((this._PassportSeria != value))
                {
                    this._PassportSeria = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PassportNumber", DbType = "NVarChar(255)")]
        public string PassportNumber
        {
            get
            {
                return this._PassportNumber;
            }
            set
            {
                if ((this._PassportNumber != value))
                {
                    this._PassportNumber = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_RegionId", DbType = "int")]
        public System.Nullable<int> RegionId
        {
            get
            {
                return this._RegionId;
            }
            set
            {
                if ((this._RegionId != value))
                {
                    this._RegionId = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_RegionName", DbType = "NVarChar(255)")]
        public string RegionName
        {
            get
            {
                return this._RegionName;
            }
            set
            {
                if ((this._RegionName != value))
                {
                    this._RegionName = value;
                }
            }
        }
    }
}
#pragma warning restore 1591
