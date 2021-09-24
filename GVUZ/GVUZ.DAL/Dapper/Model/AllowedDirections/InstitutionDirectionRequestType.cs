using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.DAL.Dapper.Model.AllowedDirections
{
    /// <summary>
    /// Вид заявки для работы с доступными направлениями
    /// </summary>
    public enum InstitutionDirectionRequestType
    {
        /// <summary>
        /// Добавление направления в список разрешенных
        /// </summary>
        AddAllowedDirection = 0,

        /// <summary>
        /// Удаление направления из списка разрешенных
        /// </summary>
        RemoveAllowedDirection = 1,
        
        /// <summary>
        /// Добавление разрешенного направления в список направлений с профильными испытаниями
        /// </summary>
        AddProfDirection = 2,
    }
}
