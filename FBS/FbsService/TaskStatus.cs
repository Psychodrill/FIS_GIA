using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FbsService
{
    public abstract class TaskStatus
    {
        private Task mTask;

        public Task Task
        {
            get
            {
                return mTask;
            }
            internal set 
            {
                mTask = value;
            }
        }

        protected void SetStatus(string code)
        {
            Task.Status = Task.GetStatus(code); 
        }

        /// <summary>
        /// Выполнить дествия текущего состояния.
        /// </summary>
        internal protected abstract void Execute();

        /// <summary>
        /// Получить код статуса.
        /// </summary>
        internal protected abstract string GetStatusCode();

        protected void FinishTask()
        {
            Task.IsActive = false;
            Task.Status = null;
        }

        protected void LogException(Exception ex)
        {
            TaskManager.Instance().Logger.Error("Error on status executing", ex);
        }

        protected void LogInfo(string message)
        {
            TaskManager.Instance().Logger.InfoFormat("{0}: {1}", Task.Code, message);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            return this.GetType() == obj.GetType();
        }

        public override int GetHashCode()
        {
            return this.GetType().GetHashCode();
        }
    }
}
