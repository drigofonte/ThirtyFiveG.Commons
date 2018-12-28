using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using System.Text;

namespace ThirtyFiveG.Commons.Extensions
{
    public static class ByteExtensions
    {
        public static string Unzip(this byte[] content)
        {
            byte[] buffer;
            using (MemoryStream ms = new MemoryStream(content))
            {
                ms.Seek(0, SeekOrigin.Begin);
                using (ZipInputStream str = new ZipInputStream(ms, (int)ms.Length))
                {
                    str.IsStreamOwner = false;
                    str.GetNextEntry();
                    using (MemoryStream stream = new MemoryStream())
                    {
                        str.CopyTo(stream);
                        buffer = new byte[stream.Length];
                        buffer = stream.ToArray();
                    }
                }
            }
            return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
        }
    }
}
