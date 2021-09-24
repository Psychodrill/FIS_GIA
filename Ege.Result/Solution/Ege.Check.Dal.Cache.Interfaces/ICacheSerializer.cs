namespace Ege.Check.Dal.Cache.Interfaces
{
    public interface ICacheSerializer
    {
        string Serialize(object input);
        T Deserialize<T>(string input);
    }
}
