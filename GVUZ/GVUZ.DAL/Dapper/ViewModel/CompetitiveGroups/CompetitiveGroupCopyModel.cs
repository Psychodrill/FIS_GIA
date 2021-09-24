using GVUZ.DAL.Dapper.ViewModel.Dictionary;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GVUZ.DAL.Dapper.ViewModel.Common;
using GVUZ.DAL.Dapper.Model.CompetitiveGroups;
using System.Linq;
using GVUZ.DAL.Dapper.Model.TargetOrganization;
using GVUZ.DAL.Dapper.Model.Dictionary;
using GVUZ.DAL.Dapper.Model.Benefit;
using GVUZ.DAL.Dto;
using System;

namespace GVUZ.DAL.Dapper.ViewModel.CompetitiveGroups
{
    public class CompetitiveGroupCopyModel
    {
        public int[] competitiveGroupIDs { get; set; }
        public int copy_year { get; set; }
        public int copy_сampaignType { get; set; }
        public int copy_levelBudget { get; set; }
        public int InstitutionID { get; set; }
    }
}
