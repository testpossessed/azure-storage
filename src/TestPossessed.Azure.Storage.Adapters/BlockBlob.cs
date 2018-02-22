using System;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;

namespace TestPossessed.Azure.Storage.Adapters
{
    public class BlockBlob : IBlockBlob
    {
        private readonly CloudBlockBlob cloudBlockBlob;

        internal BlockBlob(CloudBlockBlob cloudBlockBlob)
        {
            this.cloudBlockBlob = cloudBlockBlob;
        }

        public string DownloadText()
        {
            return this.cloudBlockBlob.DownloadText();
        }

        public void DownloadToStream(Stream stream)
        {
            this.cloudBlockBlob.DownloadToStream(stream);
        }

        public bool Exists()
        {
            return this.cloudBlockBlob.Exists();
        }

        public string StartCopy(IBlockBlob source)
        {
            var sourceBlob = (BlockBlob)source;
            return this.cloudBlockBlob.StartCopy(sourceBlob.InnerBlob());
        }

        public void UploadFromStream(Stream stream)
        {
            this.cloudBlockBlob.UploadFromStream(stream);
        }

        public void UploadText(string text)
        {
            this.cloudBlockBlob.UploadText(text);
        }

        public Uri Uri => this.cloudBlockBlob.Uri;

        internal CloudBlockBlob InnerBlob()
        {
            return this.cloudBlockBlob;
        }
    }
}