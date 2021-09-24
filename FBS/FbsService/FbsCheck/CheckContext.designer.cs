﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FbsService.FbsCheck
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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="fbs_check_db")]
	internal partial class CheckContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void DeleteCNECheckBatch(CNECheckBatch instance);
    partial void DeleteCNERequestBatch(CNERequestBatch instance);
    partial void InsertCNEForm(CNEForm instance);
    partial void UpdateCNEForm(CNEForm instance);
    partial void DeleteCNEForm(CNEForm instance);
    #endregion
		
		public CheckContext() : 
				base(global::FbsService.Properties.Settings.Default.FbsCheckConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public CheckContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public CheckContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public CheckContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public CheckContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<CNECheckBatch> CNECheckBatches
		{
			get
			{
				return this.GetTable<CNECheckBatch>();
			}
		}
		
		public System.Data.Linq.Table<CNERequestBatch> CNERequestBatches
		{
			get
			{
				return this.GetTable<CNERequestBatch>();
			}
		}
		
		public System.Data.Linq.Table<CNEForm> CNEForms
		{
			get
			{
				return this.GetTable<CNEForm>();
			}
		}
		
		private void InsertCNECheckBatch(CNECheckBatch obj)
		{
			this.UpdateCommonNationalExamCertificateCheckBatch(((System.Nullable<long>)(obj.Id)), ((System.Nullable<bool>)(obj.Executing)));
		}
		
		private void UpdateCNECheckBatch(CNECheckBatch obj)
		{
			this.UpdateCommonNationalExamCertificateCheckBatch(((System.Nullable<long>)(obj.Id)), ((System.Nullable<bool>)(obj.Executing)));
		}
		
		private void InsertCNERequestBatch(CNERequestBatch obj)
		{
			this.UpdateCommonNationalExamCertificateRequestBatch(((System.Nullable<long>)(obj.Id)), ((System.Nullable<bool>)(obj.Executing)));
		}
		
		private void UpdateCNERequestBatch(CNERequestBatch obj)
		{
			this.UpdateCommonNationalExamCertificateRequestBatch(((System.Nullable<long>)(obj.Id)), ((System.Nullable<bool>)(obj.Executing)));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SearchProcessingCommonNationalExamCertificateCheckBatch")]
		public ISingleResult<CNECheckBatch> SearchProcessingCommonNationalExamCertificateCheckBatch()
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())));
			return ((ISingleResult<CNECheckBatch>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.UpdateCommonNationalExamCertificateCheckBatch")]
		public int UpdateCommonNationalExamCertificateCheckBatch([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="BigInt")] System.Nullable<long> id, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Bit")] System.Nullable<bool> executing)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id, executing);
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SearchProcessingCommonNationalExamCertificateRequestBatch")]
		public ISingleResult<CNERequestBatch> SearchProcessingCommonNationalExamCertificateRequestBatch()
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())));
			return ((ISingleResult<CNERequestBatch>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.UpdateCommonNationalExamCertificateRequestBatch")]
		public int UpdateCommonNationalExamCertificateRequestBatch([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="BigInt")] System.Nullable<long> id, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Bit")] System.Nullable<bool> executing)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id, executing);
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetCommonNationalExamCertificateCheckBatch")]
		public ISingleResult<CNECheckBatch> GetCommonNationalExamCertificateCheckBatch([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="BigInt")] System.Nullable<long> id)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id);
			return ((ISingleResult<CNECheckBatch>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetCommonNationalExamCertificateRequestBatch")]
		public ISingleResult<CNERequestBatch> GetCommonNationalExamCertificateRequestBatch([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="BigInt")] System.Nullable<long> id)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id);
			return ((ISingleResult<CNERequestBatch>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.CheckCommonNationalExamCertificateForm")]
		public ISingleResult<CNEForm> CheckCommonNationalExamCertificateForm([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="NVarChar(50)")] string regionCode)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), regionCode);
			return ((ISingleResult<CNEForm>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.ImportCommonNationalExamCertificate")]
		public int ImportCommonNationalExamCertificate([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="NVarChar(255)")] string importCertificateFilePath, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="NVarChar(255)")] string importCertificateSubjectFilePath)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), importCertificateFilePath, importCertificateSubjectFilePath);
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.ImportCommonNationalExamCertificateDeny")]
		public int ImportCommonNationalExamCertificateDeny([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="NVarChar(255)")] string importFilePath)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), importFilePath);
			return ((int)(result.ReturnValue));
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="")]
	public partial class CNECheckBatch : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _Id;
		
		private string _Batch;
		
		private System.Nullable<bool> _Executing;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(long value);
    partial void OnIdChanged();
    partial void OnBatchChanging(string value);
    partial void OnBatchChanged();
    partial void OnExecutingChanging(System.Nullable<bool> value);
    partial void OnExecutingChanged();
    #endregion
		
		public CNECheckBatch()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long Id
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Batch", DbType="NText NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
		public string Batch
		{
			get
			{
				return this._Batch;
			}
			set
			{
				if ((this._Batch != value))
				{
					this.OnBatchChanging(value);
					this.SendPropertyChanging();
					this._Batch = value;
					this.SendPropertyChanged("Batch");
					this.OnBatchChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Executing", DbType="Bit")]
		public System.Nullable<bool> Executing
		{
			get
			{
				return this._Executing;
			}
			set
			{
				if ((this._Executing != value))
				{
					this.OnExecutingChanging(value);
					this.SendPropertyChanging();
					this._Executing = value;
					this.SendPropertyChanged("Executing");
					this.OnExecutingChanged();
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="")]
	public partial class CNERequestBatch : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _Id;
		
		private string _Batch;
		
		private System.Nullable<bool> _Executing;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(long value);
    partial void OnIdChanged();
    partial void OnBatchChanging(string value);
    partial void OnBatchChanged();
    partial void OnExecutingChanging(System.Nullable<bool> value);
    partial void OnExecutingChanged();
    #endregion
		
		public CNERequestBatch()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long Id
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Batch", DbType="NText NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
		public string Batch
		{
			get
			{
				return this._Batch;
			}
			set
			{
				if ((this._Batch != value))
				{
					this.OnBatchChanging(value);
					this.SendPropertyChanging();
					this._Batch = value;
					this.SendPropertyChanged("Batch");
					this.OnBatchChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Executing", DbType="Bit")]
		public System.Nullable<bool> Executing
		{
			get
			{
				return this._Executing;
			}
			set
			{
				if ((this._Executing != value))
				{
					this.OnExecutingChanging(value);
					this.SendPropertyChanging();
					this._Executing = value;
					this.SendPropertyChanged("Executing");
					this.OnExecutingChanged();
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="")]
	public partial class CNEForm : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private string _Number;
		
		private string _CertificateNumber;
		
		private bool _IsValid;
		
		private bool _IsCertificateExist;
		
		private bool _IsCertificateDeny;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnNumberChanging(string value);
    partial void OnNumberChanged();
    partial void OnCertificateNumberChanging(string value);
    partial void OnCertificateNumberChanged();
    partial void OnIsValidChanging(bool value);
    partial void OnIsValidChanged();
    partial void OnIsCertificateExistChanging(bool value);
    partial void OnIsCertificateExistChanged();
    partial void OnIsCertificateDenyChanging(bool value);
    partial void OnIsCertificateDenyChanged();
    #endregion
		
		public CNEForm()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Number", DbType="NVarChar(255) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
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
					this.OnNumberChanging(value);
					this.SendPropertyChanging();
					this._Number = value;
					this.SendPropertyChanged("Number");
					this.OnNumberChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CertificateNumber", DbType="NVarChar(255)")]
		public string CertificateNumber
		{
			get
			{
				return this._CertificateNumber;
			}
			set
			{
				if ((this._CertificateNumber != value))
				{
					this.OnCertificateNumberChanging(value);
					this.SendPropertyChanging();
					this._CertificateNumber = value;
					this.SendPropertyChanged("CertificateNumber");
					this.OnCertificateNumberChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsValid", DbType="Bit")]
		public bool IsValid
		{
			get
			{
				return this._IsValid;
			}
			set
			{
				if ((this._IsValid != value))
				{
					this.OnIsValidChanging(value);
					this.SendPropertyChanging();
					this._IsValid = value;
					this.SendPropertyChanged("IsValid");
					this.OnIsValidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsCertificateExist", DbType="Bit")]
		public bool IsCertificateExist
		{
			get
			{
				return this._IsCertificateExist;
			}
			set
			{
				if ((this._IsCertificateExist != value))
				{
					this.OnIsCertificateExistChanging(value);
					this.SendPropertyChanging();
					this._IsCertificateExist = value;
					this.SendPropertyChanged("IsCertificateExist");
					this.OnIsCertificateExistChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsCertificateDeny", DbType="Bit")]
		public bool IsCertificateDeny
		{
			get
			{
				return this._IsCertificateDeny;
			}
			set
			{
				if ((this._IsCertificateDeny != value))
				{
					this.OnIsCertificateDenyChanging(value);
					this.SendPropertyChanging();
					this._IsCertificateDeny = value;
					this.SendPropertyChanged("IsCertificateDeny");
					this.OnIsCertificateDenyChanged();
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
}
#pragma warning restore 1591
