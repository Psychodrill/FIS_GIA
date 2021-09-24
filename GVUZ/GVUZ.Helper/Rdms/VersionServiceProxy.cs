using System.ServiceModel;
using Rdms.Communication.Entities;
using Rdms.Communication.Interface;

namespace GVUZ.Helper.Rdms
{
    internal class VersionServiceProxy : ClientBase<IVersionService>, IVersionService
    {
        public VersionDescription GetVersionDescription(int id)
        {
            return Channel.GetVersionDescription(id);
        }

        public VersionStructure GetVersionStructure(int id)
        {
            return Channel.GetVersionStructure(id);
        }

        public int CreateVersion(VersionDescription description,
                                 VersionStructure structure)
        {
            return Channel.CreateVersion(description, structure);
        }

        public bool UpdateVersion(VersionDescription description,
                                  VersionStructure structure)
        {
            bool result = Channel.UpdateVersion(description, structure);

            return result;
        }

        public void DeleteVersion(int versionId)
        {
            Channel.DeleteVersion(versionId);
        }
    }
}