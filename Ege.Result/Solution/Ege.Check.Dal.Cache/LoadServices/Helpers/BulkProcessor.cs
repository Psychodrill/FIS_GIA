namespace Ege.Check.Dal.Cache.LoadServices.Helpers
{
    using System;
    using System.Collections.Generic;
    using Ege.Check.Dal.Cache.Interfaces;

    class BulkProcessor : IBulkProcessor
    {
        public void Process<TKey, TCached, TDtoPack>(
            CacheBulkLock<TKey, TCached> lockedElements,
            IEnumerable<TDtoPack> dtos,
            Action<TCached, TDtoPack> processAction) where TCached : class
        {
            int index = 0;
            int count = lockedElements.Elements.Count;
            foreach (var group in dtos)
            {
                if (count <= index)
                {
                    throw new InvalidOperationException(
                        string.Format("Element count mismatch: locked cache elements contain {0} elements while at least {1} dto groups processed",
                            count, index));
                }
                var element = lockedElements.Elements[index++];
                if (element == null)
                {
                    throw new InvalidOperationException(string.Format("Locked cache elements collection contains null element at index {0}", index - 1));
                }
                processAction(element.Value, group);
            }
        }
    }
}