using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GVUZ.ServiceModel.Import.Bulk.Model.Base;

namespace GVUZ.ServiceModel.Import.Bulk.Extensions
{
    public static class MapperExtensions
    {
        public static T MappingTo<T>(this object item)
        {
            return (T) Mapper.Map(item, item.GetType(), typeof (T));
        }

        public static IEnumerable<T> MappingTo<T>(this IEnumerable<object> items)
        {
            if (!items.Any()) return Enumerable.Empty<T>();
            return (IEnumerable<T>) Mapper.Map(items, items.GetType(), typeof (IEnumerable<T>));
        }

        /// <summary>
        ///     Родитель источника маппится на приемника
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="mappingExpression"></param>
        public static void InheritMappingFromSourceBaseType<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> mappingExpression)
        {
            Type sourceType = typeof (TSource);
            Type desctinationType = typeof (TDestination);
            Type sourceParentType = sourceType.BaseType;

            mappingExpression
                .BeforeMap((x, y) => Mapper.Map(x, y, sourceParentType, desctinationType))
                .ForAllMembers(x => x.Condition(r => NotAlreadyMapped(sourceParentType, desctinationType, r)));
        }

        /// <summary>
        ///     Источник маппится на родителя приемника
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="mappingExpression"></param>
        public static void InheritMappingFromDestinationBaseType<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> mappingExpression)
        {
            Type sourceType = typeof (TSource);
            Type desctinationType = typeof (TDestination);
            Type desctinationParentType = desctinationType.BaseType;

            mappingExpression
                .BeforeMap((x, y) => Mapper.Map(x, y, sourceType, desctinationParentType))
                .ForAllMembers(x => x.Condition(r => NotAlreadyMapped(sourceType, desctinationParentType, r)));
        }

        /// <summary>
        ///     Родитель источника на родителя приемника
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="mappingExpression"></param>
        public static void InheritMappingFromBothBaseTypes<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> mappingExpression)
        {
            Type sourceType = typeof (TSource);
            Type desctinationType = typeof (TDestination);
            Type sourceParentType = sourceType.BaseType;
            Type destParentType = desctinationType.BaseType;

            mappingExpression
                .BeforeMap((x, y) => Mapper.Map(x, y, sourceParentType, destParentType))
                .ForAllMembers(x => x.Condition(r => NotAlreadyMapped(sourceParentType, destParentType, r)));
        }

        private static bool NotAlreadyMapped(Type sourceType, Type desitnationType, ResolutionContext r)
        {
            return !r.IsSourceValueNull &&
                   Mapper.FindTypeMapFor(sourceType, desitnationType).GetPropertyMaps().Where(
                       m => m.DestinationProperty.Name.Equals(r.MemberName)).Select(y => !y.IsMapped()).All(b => b);
        }

        public static T ToBulkEntity<T>(this object item, int importPackageId, int institutionId) where T : IBulkEntity
        {
            return item.ToBulkEntity<T>(importPackageId, institutionId, null);
        }

        public static T ToBulkEntity<T>(this object item, int importPackageId, int institutionId, Guid? parentId)
            where T : IBulkEntity
        {
            var mapped = (T) Mapper.Map(item, item.GetType(), typeof (T));
            mapped.ImportPackageId = importPackageId;
            mapped.InstitutionId = institutionId;
            mapped.ParentId = parentId;
            return mapped;
        }

        public static List<T> ToBulkEntity<T>(this IEnumerable<object> items, int importPackageId, int institutionId)
            where T : IBulkEntity
        {
            return items.ToBulkEntity<T>(importPackageId, institutionId, null);
        }

        public static List<T> ToBulkEntity<T>(this IEnumerable<object> items, int importPackageId, int institutionId,
                                              Guid? parentId) where T : IBulkEntity
        {
            if (!items.Any()) return new List<T>();
            var mapped = (List<T>) Mapper.Map(items, items.GetType(), typeof (IEnumerable<T>));
            mapped.ForEach(c =>
                {
                    c.ImportPackageId = importPackageId;
                    c.InstitutionId = institutionId;
                    c.ParentId = parentId;
                });
            return mapped;
        }
    }
}