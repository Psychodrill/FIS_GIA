namespace GVUZ.Model.Institutions
{
    public partial class InstitutionItem
    {
        /// <summary>
        ///     �� ������������ � LINQ!
        /// </summary>
        public InstitutionItemType ItemType
        {
            // ���� ������� ��������� � LINQ, ItemTypeID ������� protected/private
            get { return (InstitutionItemType) ItemTypeID; }
            set { ItemTypeID = (short) value; }
        }
    }
}