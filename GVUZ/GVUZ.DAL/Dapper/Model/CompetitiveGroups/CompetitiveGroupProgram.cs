using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GVUZ.DAL.Dapper.Model.CompetitiveGroups
{
    [Table("CompetitiveGroupProgram")]
    public class CompetitiveGroupProgram
    {
        public int ProgramID { get; set; }

        //[StringLength(200)]
        //public string UID { get; set; }

        //public int CompetitiveGroupID { get; set; }

        //[StringLength(200)]
        //[Required]
        //public string Name { get; set; }

        //[StringLength(10)]
        //public string Code { get; set; }

        [StringLength(500)]
        public string Program { get; set; }

        //public DateTime? CreatedDate { get; set; }

        //public DateTime? ModifiedDate { get; set; }

    }

    //for autocomplete
    public class CompetitiveGroupInstitutionProgram
    {
        public int value { get; set; }

        [StringLength(500)]
        public string label { get; set; }

    }
}
