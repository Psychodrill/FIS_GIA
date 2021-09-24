using GVUZ.ServiceModel.Import.Core.Packages.Handlers;
using GVUZ.ServiceModel.Import.Package;

namespace GVUZ.ServiceModel.Import.Core.Storages
{
	public class StorageManager
	{
		private readonly DbObjectRepositoryBase _dbObjectRepo;		

		// В ходе импорта все объекты распределяются по коллекциям InsertStorage, UpdateStorage, DeleteStorage, ConflictStorage

		private readonly ConflictStorage _conflictStorage;
		private readonly DtoObjectStorage _insertStorage = new DtoObjectStorage();
		private readonly DtoObjectStorage _updateStorage = new DtoObjectStorage();
		private readonly DtoObjectStorage _processedDtoStorage = new DtoObjectStorage();
		private readonly DbObjectStorage _deleteStorage;
		
		private readonly ImportResultPackage _importResultPackage = new ImportResultPackage();

		private readonly string _userLogin;

		public string UserLogin
		{
			get { return _userLogin; }
		}

		public StorageManager(DbObjectRepositoryBase dbObjectRepo, ConflictStorage conflictStorage, string userLogin)
		{			
			_dbObjectRepo = dbObjectRepo;
			_deleteStorage = new DbObjectStorage(dbObjectRepo);
			_conflictStorage = conflictStorage;
			_userLogin = userLogin;
		}

		public StorageManager(DbObjectRepositoryBase dbObjectRepo)
		{
			_dbObjectRepo = dbObjectRepo;
			_deleteStorage = new DbObjectStorage(dbObjectRepo);
			_conflictStorage = null;
		}

		public DbObjectRepositoryBase DbObjectRepository
		{
			get
			{
				return _dbObjectRepo;
			}
		}

		public ConflictStorage ConflictStorage
		{
			get { return _conflictStorage; }
		}

		public DtoObjectStorage InsertStorage
		{
			get { return _insertStorage; }
		}

		public DbObjectStorage DeleteStorage
		{
			get { return _deleteStorage; }
		}

		public DtoObjectStorage UpdateStorage
		{
			get { return _updateStorage; }
		}

		/// <summary>
		/// Объекты dto, которые были обработаны в ходе импорта (для них был запущен ObjectImporter)
		/// Применяется в процессе загрузки дополнительных данных в ответе импорта 
		/// (используется для поиска родительских объектов).
		/// </summary>
		public DtoObjectStorage ProcessedDtoStorage
		{
			get { return _processedDtoStorage; }
		}

		public ImportResultPackage ImportResultPackage
		{
			get { return _importResultPackage; }
		}

		public int InstitutionID
		{
            get { return _dbObjectRepo.InstitutionId; }
		}
	}
}
