using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FBS.Replicator.Helpers
{
    /// <summary>
    /// Взято из MFtcUtils
    /// </summary>
    public static class AssemblyInfo
    {
        private static Data data = new Data();
        public class Data
        {
            /// <summary>
            /// Заголовок
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// Описание
            /// </summary>
            public string Description { get; set; }
            /// <summary>
            /// Компания
            /// </summary>
            public string Company { get; set; }
            /// <summary>
            /// Наименование продукта
            /// </summary>
            public string Product { get; set; }
            /// <summary>
            /// Копирайт
            /// </summary>
            public string Copyright { get; set; }
            public string Trademark { get; set; }
            /// <summary>
            /// Версия сборки
            /// </summary>
            public System.Version AssemblyVersion;
            /// <summary>
            /// Версия файла
            /// </summary>
            public System.Version FileVersion;
            /// <summary>
            /// Guid приложения
            /// </summary>
            public System.Guid Guid { get; set; }
            /// <summary>
            /// Локаль
            /// </summary>
            public string NeutralLanguage { get; set; }
            /// <summary>
            /// Видимость сборки как COM-библиотеки
            /// </summary>
            public bool IsComVisible { get; set; }
        }


        private static T GetAssemblyAttribute<T>(System.Reflection.Assembly assembly) where T : Attribute
        {
            T attribute = default(T);
            object[] attributes = assembly.GetCustomAttributes(typeof(T), true);

            if (attributes != null && attributes.Length > 0)
            {
                attribute = (T)attributes[0];
            }
            return attribute;
        }

        public static Data Get(System.Reflection.Assembly assembly)
        {
            System.Reflection.AssemblyTitleAttribute ttl_attr = AssemblyInfo.GetAssemblyAttribute<System.Reflection.AssemblyTitleAttribute>(assembly);
            if (ttl_attr != null)
                data.Title = ttl_attr.Title;

            System.Reflection.AssemblyDescriptionAttribute a_attr = AssemblyInfo.GetAssemblyAttribute<System.Reflection.AssemblyDescriptionAttribute>(assembly);
            if (a_attr != null)
                data.Description = a_attr.Description;

            System.Reflection.AssemblyCompanyAttribute cmp_attr = AssemblyInfo.GetAssemblyAttribute<System.Reflection.AssemblyCompanyAttribute>(assembly);
            if (cmp_attr != null)
                data.Company = cmp_attr.Company;

            System.Reflection.AssemblyProductAttribute p_attr = AssemblyInfo.GetAssemblyAttribute<System.Reflection.AssemblyProductAttribute>(assembly);
            if (p_attr != null)
                data.Product = p_attr.Product;

            System.Reflection.AssemblyCopyrightAttribute cpr_attr = AssemblyInfo.GetAssemblyAttribute<System.Reflection.AssemblyCopyrightAttribute>(assembly);
            if (cpr_attr != null)
                data.Copyright = cpr_attr.Copyright;
            System.Reflection.AssemblyTrademarkAttribute trm_attr = AssemblyInfo.GetAssemblyAttribute<System.Reflection.AssemblyTrademarkAttribute>(assembly);
            if (trm_attr != null)
                data.Trademark = trm_attr.Trademark;

            data.AssemblyVersion = assembly.GetName().Version;

            System.Reflection.AssemblyFileVersionAttribute flv_attr = AssemblyInfo.GetAssemblyAttribute<System.Reflection.AssemblyFileVersionAttribute>(assembly);
            if (flv_attr != null)
                data.FileVersion = new Version(flv_attr.Version);
            System.Runtime.InteropServices.GuidAttribute g_attr = AssemblyInfo.GetAssemblyAttribute<System.Runtime.InteropServices.GuidAttribute>(assembly);
            if (g_attr != null)
                data.Guid = new Guid(g_attr.Value);
            System.Resources.NeutralResourcesLanguageAttribute l_attr = AssemblyInfo.GetAssemblyAttribute<System.Resources.NeutralResourcesLanguageAttribute>(assembly);
            if (l_attr != null)
                data.NeutralLanguage = l_attr.CultureName;
            System.Runtime.InteropServices.ComVisibleAttribute com_attr = AssemblyInfo.GetAssemblyAttribute<System.Runtime.InteropServices.ComVisibleAttribute>(assembly);
            if (com_attr != null)
                data.IsComVisible = com_attr.Value;
            return data;
        }
    }
}
