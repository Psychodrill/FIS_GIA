namespace Ege.Check.Logic.Models.Requests
{
    using System;
    using System.Collections.Generic;
    using Ege.Check.Common.Hash;
    using JetBrains.Annotations;

    public class ParticipantBlankRequest
    {
        /// <summary>
        ///     Фамилия
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        ///     Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///     Отчество
        /// </summary>
        public string Patronymic { get; set; }

        /// <summary>
        ///     Номер документа
        /// </summary>
        public string Document { get; set; }
    }

    public class ParticipantBlankRequestEqualityComparer : IEqualityComparer<ParticipantBlankRequest>
    {
        public const string Characteristic = "ReplacementInsensitive";

        [NotNull]private readonly IFioHasher _fioHasher;

        public ParticipantBlankRequestEqualityComparer([NotNull]IFioHasher fioHasher)
        {
            _fioHasher = fioHasher;
        }

        public bool Equals(ParticipantBlankRequest x, ParticipantBlankRequest y)
        {
            if (x == null || y == null)
            {
                return x == null && y == null;
            }
            return x.Document != null && y.Document != null && x.Document.Equals(y.Document, StringComparison.Ordinal)
                   && _fioHasher.AreEqual(x.FirstName, y.FirstName)
                   && _fioHasher.AreEqual(x.Surname, y.Surname)
                   && _fioHasher.AreEqual(x.Patronymic, y.Patronymic);
        }

        public int GetHashCode(ParticipantBlankRequest obj)
        {
            return obj != null && obj.Document != null ? obj.Document.GetHashCode() : 0;
        }
    }
}