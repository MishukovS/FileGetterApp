namespace FileGetterApp.Business.Interfases
{
    public interface IFileGetterWithCache
    {
        byte[] Get(string fileName);
    }
}
