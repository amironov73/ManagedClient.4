/* IrbisIOUtils.cs
 */

#region Using directives

using System;
using System.IO;
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

        #endregion
    }
}
