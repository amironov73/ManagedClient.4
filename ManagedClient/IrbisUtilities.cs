// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisUtilities.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// General purpose utility routines.
    /// </summary>
    public static class IrbisUtilities
    {
        #region Public methods

        /// <summary>
        /// Parse the date in IRBIS format.
        /// </summary>
        public static DateTime ParseIrbisDate
            (
                [CanBeNull] this string text
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

        /// <summary>
        /// Slice the sequence into pieces with given size.
        /// </summary>
        [NotNull]
        public static IEnumerable<T[]> Slice<T>
            (
                [NotNull] this IEnumerable<T> sequence,
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

        /// <summary>
        /// Encode byte array into %31%32%33.
        /// </summary>
        [NotNull]
        public static string EncodePercentString
            (
                [CanBeNull] byte[] array
            )
        {
            if (ReferenceEquals(array, null)
                || array.Length == 0)
            {
                return string.Empty;
            }

            StringBuilder result = new StringBuilder();

            foreach (byte b in array)
            {
                if (b >= 'A' && b <= 'Z'
                    || b >= 'a' && b <= 'z'
                    || b >= '0' && b <= '9'
                    )
                {
                    result.Append((char) b);
                }
                else
                {
                    result.AppendFormat
                        (
                            "%{0:X2}",
                            b
                        );
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Decode %31%32%33.
        /// </summary>
        [NotNull]
        public static byte[] DecodePercentString
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return new byte[0];
            }

            using (MemoryStream stream = new MemoryStream())
            {
                for (int i = 0; i < text.Length; i++)
                {
                    char c = text[i];
                    if (c != '%')
                    {
                        stream.WriteByte((byte) c);
                    }
                    else
                    {
                        if (i >= text.Length - 2)
                        {
                            throw new FormatException("text");
                        }
                        byte b = byte.Parse
                        (
                            text.Substring(i + 1, 2),
                            NumberStyles.HexNumber
                        );
                        stream.WriteByte(b);
                        i += 2;
                    }
                }

                return stream.ToArray();
            }
        }

        /// <summary>
        /// Gather subfields with given codes.
        /// </summary>
        [CanBeNull]
        public static string GatherSubfields
            (
                [NotNull] this IrbisRecord record,
                [NotNull] string tag,
                [NotNull] string separator,
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
        [NotNull]
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
        [NotNull]
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
        /// <exception cref="ApplicationException">
        /// Если строка подключения в app.settings не найдена.
        /// </exception>
        [NotNull]
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
