// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Utilities.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

using JetBrains.Annotations;

using CM = System.Configuration.ConfigurationManager;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Несколько утилит, упрощающих код.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Выборка элемента из массива.
        /// </summary>
        public static T GetOccurrence<T>
            (
                [NotNull] this T[] array,
                int occurrence
            )
        {
            occurrence = occurrence >= 0
                            ? occurrence
                            : array.Length + occurrence;
            T result = default(T);
            if (occurrence >= 0 && occurrence < array.Length)
            {
                result = array[occurrence];
            }

            return result;
        }

        /// <summary>
        /// Выборка элемента из списка.
        /// </summary>
        public static T GetOccurrence<T>
            (
                [NotNull] this IList<T> list,
                int occurrence
            )
        {
            occurrence = occurrence >= 0
                            ? occurrence
                            : list.Count + occurrence;
            T result = default(T);
            if (occurrence >= 0 && occurrence < list.Count)
            {
                result = list[occurrence];
            }

            return result;
        }

        /// <summary>
        /// Отбирает из последовательности только
        /// ненулевые элементы.
        /// </summary>
        [NotNull]
        public static IEnumerable<T> NonNullItems<T>
            (
                [NotNull] this IEnumerable<T> sequence
            )
            where T : class
        {
            return sequence.Where(value => value != null);
        }

        /// <summary>
        /// Отбирает из последовательности только непустые строки.
        /// </summary>
        [NotNull]
        public static IEnumerable<string> NonEmptyLines
            (
                [NotNull] this IEnumerable<string> sequence
            )
        {
            return sequence.Where(line => !string.IsNullOrEmpty(line));
        }

        /// <summary>
        /// Разбивает строку по указанному разделителю.
        /// </summary>
        [NotNull]
        public static string[] SplitFirst
            (
                [NotNull] this string line,
                char delimiter
            )
        {
            int index = line.IndexOf(delimiter);
            string[] result = index < 0
                ? new[] { line }
                : new[] {
                    line.Substring(0, index),
                    line.Substring(index + 1)
                };

            return result;
        }

        /// <summary>
        /// Сравнивает строки с точностью до регистра.
        /// </summary>
        /// <param name="one">Первая строка.</param>
        /// <param name="two">Вторая строка.</param>
        /// <returns>Строки совпадают с точностью до регистра.</returns>
        public static bool SameString
            (
                [CanBeNull] this string one,
                [CanBeNull] string two
            )
        {
            return string.Compare(one, two, StringComparison.OrdinalIgnoreCase) == 0;
        }

        /// <summary>
        /// Сравнивает строки.
        /// </summary>
        public static bool SameStringSensitive
            (
                [CanBeNull] this string one,
                [CanBeNull] string two
            )
        {
            return string.Compare(one, two, StringComparison.Ordinal) == 0;
        }

        /// <summary>
        /// Проверяет, является ли искомая строка одной
        /// из перечисленных. Регистр символов не учитывается.
        /// </summary>
        /// <param name="one">Искомая строка.</param>
        /// <param name="many">Источник проверяемых строк.</param>
        /// <returns>Найдена ли искомая строка.</returns>
        public static bool OneOf
            (
                this string one,
                [NotNull] IEnumerable<string> many
            )
        {
            return many
                .Any(_ => _.SameString(one));
        }

        /// <summary>
        /// Проверяет, является ли искомая строка одной
        /// из перечисленных. Регистр символов не учитывается.
        /// </summary>
        /// <param name="one">Искомая строка.</param>
        /// <param name="many">Массив проверяемых строк.</param>
        /// <returns>Найдена ли искомая строка.</returns>
        public static bool OneOf
            (
               this string one,
               params string[] many
            )
        {
            return one.OneOf(many.AsEnumerable());
        }

        /// <summary>
        /// Проверяет, является ли искомый символ одним
        /// из перечисленных. Регистр символов не учитывается.
        /// </summary>
        /// <param name="one">Искомый символ.</param>
        /// <param name="many">Массив проверяемых символов.</param>
        /// <returns>Найден ли искомый символ.</returns>
        public static bool OneOf
            (
                this char one,
                [NotNull] IEnumerable<char> many
            )
        {
            return many
                .Any(_ => _.SameChar(one));
        }

        /// <summary>
        /// Проверяет, является ли искомый символ одним
        /// из перечисленных. Регистр символов не учитывается.
        /// </summary>
        /// <param name="one">Искомый символ.</param>
        /// <param name="many">Массив проверяемых символов.</param>
        /// <returns>Найден ли искомый символ.</returns>
        public static bool OneOf
            (
                this char one,
                params char[] many
            )
        {
            return one.OneOf(many.AsEnumerable());
        }

        /// <summary>
        /// Сравнивает символы с точностью до регистра.
        /// </summary>
        /// <param name="one">Первый символ.</param>
        /// <param name="two">Второй символ.</param>
        /// <returns>Символы совпадают с точностью до регистра.</returns>
        public static bool SameChar
            (
                this char one,
                char two
            )
        {
            return char.ToUpperInvariant(one) == char.ToUpperInvariant(two);
        }

        /// <summary>
        /// Представляет ли строка положительное целое число.
        /// </summary>
        public static bool IsPositiveInteger
            (
                [CanBeNull] this string text
            )
        {
            return text.SafeParseInt32(0) > 0;
        }

        /// <summary>
        /// Безопасное преобразование строки
        /// в целое.
        /// </summary>
        public static int SafeToInt32
            (
                [CanBeNull] string text,
                int defaultValue,
                int minValue,
                int maxValue
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return defaultValue;
            }

            int result;
            if (!int.TryParse(text, out result))
            {
                result = defaultValue;
            }
            if (result < minValue
                || result > maxValue)
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Безопасный парсинг целого числа.
        /// </summary>
        /// <param name="text">Строка, подлежащая парсингу.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <returns>Разобранное целое число или значение по умолчанию.
        /// </returns>
        public static int SafeParseInt32
            (
                [CanBeNull] this string text,
                int defaultValue
            )
        {
            int result = defaultValue;

            try
            {
                // ReSharper disable AssignNullToNotNullAttribute
                result = int.Parse(text);
                // ReSharper restore AssignNullToNotNullAttribute
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch
            // ReSharper restore EmptyGeneralCatchClause
            {
                // Do nothing
            }

            return result;
        }

        /// <summary>
        /// Безопасный парсинг целого числа.
        /// </summary>
        /// <param name="text">Строка, подлежащая парсингу.</param>
        /// <returns>Разобранное целое число или значение по умолчанию.
        /// </returns>
        public static int SafeParseInt32
            (
                [CanBeNull] this string text
            )
        {
            return SafeParseInt32
                (
                    text,
                    0
                 );
        }

        /// <summary>
        /// Сравнение строк.
        /// </summary>
        public static int SafeCompare
            (
                [CanBeNull] this string s1,
                [CanBeNull] string s2
            )
        {
            return string.Compare
                (
                    s1,
                    s2,
                    StringComparison.InvariantCultureIgnoreCase
                );
        }

        /// <summary>
        /// Сравнение строки с массивом.
        /// </summary>
        public static bool SafeCompare
            (
                [CanBeNull] string value,
                params string[] list
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            foreach (string s in list)
            {
                if (string.Equals
                    (
                        value,
                        s,
                        StringComparison.CurrentCultureIgnoreCase
                    ))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Поиск подстроки.
        /// </summary>
        public static bool SafeContains
            (
                [CanBeNull] string s1,
                [CanBeNull] string s2
            )
        {
            if (string.IsNullOrEmpty(s1)
                || string.IsNullOrEmpty(s2))
            {
                return false;
            }

            return s1
                .ToLowerInvariant()
                .Contains
                (
                    s2.ToLowerInvariant()
                );
        }

        /// <summary>
        /// Поиск подстроки.
        /// </summary>
        public static bool SafeContains
            (
                [CanBeNull] string value,
                params string[] list
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            value = value.ToUpperInvariant();
            foreach (string s in list)
            {
                if (value.Contains(s.ToUpperInvariant()))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Поиск начала строки.
        /// </summary>
        public static bool SafeStarts
            (
                [CanBeNull] string text,
                [CanBeNull] string begin
            )
        {
            if (string.IsNullOrEmpty(text)
                || string.IsNullOrEmpty(begin))
            {
                return false;
            }

            return text.ToLowerInvariant()
                .StartsWith(begin.ToLowerInvariant());
        }

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной 
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        [NotNull]
        public static string ToInvariantString
            (
                this int value
            )
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [NotNull]
        public static string ToInvariantString
            (
                this char value
            )
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Форматирование диапазона целых чисел.
        /// </summary>
        /// <remarks>Границы диапазона могут совпадать, однако
        /// левая не должна превышать правую.</remarks>
        /// <param name="first">Левая граница диапазона.</param>
        /// <param name="last">Правая граница диапазона.</param>
        /// <returns>Строковое представление диапазона.</returns>
        [NotNull]
        public static string FormatRange
            (
                int first,
                int last
            )
        {
            if (first == last)
            {
                return first.ToInvariantString();
            }

            if (first == last - 1)
            {
                return first.ToInvariantString() + ", " + last.ToInvariantString();
            }

            return first.ToInvariantString() + "-" + last.ToInvariantString();
        }

        /// <summary>
        /// Преобразование набора целых чисел в строковое представление,
        /// учитывающее возможное наличие цепочек последовательных чисел,
        /// которые форматируются как диапазоны.
        /// </summary>
        /// <param name="n">Источник целых чисел.</param>
        /// <remarks>Источник должен поддерживать многократное считывание.
        /// Числа предполагаются предварительно упорядоченные. Повторения чисел
        /// не допускаются. Пропуски в последовательностях допустимы.
        /// Числа допускаются только неотрицательные.
        /// </remarks>
        /// <returns>Строковое представление набора чисел.</returns>
        [NotNull]
        public static string CompressRange
            (
                [CanBeNull] IEnumerable<int> n
            )
        {
            if (n == null)
            {
                return string.Empty;
            }

            // ReSharper disable PossibleMultipleEnumeration
            if (!n.Any())
            {
                return string.Empty;
            }

            var result = new StringBuilder();
            var first = true;
            var prev = n.First();
            var last = prev;
            foreach (var i in n.Skip(1))
            {
                if (i != last + 1)
                {
                    result.AppendFormat("{0}{1}", first ? "" : ", ",
                        FormatRange(prev, last));
                    prev = i;
                    first = false;
                }
                last = i;
            }
            result.AppendFormat("{0}{1}", first ? "" : ", ",
                FormatRange(prev, last));

            return result.ToString();
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Разбивка текста на отдельные строки.
        /// </summary>
        /// <remarks>Пустые строки не удаляются.</remarks>
        /// <param name="text">Текст для разбиения.</param>
        /// <returns>Массив строк.</returns>
        [NotNull]
        public static string[] SplitLines
            (
                [NotNull] this string text
            )
        {
            text = text.Replace("\r\n", "\r");

            return text.Split
                (
                    '\r'
                );
        }

        /// <summary>
        /// Склейка строк в сплошной текст, разделенный переводами строки.
        /// </summary>
        /// <param name="lines">Строки для склейки.</param>
        /// <returns>Склеенный текст.</returns>
        [NotNull]
        public static string MergeLines
            (
                [NotNull] this IEnumerable<string> lines
            )
        {
            string result = string.Join
                (
                    Environment.NewLine,
                    lines.ToArray()
                );

            return result;
        }

        /// <summary>
        /// Считывает из потока максимально возможное число байт.
        /// </summary>
        /// <remarks>Полезно для считывания из сети (сервер высылает
        /// ответ, после чего закрывает соединение).</remarks>
        /// <param name="stream">Поток для чтения.</param>
        /// <returns>Массив считанных байт.</returns>
        [NotNull]
        public static byte[] ReadToEnd
            (
                [NotNull] this Stream stream
            )
        {
            using (MemoryStream result = new MemoryStream())
            {
                while (true)
                {
                    byte[] buffer = new byte[32 * 1024];
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                    {
                        break;
                    }
                    result.Write(buffer, 0, read);
                }

                return result.ToArray();
            }
        }

        /// <summary>
        /// Считывает из сокета максимально возможное число байт.
        /// </summary>
        [NotNull]
        public static byte[] ReadToEnd
            (
                [NotNull] this Socket socket
            )
        {
            using (MemoryStream result = new MemoryStream())
            {
                while (true)
                {
                    byte[] buffer = new byte[32 * 1024];
                    int read = socket.Receive(buffer);
                    if (read <= 0)
                    {
                        break;
                    }
                    result.Write(buffer, 0, read);
                }

                return result.ToArray();
            }
        }

        /// <summary>
        /// Шестнадцатиричный дамп массива байт.
        /// </summary>
        /// <param name="writer">Куда писать.</param>
        /// <param name="buffer">Байты.</param>
        /// <param name="offset">Начальное смещение.</param>
        /// <param name="count">Количество байт для дампа.</param>
        public static void DumpBytes
            (
                [NotNull] TextWriter writer,
                [NotNull] byte[] buffer,
                int offset,
                int count
            )
        {
            for (int i = 0; i < count; i++)
            {
                if (i != 0)
                {
                    writer.Write(" ");
                }
                writer.Write("{0:X2}", buffer[offset + i]);
            }
            writer.WriteLine();
        }

        /// <summary>
        /// Шестнадцатиричный дамп массива байт.
        /// </summary>
        [NotNull]
        public static string DumpBytes
            (
                [NotNull] byte[] buffer
            )
        {
            StringBuilder result = new StringBuilder(buffer.Length * 5);

            int offset;
            for (offset = 0; offset < buffer.Length; offset += 16)
            {
                result.AppendFormat
                    (
                        "{0:X6}:",
                        offset
                    );

                int run = Math.Min(buffer.Length - offset, 16);

                for (int i = 0; i < run; i++)
                {
                    result.AppendFormat
                        (
                            " {0:X2}",
                            buffer[offset + i]
                        );
                }

                result.AppendLine();
            }

            return result.ToString();
        }

        /// <summary>
        /// Добавление элемента к массиву.
        /// </summary>
        [NotNull]
        public static T[] AppendToArray<T>
            (
                [NotNull] this T[] sourceArray,
                T item
            )
        {
            List<T> result = new List<T>(sourceArray.Length);
            result.AddRange(sourceArray);
            result.Add(item);

            return result.ToArray();
        }

        /// <summary>
        /// Добавление элементов к массиву.
        /// </summary>
        [NotNull]
        public static T[] AppendToArray<T>
            (
                [NotNull] this T[] sourceArray,
                [NotNull] IEnumerable<T> items
            )
        {
            List<T> result = new List<T>(sourceArray.Length);
            result.AddRange(sourceArray);
            result.AddRange(items);

            return result.ToArray();
        }

        /// <summary>
        /// Добавление элементов к массиву.
        /// </summary>
        [NotNull]
        public static T[] AppendToArray<T>
            (
                [NotNull] this T[] sourceArray,
                params T[] items
            )
        {
            List<T> result = new List<T>(sourceArray.Length);
            result.AddRange(sourceArray);
            result.AddRange(items);

            return result.ToArray();
        }

        /// <summary>
        /// Получаем сеттинг из возможных кандидатов.
        /// </summary>
        [CanBeNull]
        public static string FindSetting
            (
                params string[] candidates
            )
        {
            foreach (string candidate in candidates.NonEmptyLines())
            {
                string setting = CM.AppSettings[candidate];
                if (!string.IsNullOrEmpty(setting))
                {
                    return setting;
                }
            }

            return null;
        }

        /// <summary>
        /// Содержит ли строка любой из перечисленных символов.
        /// </summary>
        public static bool ContainsAnySymbol
            (
                [NotNull] this string text,
                params char[] symbols
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                foreach (char c in text)
                {
                    if (symbols.Contains(c))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Строка содержит пробельные символы?
        /// </summary>
        public static bool ContainsWhitespace
            (
                [NotNull] this string text
            )
        {
            return text.ContainsAnySymbol
                (
                    ' ', '\t', '\r', '\n'
                );
        }

        /// <summary>
        /// Remove comments from the format.
        /// </summary>
        [CanBeNull]
        public static string RemoveComments
            (
                [CanBeNull] string text
            )
        {
            const char ZERO = '\0';

            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            if (!text.Contains("/*"))
            {
                return text;
            }

            StringBuilder result = new StringBuilder(text.Length);
            TextNavigator navigator = new TextNavigator(text);
            char state = ZERO;

            while (!navigator.IsEOF)
            {
                char c = navigator.ReadChar();

                switch (state)
                {
                    case '\'':
                        if (c == '\'')
                        {
                            state = ZERO;
                        }
                        result.Append(c);
                        break;

                    case '"':
                        if (c == '"')
                        {
                            state = ZERO;
                        }
                        result.Append(c);
                        break;

                    case '|':
                        if (c == '|')
                        {
                            state = ZERO;
                        }
                        result.Append(c);
                        break;

                    default:
                        if (c == '/')
                        {
                            if (navigator.PeekChar() == '*')
                            {
                                navigator.ReadTo('\r', '\n');
                            }
                            else
                            {
                                result.Append(c);
                            }
                        }
                        else if (c == '\'' || c == '"' || c == '|')
                        {
                            state = c;
                            result.Append(c);
                        }
                        else
                        {
                            result.Append(c);
                        }
                        break;
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Подготавливает строку запроса.
        /// </summary>
        /// <remarks>Строка формата не должна
        /// содержать комментариев и переводов
        /// строки (настоящих и ирбисных).
        /// </remarks>
        public static string PrepareFormat
            (
                [CanBeNull] this string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            text = RemoveComments(text);
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Replace("\r", string.Empty)
                    .Replace("\n", string.Empty)
                    .Replace('\t', ' ')
                    .Replace("\x1F", string.Empty)
                    .Replace("\x1E", string.Empty);
            }

            return text;
        }

        /// <summary>
        /// Подготавливает строку запроса,
        /// заменяя запрещённые символы на пробелы.
        /// </summary>
        [CanBeNull]
        public static string PrepareQuery
            (
                [CanBeNull] this string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            return text
                .Replace('\r', ' ')
                .Replace('\n', ' ')
                .Replace('\t', ' ');
        }

        /// <summary>
        /// Превращает строку в видимую.
        /// Пример: "(null)".
        /// </summary>
        [NotNull]
        public static string MakeVisibleString
            (
                [CanBeNull] this string text
            )
        {
            if (ReferenceEquals(text, null))
            {
                return "(null)";
            }
            if (string.IsNullOrEmpty(text))
            {
                return "(empty)";
            }
            if (string.IsNullOrWhiteSpace(text))
            {
                return "(whitespace)";
            }

            return text;
        }

        /// <summary>
        /// Разбиение массива на (почти) равные части.
        /// </summary>
        [NotNull]
        public static T[][] SplitArray<T>
            (
                [NotNull] T[] array,
                int partCount
            )
        {
            List<T[]> result = new List<T[]>(partCount);
            int length = array.Length;
            int chunkSize = length / partCount;
            while (chunkSize * partCount < length)
            {
                chunkSize++;
            }
            int offset = 0;
            for (int i = 0; i < partCount; i++)
            {
                int size = Math.Min(chunkSize, length - offset);
                T[] chunk = new T[size];
                Array.Copy(array, offset, chunk, 0, size);
                result.Add(chunk);
                offset += size;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Применяет действие к каждому элементу последовательности
        /// </summary>
        /// <returns>Ту же самую последовательность.</returns>
        [NotNull]
        public static IEnumerable<T> Tee<T>
            (
                 [NotNull] this IEnumerable<T> list,
                 [NotNull] Action<T> action
            )
        {
            foreach (T item in list)
            {
                action(item);
                yield return item;
            }
        }

        /// <summary>
        /// Replace control characters in the text.
        /// </summary>
        [CanBeNull]
        public static string ReplaceControlCharacters
            (
                [CanBeNull] string text,
                char substitute
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            bool needReplace = false;
            foreach (char c in text)
            {
                if (c < ' ')
                {
                    needReplace = true;
                    break;
                }
            }

            if (!needReplace)
            {
                return text;
            }

            StringBuilder result = new StringBuilder(text.Length);

            foreach (char c in text)
            {
                result.Append
                    (
                        c < ' ' ? substitute : c
                    );
            }

            return result.ToString();
        }

        /// <summary>
        /// Replace control characters in the text.
        /// </summary>
        [CanBeNull]
        public static string ReplaceControlCharacters
            (
                [CanBeNull] string text
            )
        {
            return ReplaceControlCharacters(text, ' ');
        }


        /// <summary>
        /// Throw <see cref="ArgumentNullException"/>
        /// if given value is <c>null</c>.
        /// </summary>
        [NotNull]
        public static T ThrowIfNull<T>
            (
                [CanBeNull] this T value
            )
            where T : class
        {
            if (ReferenceEquals(value, null))
            {
                throw new ArgumentException("value");
            }

            return value;
        }

        /// <summary>
        /// Throw <see cref="ArgumentNullException"/>
        /// if given value is <c>null</c>.
        /// </summary>
        [NotNull]
        public static T1 ThrowIfNull<T1, T2>
            (
                [CanBeNull] this T1 value
            )
            where T1 : class
            where T2 : Exception, new()
        {
            if (ReferenceEquals(value, null))
            {
                throw new T2();
            }

            return value;
        }

        /// <summary>
        /// Throw <see cref="ArgumentNullException"/>
        /// if given value is <c>null</c>.
        /// </summary>
        [NotNull]
        public static T ThrowIfNull<T>
            (
                [CanBeNull] this T value,
                [NotNull] string message
            )
            where T : class
        {
            if (ReferenceEquals(value, null))
            {
                throw new ArgumentException(message);
            }

            return value;
        }

        /// <summary>
        /// Преобразование любого значения в строку.
        /// </summary>
        /// <returns>Для <c>null</c> возвращается "(null)".
        /// </returns>
        [NotNull]
        public static string ToVisibleString<T>
            (
                [CanBeNull] this T value
            )
        {
            if (ReferenceEquals(value, null))
            {
                return "(null)";
            }

            string result = value.ToString();

            return result.ToVisibleString();
        }

        /// <summary>
        /// Determines whether given object
        /// is default value.
        /// </summary>
        public static bool NotDefault<T>
            (
                this T obj
            )
        {
            return !EqualityComparer<T>.Default.Equals
                (
                    obj,
                    default(T)
                );
        }

        /// <summary>
        /// Returns given value instead of
        /// default(T) if happens.
        /// </summary>
        public static T NotDefault<T>
            (
                this T obj,
                T value
            )
        {
            return EqualityComparer<T>.Default.Equals
                (
                    obj,
                    default(T)
                )
                ? value
                : obj;
        }

        /// <summary>
        /// Преобразование любого значения в строку.
        /// </summary>
        /// <returns>Для <c>null</c> возвращается <c>null</c>.
        /// </returns>
        [CanBeNull]
        public static string NullableToString<T>
            (
                [CanBeNull] this T value
            )
            where T : class
        {
            return ReferenceEquals(value, null)
                ? null
                : value.ToString();
        }

        /// <summary>
        /// Преобразование любого значения в строку.
        /// </summary>
        /// <returns>Для <c>null</c> возвращается "(null)".
        /// </returns>
        [NotNull]
        public static string NullableToVisibleString<T>
            (
                [CanBeNull] this T value
            )
            where T : class
        {
            string text = value.NullableToString();

            return text.ToVisibleString();
        }
    }
}
