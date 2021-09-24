namespace GVUZ.DAL.Dto
{
    /// <summary>
    /// Простой DTO для хранения пары int + наименование
    /// </summary>
    public class SimpleDto : SimpleDtoBase<int>
    {
        public SimpleDto()
        {
        }

        public SimpleDto(int id, string name) : base(id, name)
        {
        }
    }
}
