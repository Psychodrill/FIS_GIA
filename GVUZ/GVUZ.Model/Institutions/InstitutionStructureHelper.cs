using System.Linq;
using System.Reflection;
using FogSoft.Helpers;
using GVUZ.Model.Entrants;
using log4net;

namespace GVUZ.Model.Institutions
{
    /// <summary>
    ///     Вспомогательные методы для создания структуры ОУ и структуры приема.
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
                    _russianLanguageSubjectID = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.Subject).Where(x => x.Value.ToLower() == "русский язык")
                                                         .Select(x => x.Key).SingleOrDefault();
                    if (_russianLanguageSubjectID == 0)
                        LogHelper.Log.ErrorFormat(
                            "Не найден русский язык среди предеметов. Поиск проводился по фразе \"русский язык\"");
                }
            }

            return _russianLanguageSubjectID;
        }

        /// <summary>
        ///     Используется при создании ОУ.
        /// </summary>
        public static InstitutionItem AddRootStructureItemsAndSave
            (this InstitutionsEntities x, Institution institution, string itemName)
        {
            return AddStructureItemsAndSave(x, institution, AdmissionItemTypeConstants.Institution,
                                            InstitutionItemType.Institution, itemName,
                                            true);
        }

        /// <summary>
        ///     Используется преимущественно для тестов, возвращает <see cref="InstitutionItem" />,
        ///     если был создан или null.
        /// </summary>
        public static InstitutionItem AddStructureItemsAndSave
            (this InstitutionsEntities x, Institution institution, short itemTypeID,
             InstitutionItemType institutionItemType, string itemName,
             bool createItem = false,
             InstitutionItem parent = null)
        {
            if (!createItem)
            {
                Log.Error("Вызов не обработан (!createItem).");
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