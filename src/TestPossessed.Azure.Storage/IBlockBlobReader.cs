namespace TestPossessed.Azure.Storage
{
    public interface IBlockBlobReader
    {
        string ReadText(string key);
    }
}