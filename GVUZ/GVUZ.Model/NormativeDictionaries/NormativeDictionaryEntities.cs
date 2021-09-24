using System.Linq;
using GVUZ.Helper.Rdms;
using Rdms.Communication.Entities;

namespace GVUZ.Model.NormativeDictionaries
{
    public partial class NormativeDictionaryEntities
    {
        /// <summary>
        ///     ���������� ������� �������� ����������� (��� null, ���� ���������� ��� �� ��������������).
        /// </summary>
        public NormativeDictionary Get(Dictionary dictionaryId)
        {
            return NormativeDictionary.Where(x => x.DictionaryID == (int) dictionaryId).FirstOrDefault();
        }

        /// <summary>
        ///     ���������� ������� ������ ����������� (��� -1, ���� ���������� ��� �� ��������������).
        /// </summary>
        public int GetCurrentVersion(Dictionary dictionaryId)
        {
            NormativeDictionary dictionary =
                NormativeDictionary.Where(x => x.DictionaryID == (int) dictionaryId).FirstOrDefault();
            return dictionary == null ? -1 : dictionary.VersionID;
        }

        /// <summary>
        ///     �������� ���������� � ������ ����������� (��� ���������, ���� ������������� � ������ ���).
        /// </summary>
        public void AddOrEditVersion(Dictionary dictionaryId, VersionDescription versionDescription,
                                     bool saveChanges = true)
        {
            NormativeDictionary dictionary =
                NormativeDictionary.Where(x => x.DictionaryID == (int) dictionaryId).FirstOrDefault();
            if (dictionary == null)
            {
                dictionary = new NormativeDictionary
                    {
                        DictionaryID = (int) dictionaryId,
                        Name = DictionaryNames.Get(dictionaryId),
                        VersionID = versionDescription.Id ?? 0,
                        ActivationDate = versionDescription.ActivationDate,
                        VersionStateID = (byte) versionDescription.State
                    };
                NormativeDictionary.AddObject(dictionary);
            }
            else
            {
                dictionary.VersionID = versionDescription.Id ?? 0;
                dictionary.ActivationDate = versionDescription.ActivationDate;
                dictionary.VersionStateID = (byte) versionDescription.State;
            }
            if (saveChanges)
                SaveChanges();
        }
    }
}