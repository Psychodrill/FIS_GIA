using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GVUZ.Model
{
    public static class ModelExtensions
    {
        public static bool IsFlagSet(this short flag, short check)
        {
            return Convert.ToInt32(flag).IsFlagSet(Convert.ToInt32(check));
        }

        public static bool IsFlagSet(this int flag, int check)
        {
            return (flag & check) == check;
        }

        /// <summary>
        /// Формирование ExpressionTree запроса для поиска по большому кол-ву идентификаторов а конструкии IN(...) в SQL
        /// </summary>
        /// <typeparam name="TEntityObject">Тип модели</typeparam>
        /// <typeparam name="TInType">Тип идентификатора для поиска</typeparam>
        /// <param name="entities">Модель</param>
        /// <param name="peopertyExpression">Выражение, возвращающее идентифкатор для сравнения с IN(...)</param>
        /// <param name="ids">Коллекция идентификаторов в IN(...)</param>
        /// <param name="chunkSize">Количество идентификаторов в IN(...)</param>
        /// <returns></returns>
        public static IEnumerable<TEntityObject> WhereIn<TEntityObject, TInType>(this IQueryable<TEntityObject> entities,
            Expression<Func<TEntityObject, TInType>> peopertyExpression, IEnumerable<TInType> ids, int chunkSize = 100)
            where TEntityObject : EntityObject
        {
            var propertyName = peopertyExpression.GetExpressionPropertyName();
            foreach (var chunk in ids.Chunk(chunkSize))
            {
                var q = entities.Where(ContainsPredicate<TEntityObject, TInType>(chunk, propertyName));
                foreach (var item in q)
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Получение наименование property по типу результирующего expression
        /// </summary>
        /// <typeparam name="TEntityObject"></typeparam>
        /// <typeparam name="TInType"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        static string GetExpressionPropertyName<TEntityObject, TInType>(this Expression<Func<TEntityObject, TInType>> propertyExpression)
        {
            var mbody = propertyExpression.Body as MemberExpression;
            if (mbody == null)
            {
                var ubody = propertyExpression.Body as UnaryExpression;
                if (ubody != null)
                    mbody = ubody.Operand as MemberExpression;

                if (mbody == null)
                    throw new ArgumentException();
            }

            return mbody.Member.Name;
        }

        /// <summary>
        /// ExpressionTree для формирования запроса типа IN(...) через EF4
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TInType"></typeparam>
        /// <param name="arr"></param>
        /// <param name="fieldname"></param>
        /// <returns></returns>
        static Expression<Func<TEntity, bool>> ContainsPredicate<TEntity, TInType>(IEnumerable<TInType> arr, string fieldname) where TEntity : class
        {
            var entity = Expression.Parameter(typeof(TEntity), "entity");
            var member = Expression.Property(entity, fieldname);

            var containsMethods = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public).Where(m => m.Name == "Contains");
            var method = containsMethods.FirstOrDefault(m => m.GetParameters().Count() == 2);
            method = method.MakeGenericMethod(member.Type);
            var exprContains = Expression.Call(method, new Expression[] { Expression.Constant(arr), member });
            return Expression.Lambda<Func<TEntity, bool>>(exprContains, entity);
        }

        /// <summary>
        /// Результат сравнения строк без учёта регистра и начальных/конечных пробелов
        /// </summary>
        public static bool CompareIgnoreCase(this string argument1, string argument2)
        {
            if (argument1 == null) return false;
            return argument1.Trim().ToUpper() == argument2.Trim().ToUpper();
        }
    }

    /// <summary>
    /// Вспомогательный класс для формирования пакетов идентификаторов в запросе IN (...)
    /// </summary>
    public static class EnumerableSlicing
    {
        private class Status
        {
            public bool EndOfSequence;
        }

        private static IEnumerable<T> TakeOnEnumerator<T>(IEnumerator<T> enumerator, int count,
            Status status)
        {
            while (--count > 0 && (enumerator.MoveNext() || !(status.EndOfSequence = true)))
            {
                yield return enumerator.Current;
            }
        }

        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> items, int chunkSize)
        {
            if (chunkSize < 1)
                throw new ArgumentException("Кол-во идентификаторов в пакете не может быть = 0");

            var status = new Status { EndOfSequence = false };
            using (var enumerator = items.GetEnumerator())
            {
                while (!status.EndOfSequence)
                {
                    yield return TakeOnEnumerator(enumerator, chunkSize, status);
                }
            }
        }
    }
}
