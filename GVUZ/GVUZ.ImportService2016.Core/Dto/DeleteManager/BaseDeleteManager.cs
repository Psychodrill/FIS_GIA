using GVUZ.ImportService2016.Core.Main.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Dto.DeleteManager
{
    public abstract class BaseDeleteManager<T>
        where T : VocabularyBaseDto, new()
    {
        protected T dto;
        public T Dto { get { return dto; } }
        //public List<BaseDeleteManager<VocabularyBaseDto>> Children = new List<BaseDeleteManager<VocabularyBaseDto>>();
        //public BaseDeleteManager<VocabularyBaseDto> Parent = null;

        protected VocabularyStorage vocabularyStorage;

        public BaseDeleteManager(T _dto, VocabularyStorage _vocabularyStorage)
        {
            dto = _dto;
            vocabularyStorage = _vocabularyStorage;

            //CheckDependency();
        }

        /// <summary>
        /// Проверка внешних зависимостей. 
        /// </summary>
        /// <returns>True - нет зависимостей и можно удалять или обновлять. False - есть зависимости, см. соответсвующие коллекции</returns>
        public abstract bool CheckDependency();

        public virtual void FillChildren(){}

        //public bool HasDependency
        //{
        //    get
        //    {
                
        //        if (!CheckDependency()) return true;
        //        foreach (var child in Children)
        //        {
        //            if (child.HasDependency)
        //                return true;
        //        }

        //        return false;
        //    }
        //}

        public virtual void MarkDelete()
        {
            dto.IsDeleted = true;
        }


        public virtual List<VocabularyBaseDto> GetDeleteObjects()
        {
            return new List<VocabularyBaseDto>() { dto };
        }
    }
}
