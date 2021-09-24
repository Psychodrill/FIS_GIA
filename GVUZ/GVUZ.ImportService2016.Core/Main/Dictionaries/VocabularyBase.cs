using GVUZ.ImportService2016.Core.Dto.Import;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class VocabularyBase<T>
        where T: VocabularyBaseDto, new()
    {
        protected readonly List<T> items = new List<T>();
        public List<T> Items
        {
            get
            { return items.Where(t=> !t.IsDeleted).ToList(); }
        }

        public VocabularyBase(DataTable dataTable)
        {
            if (dataTable == null) return;
            items = BindDataList<T>(dataTable);
        }

        /// <summary>
        /// Получить сущность по uid
        /// </summary>
        /// <param name="uid"></param>
        /// <returns>сущность или null</returns>
        public T GetItemByUid(string uid)
        {
            return items.Where(t => t.UID == uid).FirstOrDefault();
        }

        /// <summary>
        /// Получить id записи по uid
        /// </summary>
        /// <param name="uid"></param>
        /// <returns>int id или 0, если запись не найдена</returns>
        public int GetIdByUid(string uid)
        {
            var item = GetItemByUid(uid);
            return item != null ? item.ID : 0;
        }

        public string GetNameByID(int id)
        {
            var item = items.Where(t => t.ID == id).FirstOrDefault();
            return item != null ? item.Name : "";
        }

        public void AddItems(DataTable dataTable)
        {
            if (dataTable == null) return;
            var newItems = BindDataList<T>(dataTable);

            foreach (var newItem in newItems)
            {
                var curItem = items.Where(t => t.ID == newItem.ID).FirstOrDefault();

                if (curItem != null)
                    items.Remove(curItem);
                items.Add(newItem);
            }
        }

        private List<T2> BindDataList<T2>(DataTable dt)
        {
            List<string> columns = new List<string>();
            foreach (DataColumn dc in dt.Columns)
            {
                columns.Add(dc.ColumnName);
            }

            var fields = typeof(T2).GetFields();
            var properties = typeof(T2).GetProperties();

            List<T2> lst = new List<T2>();

            foreach (DataRow dr in dt.Rows)
            {
                var ob = Activator.CreateInstance<T2>();

                //foreach (var fieldInfo in fields)
                //{
                //    if (columns.Contains(fieldInfo.Name))
                //    {
                //        fieldInfo.SetValue(ob, dr[fieldInfo.Name]);
                //    }
                //}

                foreach (var propertyInfo in properties)
                {
                    if (columns.Contains(propertyInfo.Name))
                    {
                        if (dr[propertyInfo.Name] != System.DBNull.Value)
                            propertyInfo.SetValue(ob, dr[propertyInfo.Name], null);
                        else
                            propertyInfo.SetValue(ob, null, null);
                    }
                    else if (!(new string[]{"ID", "UID", "IsDeleted", "GUID", "Name"}).Contains(propertyInfo.Name))
                    {
                        string msg = string.Format("==> В справочнике: {0} (таблица {1}) не найдено поле: {2}", typeof(T2).Name, dt.TableName, propertyInfo.Name);
                        Debug.WriteLine(msg);
                        //Debug.WriteLine("==> В справочнике: " + typeof(T2).Name + " не найдено поле: "  + propertyInfo.Name);
                    }
                }

                lst.Add(ob);
            }

            return lst;
        }
    }

    public class VocabularyBaseDto : ImportBase
    {
        public virtual new int ID { get; set; }
        public string UID { get; set; }
        public virtual string Name { get; set; }

        public virtual bool IsDeleted { get; set; }
    }
}
