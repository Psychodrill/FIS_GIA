namespace Ege.Check.Common.Hash
{
    /// <summary>
    ///     Хеширует ФИО участника
    /// </summary>
    public interface IFioHasher
    {
        /// <summary>
        ///     Вычислить хеш от ФИО
        /// </summary>
        /// <param name="surname">Фамилия</param>
        /// <param name="firstName">Имя</param>
        /// <param name="patronymic">Отчество</param>
        /// <returns>Вычисленный хеш</returns>
        string GetHash(string surname, string firstName, string patronymic);

        /// <summary>
        /// Являются ли имена одинаковыми с точностью до замен
        /// </summary>
        bool AreEqual(string name1, string name2);
    }
}
