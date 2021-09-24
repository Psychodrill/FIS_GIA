using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.DAL.Dto
{
    /// <summary>
    /// Запрос на поиск направлений подготовки в диалогах выбора специальностей
    /// для добавления в список разрешенных и в заявки на изменение списка разрешенных направлений 
    /// или направлений с профильными ВИ
    /// </summary>
    public class InstitutionDirectionSearchCommand
    {
        private IEnumerable<int> _tempDirectionsId;

        /// <summary>
        /// Вид поиска (определяет алгоритм фильтрации списка найденных направлений)
        /// <see cref="InstitutionDirectionSearchType"/>
        /// </summary>
        public InstitutionDirectionSearchType SearchType { get; set; }

        /// <summary>
        /// ID УГС
        /// </summary>
        public int UgsId { get; set; }

        /// <summary>
        /// ID уровня образования
        /// </summary>
        public int EducationLevelId { get; set; }

        /// <summary>
        /// Год для фильтра в AllowedDirections
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// Список идентификаторов направлений, уже выбранных пользователем но еще не сохраненных в заявке
        /// </summary>
        public IEnumerable<int> TempDirectionsId
        {
            get { return _tempDirectionsId ?? (_tempDirectionsId = Enumerable.Empty<int>()); }
            set { _tempDirectionsId = value; }
        }
    }
}
