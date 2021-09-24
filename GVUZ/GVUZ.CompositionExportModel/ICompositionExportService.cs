using GVUZ.CompositionExportModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GVUZ.CompositionExportModel
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ICompositionExportService : IDisposable
    {
        [OperationContract]
        CompositionExportResult GetCompositions(List<CompositionRequestItem> compositionItems);

        [OperationContract]
        ERBDCompositionInfoList GetAllCompositionInfos(bool for2015, bool for2016);

        [OperationContract]
        bool PrepareCompositionInfos(bool for2015, bool for2016);
    }


}
