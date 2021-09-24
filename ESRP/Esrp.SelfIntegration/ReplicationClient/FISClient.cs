using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using Esrp.DB;
using Esrp.Integration.Common;

namespace Esrp.SelfIntegration.ReplicationClient
{
    internal class FISClient
    {
        private ESRPServiceClientFactory clientFactory;
        private int rowCount_;
        public FISClient(string serviceUrl,int rowCount)
        {
            if (rowCount == 0)
                throw new ArgumentException("rowCount");

            clientFactory = new ESRPServiceClientFactory(serviceUrl);
            rowCount_ = rowCount;
        }

        public void RunReplication()
        {
            Prepare();
            try
            {
                while (ProcessBatch()) { }                
            }
            catch (Exception ex)
            {
                RaiseCriticalError(ex);
            }
        }

        private bool ProcessBatch()
        {
            string errorInfo;
            try
            {
                int remainingCount = clientFactory.Client.SyncWithFis(rowCount_,  out errorInfo);
                if (!String.IsNullOrEmpty(errorInfo))
                    throw new Exception(errorInfo);
                if (remainingCount > 0)
                {
                    RaiseMesssage(String.Format("Осталось {0} записей", remainingCount));
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Ошибка при обращении к веб-сервису ЕСРП: {0}", ex.Message), ex);
            }           
        }
        
        private bool prepared_;
        private void Prepare()
        {
            if (prepared_)
                return;

            try
            {
                clientFactory.CreateClient();
            }
            catch (Exception ex)
            {
                RaiseCriticalError(ex);
                return;
            }

            prepared_ = true;
        } 

        private void RaiseCriticalError(Exception ex)
        {
            if (CriticalError != null)
            {
                CriticalError(this, new ExceptionEventArgs(ex));
            }
        }

        private void RaiseMesssage(string message)
        {
            if (Message != null)
            {
                Message(this, new MessageEventArgs(message));
            }
        }

        public event EventHandler<ExceptionEventArgs> CriticalError;
        public event EventHandler<MessageEventArgs> Message;
    }
}
