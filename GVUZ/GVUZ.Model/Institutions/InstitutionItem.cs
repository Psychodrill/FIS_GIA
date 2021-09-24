namespace GVUZ.Model.Institutions
{
    public partial class InstitutionItem
    {
        /// <summary>
        ///     Не использовать в LINQ!
        /// </summary>
        public InstitutionItemType ItemType
        {
            // если сделаем поддержку в LINQ, ItemTypeID сделать protected/private
            get { return (InstitutionItemType) ItemTypeID; }
            set { ItemTypeID = (short) value; }
        }
    }
}