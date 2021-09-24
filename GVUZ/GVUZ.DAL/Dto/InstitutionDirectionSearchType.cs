using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.DAL.Dto
{
    /// <summary>
    /// Режим поиска направлений для работы с заявками и списком разрешенных направлений подготовки
    /// </summary>
    public enum InstitutionDirectionSearchType
    {
        /// <summary>
        /// Поиск направлений для включения в заявку на добавление в список разрешенных
        /// </summary>
        IncludeAllowedDirection,

        /// <summary>
        /// Поиск направлений для включения в заявку на удаление из списка разрешенных
        /// </summary>
        ExcludeAllowedDirection,

        /// <summary>
        /// Поиск разрешенных направлений для включения в список с профильными ВИ
        /// </summary>
        IncludeProfDirection,

        /// <summary>
        /// Поиск направлений при добавлении в список разрешенных от лица администратора
        /// </summary>
        IncludeAllowedDirectionAdmin
    }
}
