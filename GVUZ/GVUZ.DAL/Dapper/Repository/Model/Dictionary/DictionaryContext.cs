using System.Configuration;
namespace GVUZ.DAL.Dapper.Repository.Model.Dictionary
{
    public class DictionaryContext
    {
        private static readonly DictionaryRepository dictionaryRepository = new DictionaryRepository();

        public static DictionaryRepository Dictionaries
        {
            get
            {
                return dictionaryRepository;
            }
        }
    }
}
