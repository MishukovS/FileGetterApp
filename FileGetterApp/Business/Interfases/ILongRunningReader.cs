namespace FileGetterApp.Business.Interfases
{
    public interface ILongRunningReader
    {
        byte[] Read(string fileName);
    }
}
