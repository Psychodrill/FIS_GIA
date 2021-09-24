using System;
using System.Collections.Generic;
using System.Linq;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Bulk.Validators
{
    /// <summary>
    ///     Номер заявления не уникален
    /// </summary>
    public class ApplicationNumberIsNotUniqueValidatorAttribute : DtoValidatorBaseAttribute
    {
        public override int ErrorCode
        {
            get { return ConflictMessages.ApplicationNumberIsNotUnique; }
        }

        public override void Validate(object dto, object obj, object[] datasource)
        {
            var collection = obj as ApplicationDto[];
            if (collection == null) throw new ArgumentException("Ожидалась коллекция объектов типа ApplicationDto[]");

            List<string> duplicated = (from c in collection
                                       group c by c.ApplicationNumber
                                       into g
                                       where g.Count() > 1
                                       select g.Key).ToList();

            foreach (string appnumber in duplicated)
            {
                foreach (ApplicationDto app in collection.Where(c => c.ApplicationNumber == appnumber))
                {
                    app.IsBroken = true;
                    app.ErrorMessages.Add(ErrorMessage);
                }
            }
        }
    }
}