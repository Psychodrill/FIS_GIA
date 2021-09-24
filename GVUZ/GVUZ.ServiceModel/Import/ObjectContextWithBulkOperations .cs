using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EntityClient;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using FogSoft.Helpers;
using GVUZ.Model;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import
{
    public interface IObjectContextWithBulkOperations
    {
        void Delete<TBase, T>(ObjectSet<TBase> entitySet, Expression<Func<T, bool>> predicate)
            where T : class, TBase
            where TBase : class;
        void DeleteIn<TEntityObject, TInType>(ObjectSet<TEntityObject> entitySet,
            Expression<Func<TEntityObject, TInType>> peopertyExpression, IEnumerable<TInType> ids, int chunkSize = 100)
            where TEntityObject : EntityObject;
        void DeleteIn<TEntityObject, TInType>(ObjectSet<TEntityObject> entitySet,
            string peopertyName, IEnumerable<TInType> ids, int chunkSize = 100)
            where TEntityObject : EntityObject;
    }

    public static class ObjectSetExtension
    {
        public static void Delete<TBase, T>(this ObjectSet<TBase> entitySet, Expression<Func<T, bool>> predicate)
            where T : class, TBase
            where TBase : class
        {
            var context = entitySet.Context as IObjectContextWithBulkOperations;
            if (context == null)
                throw new NotImplementedException();
            context.Delete(entitySet, predicate);
        }
        public static void Delete<T>(this ObjectSet<T> entitySet, Expression<Func<T, bool>> predicate)
            where T : class
        {
            Delete<T, T>(entitySet, predicate);
        }

        public static void DeleteIn<TEntityObject, TInType>(this ObjectSet<TEntityObject> entitySet,
            Expression<Func<TEntityObject, TInType>> peopertyExpression, IEnumerable<TInType> ids, int chunkSize = 100)
            where TEntityObject : EntityObject
        {
            if (!ids.Any()) return;

            var context = entitySet.Context as IObjectContextWithBulkOperations;
            if (context == null)
                throw new NotImplementedException();
            context.DeleteIn(entitySet, peopertyExpression, ids, chunkSize);
        }

        public static void DeleteIn<TEntityObject, TInType>(this ObjectSet<TEntityObject> entitySet,
            string peopertyName, IEnumerable<TInType> ids, int chunkSize = 100)
            where TEntityObject : EntityObject
        {
            if (!ids.Any()) return;

            var context = entitySet.Context as IObjectContextWithBulkOperations;
            if (context == null)
                throw new NotImplementedException();
            context.DeleteIn(entitySet, peopertyName, ids, chunkSize);
        }

        public static IEnumerable<IObjectWithUID> CheckDuplicates(this IEnumerable<IObjectWithUID> items)
        {
            if (!items.Any()) return items;

            var duplicates = new HashSet<string>();
            duplicates.UnionWith(from c in items.Where(c => 
                //!string.IsNullOrEmpty(c.UID)) group c by c.UID.Trim().ToUpper() into g where g.Count() > 1 select g.Key);
#warning убрали проверку без учета регистра - так как в остальных местах при загрузке ее нет
                !string.IsNullOrEmpty(c.UID)) group c by c.UID into g where g.Count() > 1 select g.Key);

            if (duplicates.Count > 0)
                throw new ApplicationException(string.Format("Найдены дубликаты ({0}) UID в БД: {1}",
                    items.First().GetType().Name, string.Join(",", duplicates.ToArray())));

            return items;
        }
    }

    public partial class ImportEntities : IObjectContextWithBulkOperations
    {
        private List<Action> _bulkDeletedActions;
        private List<Action> BulkDeletedActions
        {
            get
            {
                if (_bulkDeletedActions == null)
                    _bulkDeletedActions = new List<Action>();
                return _bulkDeletedActions;
            }
        }

        private List<object> _bulkDeletedEntities;
        public List<object> BulkDeletedEntities
        {
            get
            {
                if (_bulkDeletedEntities == null)
                    _bulkDeletedEntities = new List<object>();
                return _bulkDeletedEntities;
            }
        }

        private Dictionary<Type, List<Func<object, bool>>> _bulkDeletedFuncs;
        public Dictionary<Type, List<Func<object, bool>>> BulkDeletedFuncs
        {
            get
            {
                if (_bulkDeletedFuncs == null)
                    _bulkDeletedFuncs = new Dictionary<Type, List<Func<object, bool>>>();
                return _bulkDeletedFuncs;
            }
        }

        public void DeleteIn<TEntityObject, TInType>(ObjectSet<TEntityObject> entitySet,
            Expression<Func<TEntityObject, TInType>> peopertyExpression, IEnumerable<TInType> ids, int chunkSize = 100)
            where TEntityObject : EntityObject
        {
            var propertyName = GetExpressionPropertyName(peopertyExpression);
            entitySet.DeleteIn(propertyName, ids, chunkSize);
        }

        public void DeleteIn<TEntityObject, TInType>(ObjectSet<TEntityObject> entitySet,
            string propertyName, IEnumerable<TInType> ids, int chunkSize = 100)
            where TEntityObject : EntityObject
        {
            foreach (var chunk in ids.Chunk(chunkSize))
            {
                var objectQuery = (ObjectQuery)entitySet.Where(ContainsPredicate<TEntityObject, TInType>(chunk, propertyName));
                var sql = objectQuery.ToTraceString();
                string from = Regex.Match(sql, "FROM [\\[A-Za-z0-9\\] .]+\\] AS").Value;
                from = from.Substring(0, from.Length - 3);
                ExecuteStoreCommand(string.Format("DELETE EX {0} AS EX JOIN ({1}) AS EX1 ON EX.{2} = EX1.{2}", from, sql, propertyName));
            }

            SaveChanges();
            Flush();
        }

        Expression<Func<TEntity, bool>> ContainsPredicate<TEntity, TInType>(IEnumerable<TInType> arr, string fieldname) where TEntity : class
        {
            var entity = Expression.Parameter(typeof(TEntity), "entity");
            var member = Expression.Convert(Expression.Property(entity, fieldname), typeof(Int32));

            var containsMethods = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public).Where(m => m.Name == "Contains");
            var method = containsMethods.FirstOrDefault(m => m.GetParameters().Count() == 2);
            method = method.MakeGenericMethod(member.Type);
            var exprContains = Expression.Call(method, new Expression[] { Expression.Constant(arr), member });
            return Expression.Lambda<Func<TEntity, bool>>(exprContains, entity);
        }

        string GetExpressionPropertyName<TEntityObject, TInType>(Expression<Func<TEntityObject, TInType>> propertyExpression)
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
        
        public void Delete<TBase, T>(ObjectSet<TBase> entitySet, Expression<Func<T, bool>> predicate)
            where T : class, TBase
            where TBase : class
        {
            ObjectQuery<T> objectQuery = (ObjectQuery<T>)entitySet.OfType<T>().Where(predicate);
            string selectSQLQuery = objectQuery.ToTraceString();
            string from = Regex.Match(selectSQLQuery, "FROM [\\[A-Za-z0-9\\] .]+\\] AS").Value;
            from = from.Substring(0, from.Length - 3);
            IEnumerator<EdmMember> keyMembersEnumerator = entitySet.EntitySet.ElementType.KeyMembers.GetEnumerator();
            StringBuilder joinClause = new StringBuilder();
            keyMembersEnumerator.MoveNext();
            for (; ; )
            {
                joinClause.Append("MMExtent.");
                joinClause.Append(keyMembersEnumerator.Current);
                joinClause.Append(" = MMExtent2.");
                joinClause.Append(keyMembersEnumerator.Current);

                if (keyMembersEnumerator.MoveNext())
                    joinClause.Append(" AND ");
                else
                    break;
            }
            
            BulkDeletedActions.Add(() => ExecuteStoreCommand(string.Format("DELETE MMExtent {0} AS MMExtent INNER JOIN ({1}) AS MMExtent2 ON {2}", from, objectQuery.ToTraceString().Replace("@p__linq__", "@p"), joinClause.ToString()), objectQuery.Parameters.Select(p => p.Value).ToArray()));
            Func<T, bool> predicateCompiled = predicate.Compile();
            Func<object, bool> predicateCompiledObject = o =>
            {
                T t = o as T;
                if (t == null)
                    return false;
                return predicateCompiled(t);
            };
            List<Func<object, bool>> bulkDeletedFuncs;
            if (BulkDeletedFuncs.TryGetValue(typeof(TBase), out bulkDeletedFuncs))
                bulkDeletedFuncs.Add(predicateCompiledObject);
            else
                BulkDeletedFuncs.Add(typeof(TBase), new List<Func<object, bool>>() { predicateCompiledObject });
            foreach (var entity in ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Deleted | EntityState.Modified | EntityState.Unchanged).Select(ose => ose.Entity).OfType<T>().Where(e => predicateCompiled(e)))
            {
                DeleteObject(entity);
                BulkDeletedEntities.Add(entity);
            }
        }


        //private void NorthwindEntities_ObjectMaterialized(object sender, ObjectMaterializedEventArgs e)
        //{
        //    List<Func<object, bool>> bulkDeletedFuncs;
        //    if (_bulkDeletedFuncs != null)
        //    {
        //        Type t = e.Entity.GetType();
        //        do
        //        {
        //            if (BulkDeletedFuncs.TryGetValue(t, out bulkDeletedFuncs))
        //                foreach (Func<object, bool> bulkDeletedFunc in bulkDeletedFuncs)
        //                    if (bulkDeletedFunc(e.Entity))
        //                    {
        //                        ObjectStateManager.GetObjectStateEntry(e.Entity).Delete();
        //                        BulkDeletedEntities.Add(e.Entity);
        //                        return;
        //                    }
        //        } while ((t = t.BaseType) != null);
        //    }
        //}

        //public override int SaveChanges(SaveOptions options)
        //{
        //    int value;
        //    using (TransactionScope transaction = new TransactionScope())
        //    {
        //        if (_bulkDeletedEntities != null)
        //            foreach (object entity in _bulkDeletedEntities)
        //            {
        //                ObjectStateEntry ose;
        //                if (ObjectStateManager.TryGetObjectStateEntry(entity, out ose))
        //                    Detach(entity);
        //            }
        //        bool acceptChanges = (options & SaveOptions.AcceptAllChangesAfterSave) == SaveOptions.AcceptAllChangesAfterSave;
        //        if (acceptChanges)
        //            options ^= SaveOptions.AcceptAllChangesAfterSave;
        //        value = base.SaveChanges(options);
        //        if (_bulkDeletedActions != null)
        //            foreach (Action action in _bulkDeletedActions)
        //                action();
        //        transaction.Complete();
        //        if (acceptChanges)
        //            AcceptAllChanges();
        //    }
        //    return value;
        //}


        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //    if (disposing)
        //        ObjectMaterialized -= NorthwindEntities_ObjectMaterialized;
        //}
    }
}