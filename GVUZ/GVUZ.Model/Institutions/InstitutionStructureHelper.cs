using System.Linq;
using System.Reflection;
using FogSoft.Helpers;
using GVUZ.Model.Entrants;
using log4net;

namespace GVUZ.Model.Institutions
{
    /// <summary>
    ///     ��������������� ������ ��� �������� ��������� �� � ��������� ������.
    /// </summary>
    public static class InstitutionStructureHelper
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static int _russianLanguageSubjectID;

        public static int GetRussianLanguageSubject(int institutionID)
        {
            if (_russianLanguageSubjectID == 0)
            {
                using (var dbContext = new EntrantsEntities())
                {
                    _russianLanguageSubjectID = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.Subject).Where(x => x.Value.ToLower() == "������� ����")
                                                         .Select(x => x.Key).SingleOrDefault();
                    if (_russianLanguageSubjectID == 0)
                        LogHelper.Log.ErrorFormat(
                            "�� ������ ������� ���� ����� ����������. ����� ���������� �� ����� \"������� ����\"");
                }
            }

            return _russianLanguageSubjectID;
        }

        /// <summary>
        ///     ������������ ��� �������� ��.
        /// </summary>
        public static InstitutionItem AddRootStructureItemsAndSave
            (this InstitutionsEntities x, Institution institution, string itemName)
        {
            return AddStructureItemsAndSave(x, institution, AdmissionItemTypeConstants.Institution,
                                            InstitutionItemType.Institution, itemName,
                                            true);
        }

        /// <summary>
        ///     ������������ ��������������� ��� ������, ���������� <see cref="InstitutionItem" />,
        ///     ���� ��� ������ ��� null.
        /// </summary>
        public static InstitutionItem AddStructureItemsAndSave
            (this InstitutionsEntities x, Institution institution, short itemTypeID,
             InstitutionItemType institutionItemType, string itemName,
             bool createItem = false,
             InstitutionItem parent = null)
        {
            if (!createItem)
            {
                Log.Error("����� �� ��������� (!createItem).");
                x.SaveChanges();
                return null;
            }
            InstitutionItem item = null;
            if (createItem)
                item = CreateInstitutionItemAndStructure(institution, institutionItemType, itemName, parent);

            x.SaveChanges();
            return item;
        }

        private static InstitutionItem CreateInstitutionItemAndStructure
            (Institution institution, InstitutionItemType institutionItemType, string itemName,
             InstitutionItem parent = null)
        {
            var institutionItem = new InstitutionItem
                {
                    BriefName = itemName,
                    Name = itemName,
                    Institution = institution,
                    ItemType = institutionItemType,
                    Parent = parent
                };
            institutionItem.InstitutionStructure.Add
                (new InstitutionStructure
                    {
                        InstitutionItem = institutionItem,
                        Parent = parent == null ? null : parent.InstitutionStructure.First()
                    });
            institution.InstitutionItem.Add(institutionItem);
            return institutionItem;
        }
    }
}