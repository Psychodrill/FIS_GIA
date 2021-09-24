using System;
using System.Collections.Generic;
using System.Linq;

namespace Esrp.EIISIntegration.Import
{
    internal class MemoryImporter : ImporterBase<ExtendedEIISObject>
    {
        public MemoryImporter(string eiisObjectCode)
        {
            eiisObjectCode_ = eiisObjectCode;
        }

        public override string Name
        {
            get { return null; }
        }

        private string eiisObjectCode_;
        public override string EIISObjectCode
        {
            get { return eiisObjectCode_; }
        }

        public IEnumerable<ExtendedEIISObject> Objects { get { return _objects; } }
        private List<ExtendedEIISObject> _objects = new List<ExtendedEIISObject>();

        protected override void SaveObjects(IEnumerable<EIISObject> objects)
        {
            _objects.AddRange(objects.Select(x => new ExtendedEIISObject(x)));
        }

        protected override void DeleteObjects()
        {
            
        }  

        protected override ExtendedEIISObject GetExistingObject(EIISObject eIISObject)
        {
            throw new NotSupportedException();
        }

        protected override ExtendedEIISObject GetExistingObject(string eIISId)
        {
            throw new NotSupportedException();
        }

        protected override void SetDBObjectFields(ExtendedEIISObject dbObject, EIISObject eIISObject, bool isNew)
        {
            throw new NotSupportedException();
        }

        protected override IEnumerable<string> RequiredFields
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        protected override bool AllowDeleteObjects
        {
            get
            {
                return false;
            }
        }
    }
}
