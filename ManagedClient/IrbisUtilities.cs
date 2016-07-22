/* IrbisUtilities.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient
{
    public static class IrbisUtilities
    {
        #region Public methods

        public static DateTime ParseIrbisDate
            (
                this string text
            )
        {
#if PocketPC

            return DateTime.ParseExact
                (
                    text,
                    "yyyyMMdd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.NoCurrentDateDefault
                );

#else

            DateTime result;

            DateTime.TryParseExact
                (
                    text,
                    "yyyyMMdd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out result
                );
            return result;
#endif
        }

        public static IEnumerable<T[]> Slice<T>
            (
                this IEnumerable<T> sequence,
                int pieceSize
            )
        {
            if (pieceSize <= 0)
            {
                throw new ArgumentOutOfRangeException("pieceSize");
            }
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            List<T> piece = new List<T>(pieceSize);
            foreach (T item in sequence)
            {
                piece.Add(item);
                if (piece.Count >= pieceSize)
                {
                    yield return piece.ToArray();
                    piece = new List<T>(pieceSize);
                }
            }

            if (piece.Count != 0)
            {
                yield return piece.ToArray();
            }
        }

        public static string EncodePercentString
            (
                byte[] array
            )
        {
            if (ReferenceEquals(array, null)
                || (array.Length == 0))
            {
                return string.Empty;
            }

            StringBuilder result = new StringBuilder();

            foreach (byte b in array)
            {
                if (((b >= 'A') && (b <= 'Z'))
                    || ((b >= 'a') && (b <= 'z'))
                    || ((b >= '0') && (b <= '9'))
                    )
                {
                    result.Append((char) b);
                }
                else
                {
                    result.AppendFormat
                        (
                            "%{0:XX}",
                            b
                        );
                }
            }

            return result.ToString();
        }

        public static byte[] DecodePercentString
            (
                string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return new byte[0];
            }

            MemoryStream stream = new MemoryStream();

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (c != '%')
                {
                    stream.WriteByte((byte) c);
                }
                else
                {
                    if (i >= (text.Length - 2))
                    {
                        throw new FormatException("text");
                    }
                    byte b = byte.Parse
                        (
                            text.Substring(i+1,2),
                            NumberStyles.HexNumber
                        );
                    stream.WriteByte(b);
                    i += 2;
                }
            }

            return stream.ToArray();
        }

        public static string GatherSubfields
            (
                this IrbisRecord record,
                string tag,
                string separator,
                params char[] codes
            )
        {
            RecordField field = record
                .Fields
                .GetField(tag)
                .FirstOrDefault();
            if (ReferenceEquals(field, null))
            {
                return null;
            }
            List<string> list = new List<string>();

            foreach (char code in codes)
            {
                string[] text = field
                    .GetSubField(code)
                    .GetSubFieldText();
                list.AddRange(text);
            }

            if (list.Count == 0)
            {
                return null;
            }

            return string.Join
                (
                    separator,
                    list.ToArray()
                );
        }

        /// <summary>
        /// Стандартные наименования для ключа строки подключения
        /// к серверу ИРБИС64.
        /// </summary>
        /// <returns></returns>
        public static string[] StandardConnectionStrings()
        {
            return new[]
            {
                "irbis-connection",
                "irbis-connection-string",
                "irbis64-connection",
                "irbis64",
                "connection-string"
            };
        }

        /// <summary>
        /// Получаем строку подключения в app.settings.
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionString()
        {
            return Utilities.FindSetting
                (
                    StandardConnectionStrings()
                );
        }

        /// <summary>
        /// Получаем уже подключенного клиента.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ApplicationException">
        /// Если строка подключения в app.settings не найдена.
        /// </exception>
        public static ManagedClient64 GetClient()
        {
            string connectionString = GetConnectionString();
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ApplicationException("Connection string not specified!");
            }

            ManagedClient64 result = new ManagedClient64();
            result.ParseConnectionString(connectionString);
            result.Connect();
            
            return result;
        }

        #endregion
    }
}
