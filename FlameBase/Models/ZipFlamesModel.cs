using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace FlameBase.Models
{
    public static class ZipFlamesModel
    {
        public static T DecompressObject<T>(string path)
        {
            try
            {
                var bytes = File.ReadAllBytes(path);
                var decompressedBytes = Decompress(bytes);
                var obj = ByteArrayToObject(decompressedBytes);
                return (T) obj;
            }
            catch (Exception)
            {
                return default;
            }
        }

        public static void CompressObject(object obj, string pathOut)
        {
            try
            {
                var bytes = ObjectToByteArray(obj);
                var compressedBytes = Compress(bytes);
                File.WriteAllBytes(pathOut, compressedBytes);
            }
            catch (Exception)
            {
                //
            }
        }


        public static void CompressDirectory(string sInDir, string sOutFile, ProgressDelegate progress = null)
        {
            var sFiles = Directory.GetFiles(sInDir, "*.*", SearchOption.AllDirectories);
            var iDirLen = sInDir[sInDir.Length - 1] == Path.DirectorySeparatorChar ? sInDir.Length : sInDir.Length + 1;

            using (var fileStream = new FileStream(sOutFile, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var gZipStream = new GZipStream(fileStream, CompressionMode.Compress))
            {
                foreach (var sFilePath in sFiles)
                {
                    var sRelativePath = sFilePath.Substring(iDirLen);
                    progress?.Invoke(sRelativePath);
                    CompressFile(sInDir, sRelativePath, gZipStream);
                }
            }
        }

        public static void DecompressToDirectory(string sCompressedFile, string sDir, ProgressDelegate progress = null)
        {
            using (var inFile = new FileStream(sCompressedFile, FileMode.Open, FileAccess.Read, FileShare.None))
            using (var zipStream = new GZipStream(inFile, CompressionMode.Decompress, true))
            {
                while (DecompressFile(sDir, zipStream, progress))
                {
                }
            }
        }

        private static void CompressFile(string sDir, string sRelativePath, GZipStream zipStream)
        {
            //Compress file name
            var chars = sRelativePath.ToCharArray();
            zipStream.Write(BitConverter.GetBytes(chars.Length), 0, sizeof(int));
            foreach (var c in chars)
                zipStream.Write(BitConverter.GetBytes(c), 0, sizeof(char));

            //Compress file content
            var bytes = File.ReadAllBytes(Path.Combine(sDir, sRelativePath));
            zipStream.Write(BitConverter.GetBytes(bytes.Length), 0, sizeof(int));
            zipStream.Write(bytes, 0, bytes.Length);
        }

        private static bool DecompressFile(string sDir, Stream zipStream, ProgressDelegate progress)
        {
            //Decompress file name
            var bytes = new byte[sizeof(int)];
            var read = zipStream.Read(bytes, 0, sizeof(int));
            if (read < sizeof(int))
                return false;

            var iNameLen = BitConverter.ToInt32(bytes, 0);
            bytes = new byte[sizeof(char)];
            var sb = new StringBuilder();
            for (var i = 0; i < iNameLen; i++)
            {
                zipStream.Read(bytes, 0, sizeof(char));
                var c = BitConverter.ToChar(bytes, 0);
                sb.Append(c);
            }

            var sFileName = sb.ToString();
            progress?.Invoke(sFileName);

            //Decompress file content
            bytes = new byte[sizeof(int)];
            zipStream.Read(bytes, 0, sizeof(int));
            var iFileLen = BitConverter.ToInt32(bytes, 0);

            bytes = new byte[iFileLen];
            zipStream.Read(bytes, 0, bytes.Length);

            var sFilePath = Path.Combine(sDir, sFileName);
            var sFinalDir = Path.GetDirectoryName(sFilePath);
            if (!Directory.Exists(sFinalDir))
                Directory.CreateDirectory(sFinalDir ?? throw new InvalidOperationException());

            using (var outFile = new FileStream(sFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                outFile.Write(bytes, 0, iFileLen);
            }

            return true;
        }


        private static byte[] Compress(byte[] raw)
        {
            using (var memory = new MemoryStream())
            {
                using (var gzip = new GZipStream(memory, CompressionLevel.Optimal))
                {
                    gzip.Write(raw, 0, raw.Length);
                }

                return memory.ToArray();
            }
        }

        private static byte[] Decompress(byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                zipStream.CopyTo(resultStream);
                return resultStream.ToArray();
            }
        }

        private static object ByteArrayToObject(byte[] arrBytes)
        {
            var memStream = new MemoryStream();
            var binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = binForm.Deserialize(memStream);
            return obj;
        }

        private static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public delegate void ProgressDelegate(string sMessage);
    }
}