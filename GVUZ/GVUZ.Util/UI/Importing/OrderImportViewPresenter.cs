using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using GVUZ.Util.Services.Import;

namespace GVUZ.Util.UI.Importing
{
    public class OrderImportViewPresenter
    {
        private readonly IOrderImportView _view;
        private BackgroundWorker _worker;
        private CancellationTokenSource _cancellationToken;
        
        public OrderImportViewPresenter(IOrderImportView view)
        {
            _view = view;
        }

        public void ViewLoaded()
        {
            _view.AbortButtonEnabled = false;
            _view.ClearLog();
            _view.ImportStatusText = null;
        }

        public void StartImport()
        {
            if (_worker != null && _worker.IsBusy)
            {
                return;
            }

            if (_worker == null)
            {
                _worker = new BackgroundWorker();
                _worker.WorkerSupportsCancellation = true;
                _worker.WorkerReportsProgress = true;
                _worker.DoWork += RunWorker;
                _worker.RunWorkerCompleted += RunWorkerCompleted;
            }

            _cancellationToken = new CancellationTokenSource();
            _worker.RunWorkerAsync();
        }

        public void StopImport()
        {
            if (_worker != null && !_worker.CancellationPending)
            {
                if (_view.GetUserConfirmation("Отменить выполнение текущей операции?") && _worker.IsBusy)
                {
                    _view.AbortButtonEnabled = false;
                    _worker.CancelAsync();
                    _cancellationToken.Cancel();
                }
            }
        }

        public void SaveLog()
        {
            FileInfo logFile = _view.GetSavedLogFile();

            if (logFile != null)
            {
                File.WriteAllLines(logFile.FullName, _view.LogText);
            }
        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _view.ImportStatusText = "Завершено";
            _view.StopTimer();
            _view.RunButtonEnabled = true;
            _view.AbortButtonEnabled = false;
            _view.CurrentProgress = _view.MaxProgress;
            ImportResult result = e.Result as ImportResult;
            if (result != null)
            {
                _view.AppendLog(string.Format("Завершено. Обработано записей {0} из {1}", result.ProcessedRecords, result.TotalRecords));

                if (result.ProcessedRecords > 0)
                {
                    _view.AppendLog(string.Format("Успешно обработано {0} из {1}", result.SuccessRecords, result.ProcessedRecords));
                    _view.AppendLog(string.Format("Обработано с ошибками {0} из {1}", result.FailedRecords, result.ProcessedRecords));    
                }
            }
        }

        private void RunWorker(object sender, DoWorkEventArgs e)
        {
            _view.ClearLog();
            _view.ImportStatusText = "Инициализация...";
            _view.StartTimer();
            _view.RunButtonEnabled = false;
            _view.AbortButtonEnabled = true;
            _view.MaxProgress = 100;
            _view.CurrentProgress = 0;

            var setup = new ImportSetupInfo();
            var reader = new ApplicationOrderRecordReader(setup.ConnectionString, _cancellationToken.Token);
            var importer = new ApplicationOrderRecordImporter(setup.ConnectionString);
            var result = new ImportResult();
            
            try
            {
                reader.RecordFetched += (o, args) =>
                {
                    result.TotalRecords = args.TotalRecords;
                    _view.MaxProgress = args.TotalRecords;
                    _view.CurrentProgress++;
                    _view.ImportStatusText = string.Format("Обработка записи {0} из {1}", result.ProcessedRecords+1, args.TotalRecords);

                    try
                    {
                        importer.Import(args.Record);
                        result.SuccessRecords++;
                    }
                    catch (ApplicationException ie)
                    {
                        result.FailedRecords++;
                        _view.AppendLog(string.Format("Ошибка обработки заявления Id={0}, номер={1}, дата={2:dd.MM.yyyy}: {3}", args.Record.Id, args.Record.ApplicationNumber, args.Record.RegistrationDate, ie.Message));
                    }
                };

                reader.Run();
            }
            catch (Exception ex)
            {
                _view.AppendLog("Ошибка: " + ex.Message);
            }

            e.Result = result;
        }
    }
}