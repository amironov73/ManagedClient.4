/* Utilities.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

using CM=System.Configuration.ConfigurationManager;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Несколько утилит, упрощающих код.
    /// </summary>
    [PublicAPI]
    public static class Utilities
    {
        /// <summary>
        /// Выборка элемента из массива.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="occurrence"></param>
        /// <returns></returns>
        public static T GetOccurrence<T>
            (
                this T[] array,
                int occurrence
            )
        {
            occurrence = (occurrence >= 0)
                            ? occurrence
                            : array.Length + occurrence;
            T result = default(T);
            if ((occurrence >= 0) && (occurrence < array.Length))
            {
                result = array[occurrence];
            }
            return result;
        }

        /// <summary>
        /// Выборка элемента из списка.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="occurrence"></param>
        /// <returns></returns>
        public static T GetOccurrence<T>
            (
                this IList<T> list,
                int occurrence
            )
        {
            occurrence = (occurrence >= 0)
                            ? occurrence
                            : list.Count + occurrence;
            T result = default(T);
            if ((occurrence >= 0) && (occurrence < list.Count))
            {
                result = list[occurrence];
            }
            return result;
        }

        /// <summary>
        /// Отбирает из последовательности только
        /// ненулевые элементы.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static IEnumerable<T> NonNullItems<T>
            (
                this IEnumerable<T> sequence
            )
            where T: class
        {
            return sequence.Where(value => value != null);
        }

        /// <summary>
        /// Отбирает из последовательности только непустые строки.
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static IEnumerable<string> NonEmptyLines
            (
                this IEnumerable<string> sequence
            )
        {
            return sequence.Where(line => !string.IsNullOrEmpty(line));
        }

        /// <summary>
        /// Разбивает строку по указанному разделителю.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string[] SplitFirst
            (
                this string line,
                char delimiter
            )
        {
            int index = line.IndexOf(delimiter);
            string[] result = (index < 0)
                                  ? new[] { line }
                                  : new[] { line.Substring(0, index),
                                      line.Substring(index + 1) };
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
                this string one,
                string two
            )
        {
            return string.Compare(one, two, StringComparison.OrdinalIgnoreCase) == 0;
        }

        /// <summary>
        /// Сравнивает строки.
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        public static bool SameStringSensitive
            (
                this string one,
                string two
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
                IEnumerable<string> many
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
                IEnumerable<char> many
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
#if PocketPC
            return (char.ToUpper(one) == char.ToUpper(two));
#else
            return (char.ToUpperInvariant(one) == char.ToUpperInvariant(two));
#endif
        }

        /// <summary>
        /// Представляет ли строка положительное целое число.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsPositiveInteger
            (
                this string text
            )
        {
            return (text.SafeParseInt32(0) > 0);
        }

        /// <summary>
        /// Безопасное преобразование строки
        /// в целое.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="defaultValue"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static int SafeToInt32
            (
                string text,
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
            if ((result < minValue)
                || (result > maxValue))
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
        /// <returns>Разобранное целое число или значение по умолчанию.</returns>
        public static int SafeParseInt32
            (
                this string text,
                int defaultValue
            )
        {
            int result = defaultValue;

            try
            {
                result = int.Parse(text);
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch
            // ReSharper restore EmptyGeneralCatchClause
            {
                // Do nothing
            }

            //if (!Int32.TryParse(text, out result))
            //{
            //    result = defaultValue;
            //}
            return result;
        }

        /// <summary>
        /// Безопасный парсинг целого числа.
        /// </summary>
        /// <param name="text">Строка, подлежащая парсингу.</param>
        /// <returns>Разобранное целое число или значение по умолчанию.</returns>
        public static int SafeParseInt32
            (
                this string text
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
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static int SafeCompare
            (
                this string s1,
                string s2
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
        /// <param name="value"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool SafeCompare
            (
                string value,
                params string[] list
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            foreach (string s in list)
            {
                if (String.Equals
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
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static bool SafeContains
            (
                string s1,
                string s2
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
        /// <param name="value"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool SafeContains
            (
                string value,
                params string[] list
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            foreach (string s in list)
            {
                if (value.ToUpper().Contains(s.ToUpper()))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Поиск начала строки.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="begin"></param>
        /// <returns></returns>
        public static bool SafeStarts
            (
                string text,
                string begin
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
            if (first == (last - 1))
            {
                return (first.ToInvariantString() + ", " + last.ToInvariantString());
            }
            return (first.ToInvariantString() + "-" + last.ToInvariantString());
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
        public static string CompressRange
            (
                IEnumerable<int> n
            )
        {
            if (n == null)
            {
                return string.Empty;
            }

            // ReSharper disable PossibleMultipleEnumeration
            if (!n.Any())
            {
                return String.Empty;
            }

            var result = new StringBuilder();
            var first = true;
            var prev = n.First();
            var last = prev;
            foreach (var i in n.Skip(1))
            {
                if (i != (last + 1))
                {
                    result.AppendFormat("{0}{1}", (first ? "" : ", "),
                        FormatRange(prev, last));
                    prev = i;
                    first = false;
                }
                last = i;
            }
            result.AppendFormat("{0}{1}", (first ? "" : ", "),
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
        public static string[] SplitLines
            (
                this string text
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
        public static string MergeLines
            (
                this IEnumerable<string> lines
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
        public static byte[] ReadToEnd
            (
                this Stream stream
            )
        {
            MemoryStream result = new MemoryStream();

            while (true)
            {
                byte[] buffer = new byte[10 * 1024];
                int read = stream.Read(buffer, 0, buffer.Length);
                if (read <= 0)
                {
                    break;
                }
                result.Write(buffer, 0, read);
            }

            return result.ToArray();
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
                TextWriter writer,
                byte[] buffer,
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
        /// Добавление элемента к массиву.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceArray"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static T[] AppendToArray<T>
            (
                this T[] sourceArray,
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
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceArray"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static T[] AppendToArray<T>
            (
                this T[] sourceArray,
                IEnumerable<T> items
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
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceArray"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static T[] AppendToArray<T>
            (
                this T[] sourceArray,
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
        /// <param name="candidates"></param>
        /// <returns></returns>
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
        /// <param name="text"></param>
        /// <param name="symbols"></param>
        /// <returns></returns>
        public static bool ContainsAnySymbol
            (
                this string text,
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
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool ContainsWhitespace
            (
                this string text
            )
        {
            return text.ContainsAnySymbol
                (
                    ' ', '\t', '\r', '\n'
                );
        }

        /// <summary>
        /// Подготавливает строку запроса
        /// </summary>
        /// <param name="text"></param>
        /// <remarks>Строка формата не должна
        /// содержать комментариев и переводов
        /// строки (настоящих и ирбисных)
        /// </remarks>
        /// <returns></returns>
        [CanBeNull]
        public static string PrepareFormat
            (
                [CanBeNull] this string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            text = Regex.Replace
                (
                    text,
                    "/[*].*?[\r\n]",
                    " "
                )
                .Replace ( '\r', ' ' )
                .Replace ( '\n', ' ' )
                .Replace ( '\t', ' ' )
                .Replace ( '\x1F', ' ' )
                .Replace ( '\x1E', ' ' );

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
    }
}
