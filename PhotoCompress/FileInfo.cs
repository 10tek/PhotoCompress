using System.IO;

namespace PhotoCompress
{
    public class FileInfo
    {
        public byte[] PhotoBytes { get; set; }
        public string FileName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Coordinates { get; set; }
        public string Tag { get; set; }

        public FileInfo()
        {
            PhotoBytes = null;
        }

        public FileInfo(string path)
        {
            PhotoBytes = File.ReadAllBytes(path);
        }
    }
}

