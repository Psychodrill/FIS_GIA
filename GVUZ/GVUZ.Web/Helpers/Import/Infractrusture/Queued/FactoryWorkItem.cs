using System;

namespace GVUZ.Web.Import.Infractrusture.Queued
{
    public class FactoryWorkItem<T, TResult> : IEquatable<FactoryWorkItem<T, TResult>>
    {
        public static object x = new object(); 

        public Guid TicketId { get; set; }
        public bool IsDisposed { get; set; }
        public Func<T, TResult> Action { get; set; }
        public T ActionTask { get; set; }
        
        public event Action<TResult> OnWorkCompleteEvent;
        public event Action<string> OnWorkFailedEvent;

        public void RaiseCompleteEvent(TResult result)
        {
            lock (x)
            {
                if (OnWorkCompleteEvent != null)
                    OnWorkCompleteEvent(result);
            }
        }

        public void RaiseFailedEvent(string message)
        {
            lock (x)
            {
                if (OnWorkFailedEvent != null)
                    OnWorkFailedEvent(message);
            }
        }

        #region IEquatable<FactoryWorkItem<T, TResult>>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as FactoryWorkItem<T, TResult>);
        }

        public bool Equals(FactoryWorkItem<T, TResult> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            bool result = true;
            result &= other.TicketId == TicketId;
            return result;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = 17;
                result = result * 37 + TicketId.GetHashCode();
                return result;
            }
        }
        #endregion
    }
}