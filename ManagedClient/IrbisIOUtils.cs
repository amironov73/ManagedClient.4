// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisIOUtils.cs
 */

#region Using directives

using System;
using System.IO;
using System.IO.Compression;

using JetBrains.Annotations;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// 
    /// </summary>
    public static class IrbisIOUtils
    {
        #region Public methods

        /// <summary>
        /// Converts network-ordered integer to host-ordered one.
        /// </summary>
        public static void NetworkToHost16
            (
                byte[] array, 
                int offset
            )
        {
            byte temp = array[offset];
            array[offset] = array[offset + 1];
            array[offset + 1] = temp;
        }

        /// <summary>
        /// Converts network-ordered integer to host-ordered one.
        /// </summary>
        public static void NetworkToHost32
            (
                byte[] array, 
                int offset
            )
        {
            byte temp1 = array[offset];
            byte temp2 = array[offset + 1];
            array[offset] = array[offset + 3];
            array[offset + 1] = array[offset + 2];
            array[offset + 3] = temp1;
            array[offset + 2] = temp2;
        }

        /// <summary>
        /// Converts network-ordered integer to host-ordered one.
        /// </summary>
        public static void NetworkToHost64
            (
                byte[] array, 
                int offset
            )
        {
            NetworkToHost32(array,offset);
            NetworkToHost32(array,offset+4);
        }

        /// <summary>
        /// Reads network-ordered integer from the stream.
        /// </summary>
        public static short ReadInt16Network
            (
                this Stream stream
            )
        {
            byte[] buffer = new byte[2];

            int readed = stream.Read(buffer, 0, 2);
            if (readed != 2)
            {
                throw new IOException();
            }
            NetworkToHost16(buffer,0);
            short result = BitConverter.ToInt16(buffer, 0);
            return result;
        }

        /// <summary>
        /// Reads network-ordered integer from the stream.
        /// </summary>
        public static short ReadInt16Host
            (
                this Stream stream
            )
        {
            byte[] buffer = new byte[2];

            int readed = stream.Read(buffer, 0, 2);
            if (readed != 2)
            {
                throw new IOException();
            }
            short result = BitConverter.ToInt16(buffer, 0);
            return result;
        }

        /// <summary>
        /// Reads network-ordered integer from the stream.
        /// </summary>
        public static int ReadInt32Network 
            ( 
                this Stream stream 
            )
        {
            byte[] buffer = new byte[4];

            int readed = stream.Read ( buffer, 0, 4 );
            if ( readed != 4 )
            {
                throw new IOException();
            }
            NetworkToHost32(buffer,0);
            int result = BitConverter.ToInt32 ( buffer, 0 );
            return result;
        }

        /// <summary>
        /// Reads network-ordered integer from the stream.
        /// </summary>
        public static int ReadInt32Host
            (
                this Stream stream
            )
        {
            byte[] buffer = new byte[4];

            int readed = stream.Read(buffer, 0, 4);
            if (readed != 4)
            {
                throw new IOException();
            }
            int result = BitConverter.ToInt32(buffer, 0);

            return result;
        }

        /// <summary>
        /// Reads network-ordered integer from the stream.
        /// </summary>
        public static long ReadInt64Network 
            ( 
                this Stream stream 
            )
        {
            byte[] buffer = new byte[8];

            int readed = stream.Read ( buffer, 0, 8 );
            if ( readed != 8 )
            {
                throw new IOException();
            }
            NetworkToHost64(buffer, 0);
            long result = BitConverter.ToInt64(buffer,0);
            return result;
        }

        /// <summary>
        /// Reads network-ordered integer from the stream.
        /// </summary>
        public static long ReadInt64Host
            (
                this Stream stream
            )
        {
            byte[] buffer = new byte[8];

            int readed = stream.Read(buffer, 0, 8);
            if (readed != 8)
            {
                throw new IOException();
            }
            long result = BitConverter.ToInt64(buffer,0);
            return result;
        }

        /// <summary>
        /// Read some bytes from the stream.
        /// </summary>
        public static byte[] ReadBytes 
            ( 
                this Stream stream, 
                int count 
            )
        {
            byte[] result = new byte[count];
            int read = stream.Read ( result, 0, count );
            if ( read <= 0 )
            {
                return null;
            }
            if ( read != count )
            {
                Array.Resize ( ref result, read );
            }
            return result;
        }

        /// <summary>
        /// Reads string from the stream.
        /// </summary>
        [CanBeNull]
        public static string ReadNullableString
            (
                [NotNull] this BinaryReader reader
            )
        {
            bool flag = reader.ReadBoolean();
            return flag
                ? reader.ReadString()
                : null;
        }

        /// <summary>
        /// Writes specified text to the stream.
        /// </summary>
        [NotNull]
        public static BinaryWriter WriteNullable
            (
                [NotNull] this BinaryWriter writer,
                [CanBeNull] string text
            )
        {
            if (text != null)
            {
                writer.Write(true);
                writer.Write(text);
            }
            else
            {
                writer.Write(false);
            }

            return writer;
        }

        /// <summary>
        /// Write 32-bit integer in packed format.
        /// </summary>
        /// <remarks>Borrowed from
        /// http://referencesource.microsoft.com/
        /// </remarks>
        public static BinaryWriter WritePackedInt32
            (
                [NotNull] this BinaryWriter writer,
                int value
            )
        {
            uint v = (uint)value;
            while (v >= 0x80)
            {
                writer.Write((byte)(v | 0x80));
                v >>= 7;
            }
            writer.Write((byte)v);

            return writer;
        }

        /// <summary>
        /// Read 32-bit integer in packed format.
        /// </summary>
        /// <remarks>Borrowed from
        /// http://referencesource.microsoft.com/
        /// </remarks>
        public static int ReadPackedInt32
            (
                [NotNull] this BinaryReader reader
            )
        {
            int count = 0;
            int shift = 0;
            byte b;
            do
            {
                if (shift == 5 * 7)
                {
                    throw new FormatException();
                }

                b = reader.ReadByte();
                count |= (b & 0x7F) << shift;
                shift += 7;
            } while ((b & 0x80) != 0);
            return count;
        }

        /// <summary>
        /// Сохранение в поток обнуляемого объекта.
        /// </summary>
        public static BinaryWriter WriteNullable<T>
            (
                [NotNull] this BinaryWriter writer,
                [CanBeNull] T obj
            )
            where T: class, IHandmadeSerializable
        {
            if (ReferenceEquals(obj, null))
            {
                writer.Write(false);
            }
            else
            {
                writer.Write(true);
                obj.SaveToStream(writer);
            }

            return writer;
        }

        /// <summary>
        /// Считывание из потока обнуляемого объекта.
        /// </summary>
        [CanBeNull]
        public static T ReadNullable<T>
            (
                [NotNull] this BinaryReader reader,
                [NotNull] Func<BinaryReader,T> func
            )
            where T: class
        {
            bool isNull = !reader.ReadBoolean();
            if (isNull)
            {
                return null;
            }

            T result = func(reader);

            return result;
        }

        /// <summary>
        /// Сохранение в поток массива элементов.
        /// </summary>
        public static void SaveToStream<T>
            (
                [CanBeNull][ItemNotNull] this T[] array,
                [NotNull] BinaryWriter writer
            )
            where T : IHandmadeSerializable
        {
            if (ReferenceEquals(array, null))
            {
                writer.Write(false);
            }
            else
            {
                writer.Write(true);
                writer.WritePackedInt32(array.Length);
                foreach (T item in array)
                {
                    item.SaveToStream(writer);
                }
            }
        }

        /// <summary>
        /// Сохранение в файл массива объектов,
        /// умеющих сериализоваться вручную.
        /// </summary>
        public static void SaveToFile<T>
            (
                [NotNull] [ItemNotNull] this T[] array,
                [NotNull] string fileName
            )
            where T: IHandmadeSerializable
        {
            using (Stream stream = File.Create(fileName))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                array.SaveToStream(writer);
            }
        }

        /// <summary>
        /// Сохранение в файл массива объектов
        /// с одновременной упаковкой.
        /// </summary>
        public static void SaveToZipFile<T>
            (
                [NotNull] [ItemNotNull] this T[] array,
                [NotNull] string fileName
            )
            where T : IHandmadeSerializable
        {
            using (Stream stream = File.Create(fileName))
            using (DeflateStream deflate = new DeflateStream
                (
                    stream,
                    CompressionMode.Compress
                ))
            using (BinaryWriter writer = new BinaryWriter(deflate))
            {
                array.SaveToStream(writer);
            }
        }

        /// <summary>
        /// Сохранение массива объектов.
        /// </summary>
        public static byte[] SaveToMemory<T>
            (
                [NotNull][ItemNotNull] this T[] array
            )
            where T: IHandmadeSerializable
        {
            using (MemoryStream stream = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                array.SaveToStream(writer);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Сохранение массива объектов.
        /// </summary>
        public static byte[] SaveToZipMemory<T>
            (
                [NotNull][ItemNotNull] this T[] array
            )
            where T : IHandmadeSerializable
        {
            using (MemoryStream stream = new MemoryStream())
            using (DeflateStream deflate = new DeflateStream
                (
                    stream,
                    CompressionMode.Compress
                ))
            using (BinaryWriter writer = new BinaryWriter(deflate))
            {
                array.SaveToStream(writer);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Считывание массива из потока.
        /// </summary>
        [CanBeNull]
        [ItemNotNull]
        public static T[] ReadArray<T>
            (
                [NotNull] this BinaryReader reader,
                [NotNull] Func<BinaryReader, T> func
            )
        {
            bool isNull = !reader.ReadBoolean();
            if (isNull)
            {
                return null;
            }

            int count = reader.ReadPackedInt32();
            T[] result = new T[count];
            for (int i = 0; i < count; i++)
            {
                T item = func(reader);
                result[i] = item;
            }

            return result;
        }

        /// <summary>
        /// Считывание массива из файла.
        /// </summary>
        [CanBeNull]
        [ItemNotNull]
        public static T[] ReadFromFile<T>
            (
                [NotNull] string fileName,
                [NotNull] Func<BinaryReader, T> func
            )
        {
            using (Stream stream = File.OpenRead(fileName))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                return reader.ReadArray(func);
            }
        }

        /// <summary>
        /// Считывание массива из файла.
        /// </summary>
        [CanBeNull]
        [ItemNotNull]
        public static T[] ReadFromZipFile<T>
            (
                [NotNull] string fileName,
                [NotNull] Func<BinaryReader, T> func
            )
        {
            using (Stream stream = File.OpenRead(fileName))
            using (DeflateStream deflate = new DeflateStream
                (
                    stream,
                    CompressionMode.Decompress
                ))
            using (BinaryReader reader = new BinaryReader(deflate))
            {
                return reader.ReadArray(func);
            }
        }

        /// <summary>
        /// Считывание массива из памяти.
        /// </summary>
        public static T[] ReadFromMemory<T>
            (
                [NotNull] this byte[] array,
                [NotNull] Func<BinaryReader,T> func
            )
        {
            using (Stream stream = new MemoryStream(array))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                return reader.ReadArray(func);
            }
        }

        /// <summary>
        /// Считывание массива из памяти.
        /// </summary>
        public static T[] ReadFromZipMemory<T>
            (
                [NotNull] this byte[] array,
                [NotNull] Func<BinaryReader, T> func
            )
        {
            using (Stream stream = new MemoryStream(array))
            using (DeflateStream deflate = new DeflateStream
                (
                    stream,
                    CompressionMode.Decompress
                ))
            using (BinaryReader reader = new BinaryReader(deflate))
            {
                return reader.ReadArray(func);
            }
        }


        #endregion
    }
}
