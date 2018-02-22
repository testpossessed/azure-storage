using System;
using System.IO;

namespace TestPossessed.Azure.Storage
{
    public interface IBlockBlobWriter
    {
        Uri Write(Stream stream, string key);
        Uri Write(string text, string key);
    }
}