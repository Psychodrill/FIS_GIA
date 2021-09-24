using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Esrp.DB.Common
{
    public abstract class EntityBase
    {
        private bool _hasChanges;
        public bool HasChanges { get { return _hasChanges; } }

        protected internal void SetHasChanges(bool value)
        {
            _hasChanges = value;
        }
    }
}
