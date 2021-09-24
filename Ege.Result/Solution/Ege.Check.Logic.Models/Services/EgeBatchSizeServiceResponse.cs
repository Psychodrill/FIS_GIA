namespace Ege.Check.Logic.Models.Services
{
    using System.Runtime.Serialization;

    [DataContract]
    public class EgeBatchSizeServiceResponse
    {
        [DataMember]
        public int BatchSize { get; set; }
    }
}