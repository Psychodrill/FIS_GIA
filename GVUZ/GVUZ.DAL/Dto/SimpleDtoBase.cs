namespace GVUZ.DAL.Dto
{
    /// <summary>
    /// Базовый класс простого DTO для хранения пары Id + Наименование 
    /// (для всяких комбобоксов или простых ассоциаций где нужно выводить надвание связанной сущности)
    /// </summary>
    /// <typeparam name="TId">Тип идентификатора</typeparam>
    public class SimpleDtoBase<TId>
    {
        public SimpleDtoBase()
        {
        }

        public SimpleDtoBase(TId id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public TId Id { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public string Name { get; set; }
    }
}
