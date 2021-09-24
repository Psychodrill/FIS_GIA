using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using GVUZ.Util.Services.Parser;

namespace GVUZ.Util.UI.Parsing
{
    public class ParseViewPresenter
    {
        private readonly IParseView _view;
        private BackgroundWorker _worker;
        private CancellationTokenSource _cancellationToken;

        public ParseViewPresenter(IParseView view)
        {
            _view = view;
        }

        public void StartParse()
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
                _worker.ProgressChanged += RunWorkerProgress;
            }

            _cancellationToken = new CancellationTokenSource();
            _worker.RunWorkerAsync();
        }

        private void RunWorkerProgress(object sender, ProgressChangedEventArgs e)
        {
            ImportPackageListReadProgressEventArgs args = e.UserState as ImportPackageListReadProgressEventArgs;

            if (_worker.CancellationPending)
            {
                _view.ParseStatusText = "Отмена...";
            }
            else
            {
                _view.ParseStatusText = args == null ? "Обработка..." : string.Format("Обработано {0} из {1} пакетов ({2}%)", args.ReadPackages, args.TotalPackages, args.Progress);
                _view.CurrentProgress = e.ProgressPercentage;
            }
        }

        public void StopParse()
        {
            if (_worker != null && !_worker.CancellationPending)
            {
                if (_view.GetUserConfirmation("Отменить выполнение текущей операции?") && _worker.IsBusy)
                {
                    _view.StopButtonEnabled = false;
                    _worker.CancelAsync();
                    _cancellationToken.Cancel();
                }
            }
        }

        public void ViewLoaded()
        {
            _view.ContentVisible = false;
        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _view.StopTimer();
            _view.ContentVisible = false;
            _view.StartButtonEnabled = true;
            _view.SetupButtonEnabled = true;
            _view.StopButtonEnabled = false;

            ParseResult result = e.Result as ParseResult;

            if (result != null)
            {
                if (result.Status == ParseResultStatus.Completed)
                {
                    _view.InformationDialog("Операция успешно завершена", "Завершено");
                    
                }
                else if (result.Status == ParseResultStatus.Cancelled)
                {
                    _view.WarningDialog("Выполнение операции отменено пользователем", "Отменено");
                }
                else if (result.Status == ParseResultStatus.Exception)
                {
                    _view.ErrorDialog(result.Message ?? "Произошла нераспознанная ошибка", "Ошибка");
                }
            }
            else if (e.Cancelled)
            {
                _view.WarningDialog("Выполнение операции отменено пользователем", "Отменено");
            }
        }

        private void RunWorker(object sender, DoWorkEventArgs e)
        {
            _view.StartButtonEnabled = false;
            _view.StopButtonEnabled = false;
            _view.SetupButtonEnabled = false;
            _view.CurrentProgress = 0;
            _view.ParseStatusText = "Запуск...";
            _view.ContentVisible = true;

            SqlBulkCopy bulk = null;

            try
            {
                var setup = new ParseSetupInfo();

                if (setup.TruncateTable)
                {
                    using (var cn = new SqlConnection(setup.ConnectionString))
                    {
                        cn.Open();
                        using (SqlCommand cmd = cn.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "truncate table [dbo].[ImportPackageParsed]";
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                using (var list = new ImportPackageList(setup.ConnectionString, setup.Filter, 1000))
                {
                    list.ReportProgress = true;
                    list.ProgressNotificationRate = 5;
                    list.Progress += ImportPackageListOnProgress;

                    using (var reader = new ApplicationOrderRecordListReader(new ApplicationOrderRecordList(list)))
                    {
                        bulk = new SqlBulkCopy(setup.ConnectionString);
                        bulk.DestinationTableName = "[dbo].[ImportPackageParsed]";
                        bulk.BulkCopyTimeout = 0;
                        bulk.NotifyAfter = 100;
                        reader.GetColumnMappings().ToList().ForEach(x => bulk.ColumnMappings.Add(x));
                        bulk.SqlRowsCopied += BulkOnSqlRowsCopied;

                        _view.ParseStatusText = "Обработка...";
                        _view.StartTimer();
                        _view.StopButtonEnabled = true;
                        
                        try
                        {
                            bulk.WriteToServer(reader);
                            e.Result = new ParseResult();
                        }
                        catch (OperationAbortedException)
                        {
                            e.Result = new ParseResult(ParseResultStatus.Cancelled);
                        }
                        finally
                        {
                            list.ReportProgress = false;
                            list.Progress -= ImportPackageListOnProgress;
                        }
                    }
                }
            }
            catch (Exception unhandled)
            {
                StringBuilder errorBuilder = new StringBuilder("При выполнении операции произошла ошибка");
                Exception current = unhandled;

                while (current != null)
                {
                    if (!string.IsNullOrEmpty(current.Message))
                    {
                        errorBuilder.AppendLine();
                        errorBuilder.Append(current.Message);
                    }

                    current = current.InnerException;
                }

                e.Result = new ParseResult(ParseResultStatus.Exception)
                {
                    Message = errorBuilder.ToString(),
                    Exception = unhandled,
                };
            }
            finally
            {
                if (bulk != null)
                {
                    bulk.SqlRowsCopied -= BulkOnSqlRowsCopied;
                }

                _view.ParseStatusText = "Завершение...";
                _view.StopTimer();
            }
        }

        private void BulkOnSqlRowsCopied(object sender, SqlRowsCopiedEventArgs e)
        {
            e.Abort = _worker.CancellationPending;
        }

        private void ImportPackageListOnProgress(object sender, ImportPackageListReadProgressEventArgs e)
        {
            _worker.ReportProgress(e.Progress, e);
        }

        public void SetupParse()
        {
            using (var form = new ParseSettingsForm())
            {
                form.ShowDialog(_view as IWin32Window);
            }
        }
    }
}