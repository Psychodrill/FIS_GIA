using System.Xml.Serialization;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Export
{
    public class PreparatoryCourseDto
    {
        public string CourseName;
        public string Information;
        [XmlArrayItem("Subject")] public EntranceTestSubjectDto[] Subjects;
    }
}