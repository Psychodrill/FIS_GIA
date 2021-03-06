#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FbsService.Fbs
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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="FBS_2014")]
	internal partial class FbsContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertEntrantCheckBatch(EntrantCheckBatch instance);
    partial void DeleteEntrantCheckBatch(EntrantCheckBatch instance);
    partial void InsertSLCertificateCheckBatch(SLCertificateCheckBatch instance);
    partial void DeleteSLCertificateCheckBatch(SLCertificateCheckBatch instance);
    partial void InsertCompetitionRequestBatch(CompetitionRequestBatch instance);
    partial void DeleteCompetitionRequestBatch(CompetitionRequestBatch instance);
    #endregion
		
		public FbsContext() : 
				base(global::FbsService.Properties.Settings.Default.FBS_2014ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public FbsContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public FbsContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public FbsContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public FbsContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<EntrantCheckBatch> EntrantCheckBatches
		{
			get
			{
				return this.GetTable<EntrantCheckBatch>();
			}
		}
		
		public System.Data.Linq.Table<SLCertificateCheckBatch> SLCertificateCheckBatches
		{
			get
			{
				return this.GetTable<SLCertificateCheckBatch>();
			}
		}
		
		public System.Data.Linq.Table<CompetitionRequestBatch> CompetitionRequestBatches
		{
			get
			{
				return this.GetTable<CompetitionRequestBatch>();
			}
		}
		
		private void UpdateEntrantCheckBatch(EntrantCheckBatch obj)
		{
			this.UpdateEntrantCheckBatchExecuting(((System.Nullable<long>)(obj.Id)), ((System.Nullable<bool>)(obj.Executing)));
		}
		
		private void UpdateSLCertificateCheckBatch(SLCertificateCheckBatch obj)
		{
			this.UpdateSchoolLeavingCertificateCheckBatchExecuting(((System.Nullable<long>)(obj.Id)), ((System.Nullable<bool>)(obj.Executing)));
		}
		
		private void UpdateCompetitionRequestBatch(CompetitionRequestBatch obj)
		{
			this.UpdateCompetitionCertificateRequestBatchExecuting(((System.Nullable<long>)(obj.Id)), ((System.Nullable<bool>)(obj.Executing)));
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
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SearchProcessingEntrantCheckBatch")]
		public ISingleResult<EntrantCheckBatch> SearchProcessingEntrantCheckBatch()
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())));
			return ((ISingleResult<EntrantCheckBatch>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.UpdateEntrantCheckBatchExecuting")]
		public int UpdateEntrantCheckBatchExecuting([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="BigInt")] System.Nullable<long> id, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Bit")] System.Nullable<bool> executing)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id, executing);
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetEntrantCheckBatch")]
		public ISingleResult<EntrantCheckBatch> GetEntrantCheckBatch([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="BigInt")] System.Nullable<long> id)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id);
			return ((ISingleResult<EntrantCheckBatch>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SearchProcessingSchoolLeavingCertificateCheckBatch")]
		public ISingleResult<SLCertificateCheckBatch> SearchProcessingSchoolLeavingCertificateCheckBatch()
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())));
			return ((ISingleResult<SLCertificateCheckBatch>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.UpdateSchoolLeavingCertificateCheckBatchExecuting")]
		public int UpdateSchoolLeavingCertificateCheckBatchExecuting([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="BigInt")] System.Nullable<long> id, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Bit")] System.Nullable<bool> executing)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id, executing);
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetSchoolLeavingCertificateCheckBatch")]
		public ISingleResult<SLCertificateCheckBatch> GetSchoolLeavingCertificateCheckBatch([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="BigInt")] System.Nullable<long> id)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id);
			return ((ISingleResult<SLCertificateCheckBatch>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.UpdateCompetitionCertificateRequestBatchExecuting")]
		public int UpdateCompetitionCertificateRequestBatchExecuting([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="BigInt")] System.Nullable<long> id, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Bit")] System.Nullable<bool> executing)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id, executing);
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SearchProcessingCompetitionCertificateRequestBatch")]
		public ISingleResult<CompetitionRequestBatch> SearchProcessingCompetitionCertificateRequestBatch()
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())));
			return ((ISingleResult<CompetitionRequestBatch>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetCompetitionCertificateRequestBatch")]
		public ISingleResult<CompetitionRequestBatch> GetCompetitionCertificateRequestBatch([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="BigInt")] System.Nullable<long> id)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id);
			return ((ISingleResult<CompetitionRequestBatch>)(result.ReturnValue));
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="")]
	public partial class EntrantCheckBatch : INotifyPropertyChanging, INotifyPropertyChanged
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
		
		public EntrantCheckBatch()
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Batch", DbType="NText", UpdateCheck=UpdateCheck.Never)]
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
	public partial class SLCertificateCheckBatch : INotifyPropertyChanging, INotifyPropertyChanged
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
		
		public SLCertificateCheckBatch()
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
	public partial class CompetitionRequestBatch : INotifyPropertyChanging, INotifyPropertyChanged
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
		
		public CompetitionRequestBatch()
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
}
#pragma warning restore 1591
