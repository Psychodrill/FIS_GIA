namespace GVUZ.DAL.Dto
{

    /// <summary>
    /// Описание направления подготовки
    /// </summary>
    public interface IDirectionDescription
    {
        //string Code { get; }

        string NewCode { get; }

        string QualificationCode { get; }

        string QualificationName { get; }

        string DirectionName { get; }

        int EducationLevelId { get; }

        string EducationLevelName { get; }
    }
}
