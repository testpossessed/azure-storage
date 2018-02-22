using System;
using System.IO;

namespace TestPossessed.Azure.Storage.Adapters
{
    public interface IBlockBlob
    {
        Uri Uri { get; }
        string DownloadText();
        void DownloadToStream(Stream stream);
        bool Exists();
        string StartCopy(IBlockBlob source);
        void UploadFromStream(Stream stream);
        void UploadText(string text);
    }
}