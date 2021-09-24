//using GVUZ.DAL.Dapper.Model.TargetOrganization;
using System;
using System.Collections.Generic;
using GVUZ.DAL.Dapper.ViewModel.InstitutionProgram;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GVUZ.DAL.Dapper.Repository.Interfaces.InstitutionProgram
{
    public interface IInstitutionProgramRepository
    {
        dynamic GetInstitutionProgram(int institutionId);
        dynamic UpdateProgram(int institutionId, InstitutionProgramModel data);
        List<string> CanDeleteProgram(int institutionID, int? institutionProgramID);
        bool DeleteProgram(int institutionId, int? institutionProgramID);
    }
}
