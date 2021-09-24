using GVUZ.DAL.Dapper.Repository.Model.OlympicDiplomant;
using GVUZ.Data.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace GVUZ.Web.ViewModels.OlympicDiplomant
{
    public class OlympicDiplomantBaseViewModel
    {

        //-----------------------------------------------------------------------------------------------------

        OlympicDiplomantRepository repository;
        public OlympicDiplomantRepository Repository
        {
            get
            {
                if (repository == null)
                    repository = new OlympicDiplomantRepository();
                return repository;
            }
        }

        //-----------------------------------------------------------------------------------------------------

        IEnumerable<SelectorItem> identityDocumentType;
        public IEnumerable<SelectorItem> IdentityDocumentType
        {
            get
            {
                if (identityDocumentType == null)
                {
                    identityDocumentType = Repository.GetIdentityDocumentTypeAll().Select(s => new SelectorItem
                    {
                        Id = s.IdentityDocumentTypeID,
                        Name = s.Name
                    });
                    identityDocumentType = new List<SelectorItem>(){ new SelectorItem
                    {
                        Id = 0,
                        Name = "Не выбрано"
                    }}.Concat(identityDocumentType);
                }
                return identityDocumentType;
            }
        }


        //-----------------------------------------------------------------------------------------------------

    }
}