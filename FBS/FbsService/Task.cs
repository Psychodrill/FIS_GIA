using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace FbsService
{
    /// <summary>
    /// Задание на выполнение.
    /// </summary>
    public partial class Task
    {
        /// <summary>
        /// Приоритет задания.
        /// </summary>
        public enum TaskPriority
        {
            Low = 0,
            Normal = 1,
            High = 2,
        }

        // Коды приоритетов.
        private static string[] PriorityCodes = { "low", "normal", "high" };
        private const string TypeXAttributeName = "Type";

        // Текущий статус.
        [NonSerialized]
        private TaskStatus mStatus = null;

        // Задача является только описанием для задач.
        private bool IsTaskDescription()
        {
            return this.GetType() == typeof(Task);
        }

        partial void OnCreated()
        {
            // Приоритет по умолчанию.
            this.Priority = TaskPriority.Normal;
            this.IsActive = true;
            // Код задачи устанавливается только при создании и больше не меняется.
            this.Code = this.GetTaskCode();
            if (!this.IsTaskDescription())
                this.Status = GetStatus(this.InternalStatus);
        }

        partial void OnLoaded()
        {
            if (!this.IsTaskDescription())
                this.Status = GetStatus(this.InternalStatus);
        }

        /// <summary>
        /// Сохранить изменения задачи. 
        /// </summary>
        public void Update()
        {
            if (this.Code == null)
                throw new NullReferenceException(string.Format("Can't update object of type {0}: Code is Empty.", 
                        this.GetType()));
            if (!this.IsActive & this.InternalStatus == null)
                this.InternalStatus = "finished";
            // Сохраняем все необходимые свойства в xml.
            Xml = this.ToXml();
            TaskContext.BeginLock();
            try
            {
                TaskContext.Instance().InternalUpdateTask(this);
            }
            finally
            {
                TaskContext.EndLock();
            }
        }

        /// <summary>
        /// Приоритет.
        /// </summary>
        [XmlIgnore()]
        public TaskPriority Priority
        {
            get
            {
                for (int i = 0; i < Task.PriorityCodes.Length; i++ )
                    if (Task.PriorityCodes[i] == this.InternalPriority)
                        return (TaskPriority)i;
                return TaskPriority.Normal;
            }
            set
            {
                this.InternalPriority = Task.PriorityCodes[(int)value];
            }
        }

        // Сохранить в Xml.
        private XElement ToXml()
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            serializer.Serialize(writer, this);
            StringReader reader = new StringReader(sb.ToString());
            XElement result = XElement.Load(reader);
            result.SetAttributeValue(XName.Get(TypeXAttributeName), this.GetType().FullName);
            return result;
        }

        // Создать задачу по Xml.
        private Task FromXml()
        {
            Task result = null;
            try
            {
                if (Xml == null)
                    return this;
                XAttribute attr = Xml.Attribute(XName.Get(TypeXAttributeName));
                if (attr != null)
                {
                    var serializer = new XmlSerializer(Type.GetType(attr.Value));
                    using(var xr = Xml.CreateReader())
                    {
                        result = (Task)serializer.Deserialize(xr);
                        result.Id = Id;
                        result.Code = Code;
                        result.InternalStatus = InternalStatus;
                        result.InternalPriority = InternalPriority;
                        result.OnLoaded();
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Error("ToXml", ex);
            }
            return result;
        }

        /// <summary>
        /// Получить статус задачи.
        /// </summary>
        /// <param name="code">Код задачи.</param>
        /// <returns>Статус задачи по переданному коду.</returns>
        protected internal virtual TaskStatus GetStatus(string code)
        {
            return null;
        }

        /// <summary>
        /// Получить код задачи.
        /// </summary>
        protected virtual string GetTaskCode()
        {
            return null;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is Task))
                return false;
            Task task = (Task)obj;
            return task.Id == this.Id && task.Code == this.Code;
        }

        public override int GetHashCode()
        {
            return this.GetType().GetHashCode() ^ this.Id.GetHashCode();
        }

        /// <summary>
        /// Статус задачи.
        /// </summary>
        [XmlIgnore()]
        public TaskStatus Status
        {
            get
            {
                if (mStatus == null && IsActive)
                    this.Status = GetStatus(this.InternalStatus);
                return mStatus;
            }
            internal set
            {
                if (value == null)
                    this.InternalStatus = null;
                else
                    this.InternalStatus = value.GetStatusCode();
                this.mStatus = value;
            }
        }

        static internal Task GetTask(long id)
        {
            TaskContext.BeginLock();
            try
            {
                return TaskContext.Instance().GetTask((long?)id).Single().FromXml();
            }
            finally
            {
                TaskContext.EndLock();
            }
        }

        static internal Task[] GetActiveTasks(int count)
        {
            TaskContext.BeginLock();
            try
            {
                return TaskContext.Instance().SearchActiveTask(null, count, null).ToArray();
            }
            finally
            {
                TaskContext.EndLock();
            }
        }

        public void ExecuteStep()
        {
            Task executingTask;
            if (this.GetType() == typeof(Task))
                if (this.Xml == null)
                    executingTask = GetTask(this.Id);
                else
                    executingTask = this.FromXml();
            else
                executingTask = this;

            if (executingTask == null) return;

            if (executingTask.Status != null)
            {
                executingTask.Status.Task = executingTask;
                try
                {
                    executingTask.Status.Execute();
                }
                catch(Exception ex)
                {
                    if (!(ex is IOException))
                    TaskManager.Instance().Logger.Error("ошибка ", ex);
                }

                if (executingTask.Status != null)
                    executingTask.Status.Task = null;
            }
            else
                executingTask.IsActive = false;
            executingTask.Update();
            if (this != executingTask)
            {
                this.Priority = executingTask.Priority;
                this.IsActive = executingTask.IsActive;
                this.Status = executingTask.Status;
            }
        }

        public void Execute()
        {
            while (this.IsActive)
                this.ExecuteStep();
        }
    }

    partial class TaskContext
    {
        internal void InternalUpdateTask(Task task)
        {
            this.UpdateTask(task);
        }

        [Function(Name = "dbo.SearchActiveTask")]
        public ISingleResult<Task> SearchActiveTask([Parameter(DbType = "Int")] int? startRowIndex,
                [Parameter(DbType = "Int")] int? maxRowCount,
                [Parameter(DbType = "Bit")] bool? showCount)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                    startRowIndex, maxRowCount, showCount);
            return ((ISingleResult<Task>)(result.ReturnValue));
        }
    }

}
