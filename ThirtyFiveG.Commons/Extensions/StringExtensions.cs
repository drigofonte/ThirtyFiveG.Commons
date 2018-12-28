using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ThirtyFiveG.Commons.Extensions
{
    public static class StringExtensions
    {
        #region Public methods
        public static string GetValueOrEmpty(this string s, bool isLower = false)
        {
            return s != null ? isLower ? s.ToLower() : s : string.Empty;
        }

        public static Expression ToExpression(this string json, Assembly[] assemblies)
        {
            return JsonConvert.DeserializeObject<LambdaExpression>(json, ExpressionExtensions.GetSettings(assemblies));
        }

        public static byte[] Zip(this string content)
        {
            byte[] bytes;
            byte[] dataBuffer = new byte[4096];
            bytes = Encoding.UTF8.GetBytes(content);
            using (MemoryStream ms = new MemoryStream())
            {
                using (ZipOutputStream str = new ZipOutputStream(ms))
                {
                    str.SetLevel(9);
                    ZipEntry entry = new ZipEntry(DateTime.Now.Ticks.ToString());
                    entry.DateTime = DateTime.Now;
                    str.PutNextEntry(entry);
                    using (MemoryStream stream = new MemoryStream(bytes))
                    {
                        StreamUtils.Copy(stream, str, dataBuffer);
                    }
                    str.CloseEntry();
                    str.IsStreamOwner = false;
                    ms.Position = 0;
                    return ms.ToArray();
                }
            }
        }

        public static byte[] ToBytes(this string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static Stream ToStream(this string str)
        {
            return ToMemoryStream(str);
        }

        public static MemoryStream ToMemoryStream(this string str)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(str);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public static string SplitCamelCaseString(this string str)
        {
            List<char> chars = new List<char>();
            chars.Add(str[0]);
            for (int i = 1; i < str.Length; i++)
            {
                char c = str[i];
                if (char.IsUpper(c) && char.IsLower(str[i - 1]) && (i + 1 == str.Length - 1 || char.IsLower(str[i + 1])))
                    chars.Add(' ');
                chars.Add(c);
            }

            return new string(chars.ToArray());
        }
        #endregion
    }
}
