namespace Ege.Check.Dal.Cache.LoadServices.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    public class ParticipantExamLookup<T>
    {
        [NotNull]
        public List<KeyValuePair<ParticipantCacheModel, ILookup<int, T>>> Lookup { get; set; }

        /// <summary>
        /// Набор ключей для кэша с ключом-экзаменом
        /// </summary>
        [NotNull]
        public IEnumerable<KeyValuePair<ParticipantCacheModel, int>> Exams 
        {
            get
            {
                return Lookup.SelectMany(
                    p => p.Value.Select(pe => new KeyValuePair<ParticipantCacheModel, int>(p.Key, pe.Key)));
            } 
        }

        /// <summary>
        /// Набор элементов для экзамена участника, порядок экзаменов должен совпадать с порядком в коллекции Exams
        /// </summary>
        [NotNull]
        public IEnumerable<IGrouping<int, T>> ItemsByExams
        {
            get { return Lookup.SelectMany(pe => pe.Value); }
        }

        public ParticipantExamLookup([NotNull]List<KeyValuePair<ParticipantCacheModel, ILookup<int, T>>> collection)
        {
            Lookup = collection;
        }
    }
}
