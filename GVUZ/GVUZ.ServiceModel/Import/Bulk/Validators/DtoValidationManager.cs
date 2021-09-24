using System;
using System.Linq;
using System.Reflection;

namespace GVUZ.ServiceModel.Import.Bulk.Validators
{
    public class ValidationManager
    {
        /// <summary>
        ///     Валидация dto объекта
        /// </summary>
        /// <returns></returns>
        public bool Validate(object obj, object[] datasource)
        {
            ValidateFields(obj, datasource);
            return true;
        }

        /// <summary>
        ///     Валидация полей объекта
        /// </summary>
        /// <returns></returns>
        private void ValidateFields(object obj, object[] datasource)
        {
            foreach (FieldInfo finfo in obj.GetType().GetFields())
            {
                /* Извлекаем значение поля */
                object fvalue = finfo.GetValue(obj);
                if (fvalue == null) continue;

                /* Запускаем все валидаторы, которые висят на поле */
                finfo.GetCustomAttributes(typeof (DtoValidatorBaseAttribute), true).AsParallel().ForAll(c =>
                                                                                                        ((
                                                                                                         DtoValidatorBaseAttribute
                                                                                                         ) c).Validate(
                                                                                                             obj, fvalue,
                                                                                                             datasource));

                if (IsPrimitiveType(finfo.FieldType))
                    continue;

                if (finfo.FieldType.IsArray)
                    ((object[]) fvalue).AsParallel().ForAll(c => ValidateFields(c, datasource));
                else
                    ValidateFields(fvalue, datasource);
            }

            if (!IsPrimitiveType(obj.GetType()))
            {
                foreach (PropertyInfo pinfo in obj.GetType().GetProperties().Where(c =>
                                                                                   c.GetCustomAttributes(
                                                                                       typeof (DtoValidatorBaseAttribute
                                                                                           ), true)
                                                                                    .Any()))
                {
                    object pvalue = pinfo.GetValue(obj, null);
                    if (pvalue == null) continue;

                    /* Запускаем все валидаторы, которые висят на поле */
                    pinfo.GetCustomAttributes(typeof (DtoValidatorBaseAttribute), true).AsParallel().ForAll(c =>
                                                                                                            ((
                                                                                                             DtoValidatorBaseAttribute
                                                                                                             ) c)
                                                                                                                .Validate
                                                                                                                (obj,
                                                                                                                 pvalue,
                                                                                                                 datasource));

                    if (IsPrimitiveType(pinfo.GetType()))
                        continue;

                    if (pinfo.PropertyType.IsArray)
                        ((object[]) pvalue).AsParallel().ForAll(c => ValidateFields(c, datasource));
                    else
                        ValidateFields(pvalue, datasource);
                }
            }
        }

        /// <summary>
        ///     Выявление простых типов, для исключения их из рекурсии
        /// </summary>
        /// <returns></returns>
        public bool IsPrimitiveType(Type type)
        {
            if (type == typeof (string))
                return true;

            if (type == typeof (bool))
                return true;

            if (type == typeof (int))
                return true;

            if (type == typeof (decimal))
                return true;

            return false;
        }
    }
}