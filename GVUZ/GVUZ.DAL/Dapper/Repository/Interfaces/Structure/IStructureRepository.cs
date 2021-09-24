using GVUZ.DAL.Dto;
using System.Collections.Generic;

namespace GVUZ.DAL.Dapper.Repository.Interfaces.Structure
{
    public interface IStructureRepository
    {
        List<StructureInfoDto> GetStructure(int institutionId, string userName);
    }
}
