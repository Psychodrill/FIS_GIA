using System.Collections.Generic;
using System.ServiceModel;
using Rdms.Communication.Entities;
using Rdms.Communication.Exceptions;

namespace Rdms.Communication.Interface
{
	[ServiceContract]
	public interface IDirectoryService
	{
		/// <summary>
		/// 	Получение описания справочника по идентификатору
		/// </summary>
		/// <param name = "id">Идентификатор справочника</param>
		/// <returns>Описание справочника</returns>
		[OperationContract]
		DirectoryDescription GetDirectoryDescription(int id);

		/// <summary>
		/// 	Получение структуры справочника, включая описание версий, по идентификатору
		/// </summary>
		/// <param name = "id">Идентификатор справочника</param>
		/// <returns>Описание структуры справочника</returns>
		[OperationContract]
		DirectoryStructure GetDirectoryStructure(int id);

		[OperationContract]
		List<DirectoryDescription> GetAllDirectoryDescriptions();

		[OperationContract]
		Dictionary<int, DirectoryStructure> GetAllDirectoryStructures();

		[OperationContract]
		[FaultContract(typeof (NameAlreadyInUseException))]
		int CreateDirectory(DirectoryDescription description);

		[OperationContract]
		[FaultContract(typeof (NameAlreadyInUseException))]
		void UpdateDirectory(DirectoryDescription description);

		[OperationContract]
		[FaultContract(typeof (IllegalDeletingException))]
		void DeleteDirectory(int directoryId);

		#region Theme

		[OperationContract]
		Dictionary<int, DirectoryTheme> GetThemeList();

		[OperationContract]
		[FaultContract(typeof (NameAlreadyInUseException))]
		int SaveTheme(DirectoryTheme theme);

		[OperationContract]
		void DeleteTheme(int id);

		#endregion
	}
}