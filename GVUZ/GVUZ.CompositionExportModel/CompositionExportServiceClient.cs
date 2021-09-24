using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace GVUZ.CompositionExportModel
{
    public class CompositionExportServiceClient : ClientBase<ICompositionExportService>, ICompositionExportService
    {
        public CompositionExportServiceClient() { }

        public CompositionExportServiceClient(Binding binding, EndpointAddress endpointAddress) : base(binding, endpointAddress) { }

        public CompositionExportResult GetCompositions(List<CompositionRequestItem> compositionItems)
        {
            return Channel.GetCompositions(compositionItems);
        }

        public ERBDCompositionInfoList GetAllCompositionInfos(bool for2015, bool for2016)
        {
            return Channel.GetAllCompositionInfos(for2015, for2016);
        }

        public bool PrepareCompositionInfos(bool for2015, bool for2016)
        {
            return Channel.PrepareCompositionInfos(for2015, for2016);
        }
    }
}
