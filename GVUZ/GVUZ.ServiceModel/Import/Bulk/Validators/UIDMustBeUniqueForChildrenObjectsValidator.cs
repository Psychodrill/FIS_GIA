using System.Collections.Generic;
using System.Linq;
using FogSoft.Helpers;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Bulk.Validators
{
    /// <summary>
    ///     Объект содержит коллекцию элементов ({0}), у которых есть дубликаты по UID ({1}).
    /// </summary>
    public class UIDMustBeUniqueForChildrenObjectsValidatorAttribute : DtoValidatorBaseAttribute
    {
        public override int ErrorCode
        {
            get { return ConflictMessages.UIDMustBeUniqueForChildrenObjects; }
        }

        public override void Validate(object dto, object obj, object[] datasource)
        {
            var collection = obj as BaseDto[];
            if (collection == null)
            {
                /* Собираем коллекцию dto которые содержатся внутри */
            }

            List<string> duplicatedUIDs = (from c in collection
                                           group c by c.UID
                                           into g where g.Count() > 1
                                           select g.Key).ToList();

            foreach (string uid in duplicatedUIDs)
            {
                foreach (BaseDto app in collection.Where(c => c.UID == uid))
                {
                    app.IsBroken = true;
                    app.ErrorMessages.Add(string.Format(ErrorMessage, obj.GetDescription(), uid));
                }
            }
        }
    }
}