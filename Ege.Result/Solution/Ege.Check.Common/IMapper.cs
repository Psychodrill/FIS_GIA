namespace Ege.Check.Common
{
    public interface IMapper<in TFrom, out TTo>
    {
        TTo Map(TFrom from);
    }
}