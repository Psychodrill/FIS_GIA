using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace GVUZ.Model.Institutions
{
    public partial class InstitutionsEntities
    {
        /// <summary>
        ///     Возвращает возможные <see cref="AdmissionItemType" /> по их <see cref="AdmissionItemLevel" />.
        /// </summary>
        /// <remarks>
        ///     При получении справочников имеют смысл следующие значения:
        ///     <list type="bullet">
        ///         <item>
        ///             <see cref="AdmissionItemLevel.EducationLevel" /> (Уровень образования)
        ///         </item>
        ///         <item>
        ///             <see cref="AdmissionItemLevel.Study" /> (Форма обучения)
        ///         </item>
        ///         <item>
        ///             <see cref="AdmissionItemLevel.AdmissionType" /> (Тип набора/приема)
        ///         </item>
        ///     </list>
        /// </remarks>
        public List<AdmissionItemType> GetAdmissionItemTypes(AdmissionItemLevel level)
        {
            return (from t in DictionaryCache.GetEntries(DictionaryCache.GetDictionaryByAdmissionLevel(level))
                    select new AdmissionItemType {ItemTypeID = (short) t.Key, Name = (string)t.Value}).ToList();
        }

        /// <summary>
        ///     Возвращает названия ОУ для autocomplete.
        /// </summary>
        public List<string> GetInstitutionNames(string namePart)
        {
            return
                (from i in Institution where i.FullName.Contains(namePart) orderby i.FullName select i.FullName).ToList();
        }

        /// <summary>
        ///     Возвращает названия регионов для autocomplete.
        /// </summary>
        public List<string> GetRegionNames(string namePart)
        {
            return (from r in RegionType where r.Name.Contains(namePart) orderby r.Name select r.Name).ToList();
        }

        /// <summary>
        ///     Возвращает названия направлений (специальностей) для autocomplete.
        /// </summary>
        public List<string> GetDirectionNames(string namePart)
        {
            return
                (from d in Direction where d.Name.Contains(namePart) orderby d.Name select d.Name).Distinct().ToList();
        }

        /// <summary>
        ///     Возвращает коды направлений (специальностей) для autocomplete.
        /// </summary>
        public List<string> GetDirectionCodes(string codePart)
        {
            return (from d in Direction where d.Code.Contains(codePart) orderby d.Code select d.Code).ToList();
        }
    }
}