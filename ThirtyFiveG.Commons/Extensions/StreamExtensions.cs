using System.IO;
using System.Text;

namespace ThirtyFiveG.Commons.Extensions
{
    public static class StreamExtensions
    {
        public static string AsString(this Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            string streamAsString = string.Empty;
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            streamAsString = reader.ReadToEnd();
            return streamAsString;
        }

        public static byte[] AsBytes(this Stream stream)
        {
            Stream data = new MemoryStream();

            stream.CopyTo(data);
            data.Seek(0, SeekOrigin.Begin);
            byte[] buf = new byte[data.Length];
            data.Read(buf, 0, buf.Length);

            return buf;
        }
    }
}
