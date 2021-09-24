using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace GVUZ.AppExport.Services
{
    public class ExportProcessor
    {
        private readonly IApplicationExportRequestRepository _repository;
        private readonly ApplicationExportRequestMonitor _monitor;
        private readonly ConcurrentQueue<Guid> _currentItems = new ConcurrentQueue<Guid>();
        private readonly CancellationToken _token;
        private readonly int _numTasks;
        private readonly DirectoryInfo _sharedStorage;
        private readonly ILog _log;
        private const int MaxTasks = 64;
        private const int DefaultNumTasks = 4;

        private ILog Log
        {
            get
            {
                return _log ?? NullLogger.Instance;
            }
        }

        public ExportProcessor(CancellationToken token, int numTasks, IApplicationExportRequestRepository repository, ApplicationExportRequestMonitor monitor, DirectoryInfo sharedStorage)
        {
            _token = token;
            _repository = repository;
            _monitor = monitor;
            _sharedStorage = sharedStorage;
            _numTasks = numTasks >= 1 && numTasks <= MaxTasks ? numTasks : DefaultNumTasks;
            _log = LogManager.GetLogger(string.Empty) ?? NullLogger.Instance;
        }

        public void Run()
        {
            try
            {
                Log.Debug("Сброс состояния обработки запросов");
                _repository.ResetIncomplete();
                Log.InfoFormat("Хранилище файлов выгрузки: {0}", GetStorage().FullName);
                Log.DebugFormat("Инициализация потоков обработки ({0})", _numTasks);
                for (int i = 0; i < _numTasks; i++)
                {
                    Task.Factory.StartNew(DispatchQueue, TaskCreationOptions.LongRunning);
                }

                Log.Debug("Запуск монитора запросов");
                _monitor.RequestCreated += OnRequestCreated;
                _monitor.Run();
                Log.Debug("Монитор остановлен");
                _monitor.RequestCreated -= OnRequestCreated;
            }
            catch (Exception e)
            {
                Log.Fatal("Ошибка при запуске службы экспорта", e);
                throw;
            }
            
        }

        private void OnRequestCreated(object sender, ApplicationExportRequestMonitorEventArgs e)
        {
            _repository.CommitState(e.RequestId, ApplicationExportRequestStatus.Enqueued);

            foreach (Guid id in e.RequestId)
            {
                _currentItems.Enqueue(id);
            }
        }

        private void DispatchQueue()
        {
            do
            {
                while (_currentItems.Count > 0 && !_token.IsCancellationRequested)
                {
                    Guid requestId;
                    if (_currentItems.TryDequeue(out requestId))
                    {
                        Log.DebugFormat("Начало обработки запроса #{0} потоком {1}", requestId, Thread.CurrentThread.ManagedThreadId);

                        try
                        {
                            ProcessRequest(requestId);
                            Log.DebugFormat("Запрос #{0} успешно обработан потоком {1}", requestId, Thread.CurrentThread.ManagedThreadId);
                        }
                        catch (OperationCanceledException)
                        {
                            Log.WarnFormat("Получен сигнал о заверешении работы сервиса. Обработка запроса #{0} приостановлена и будет продожена при следующем запуске службы", requestId);
                        }
                        catch (Exception e)
                        {
                            Log.Error(string.Format("Ошибка обработки запроса #{0} потоком {1}", requestId, Thread.CurrentThread.ManagedThreadId), e);
                            _repository.CommitState(requestId, ApplicationExportRequestStatus.Error);
                        }
                    }
                }

            } while (!_token.WaitHandle.WaitOne(TimeSpan.FromSeconds(5)));
        }

        private void ProcessRequest(Guid requestId)
        {
            _token.ThrowIfCancellationRequested();
            _repository.CommitState(requestId, ApplicationExportRequestStatus.Processing);
            _token.ThrowIfCancellationRequested();

            ApplicationExportRequest request = _repository.FindByRequestId(requestId);

            if (request == null)
            {
                throw new ApplicationException(string.Format("Запрос на экспорт #{0} не найден", requestId));
            }

            var loader = GetLoader(request.InstitutionId, request.YearStart);
            var writer = new ApplicationXmlExporter(loader, request.InstitutionId);
            var fi = GetOutputFile(request.RequestDate, request.RequestId);
            
            using (var fs = new FileStream(fi.FullName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                writer.Export(fs);
            }

            _repository.CommitState(requestId, ApplicationExportRequestStatus.Complete);
        }

        private IApplicationExportLoader GetLoader(long institutionId, int yearStart)
        {
            // return new ApplicationExportLoader(institutionId, yearStart);
            return new DbApplicationExportLoader(ConfigurationManager.ConnectionStrings["AppExport"].ConnectionString, _token, DbApplicationExportLoader.DefaultPageSize, institutionId, yearStart);
        }

        private DirectoryInfo GetStorage()
        {
            if (!_sharedStorage.Exists)
            {
                _sharedStorage.Create();
                _sharedStorage.Refresh();
            }

            return _sharedStorage;
        }

        private FileInfo GetOutputFile(DateTime date, Guid requestId)
        {
            string fileName = string.Format("{0}.xml", requestId.ToString("N"));
            string fileFolderName = date.ToString("yyyyMMdd");
            DirectoryInfo fileLocation = new DirectoryInfo(Path.Combine(GetStorage().FullName, fileFolderName));
            if (!fileLocation.Exists)
            {
                fileLocation.Create();
            }

            return new FileInfo(Path.Combine(fileLocation.FullName, fileName));
        }
    }
}