/* Unifor.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Pft
{
    public sealed class Unifor
    {
        #region Properties

        public PftContext Context { get; set; }

        public PftGroupStatement Group { get; set; }

        #endregion

        #region Construction

        public Unifor()
        {
        }

        public Unifor
            (
                PftContext context
            )
        {
            Context = context;
        }

        public Unifor
            (
                PftContext context, 
                PftGroupStatement grp
            )
        {
            Context = context;
            Group = grp;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public string DecodeViaDictionary
            (
                string format
            )
        {
            if (string.IsNullOrEmpty(format))
            {
                return string.Empty;
            }

            char delimiter = format.Contains('!')
                ? '!'
                : '\\';

            bool caseSensitive = delimiter == '\\';

            string[] parts = format.Split(delimiter);
            if (parts.Length != 2)
            {
                return string.Empty;
            }

            IrbisMenu menu = IrbisMenu.Read
                (
                    Context.Client,
                    parts[0]
                );
            string result = caseSensitive
                ? menu.GetStringSensitive(parts[1], string.Empty)
                : menu.GetString(parts[1], string.Empty);

            return result;
        }

        public string DecodeViaIniFile
            (
                string format
            )
        {
            if (string.IsNullOrEmpty(format))
            {
                return string.Empty;
            }

            string[] parts = format.Split(',');
            if (parts.Length < 2)
            {
                return string.Empty;
            }
            string section = parts[0];
            string parameter = parts[1];
            string defaultValue = (parts.Length > 2)
                ? parts[2]
                : string.Empty;
            string result = Context.Client.Settings[section, parameter];
            if (string.IsNullOrEmpty(result))
            {
                result = defaultValue;
            }
            return result;
        }

        public string ExecuteFormat
            (
                string format
            )
        {
            PftFormatter formatter = new PftFormatter(Context);
            formatter.ParseInput(format);
            string result = formatter.Format(Context.Record);
            return result;
        }

        public string ExecuteFormatName
            (
                string format
            )
        {
            if (string.IsNullOrEmpty(format))
            {
                return string.Empty;
            }
            format = format + ".pft";
            string text = Context.Client.ReadTextFile(format);
            return ExecuteFormat(text);
        }

        public string GetFieldRepetition
            (
                string format
            )
        {
            if (string.IsNullOrEmpty(format))
            {
                return string.Empty;
            }
            if (format.Length < 2)
            {
                return string.Empty;
            }
            FieldReference field = new FieldReference(format);
            string result = field.FormatSingle(Context.Record);
            return result;
        }

        public string GetFieldRepetitionCount
            (
                string format
            )
        {
            int count = Context.Record.Fields.GetField(format).Length;
            return count.ToInvariantString();
        }

        public string Evaluate
            (
                string format
            )
        {
            if (string.IsNullOrEmpty(format))
            {
                return string.Empty;
            }

            char firstLetter = char.ToUpperInvariant(format[0]);
            char secondLetter = '\0';
            char thirdLetter = '\0';
            string format1 = format;
            string format2 = format;
            if (format.Length > 1)
            {
                format1 = format1.Substring(1);
                secondLetter = char.ToUpperInvariant(format[1]);
            }
            if (format.Length > 2)
            {
                format2 = format2.Substring(2);
                thirdLetter = char.ToLowerInvariant(format[2]);
            }
            format = format.Substring(1);
            
            StringBuilder result = new StringBuilder();

            switch (firstLetter)
            {
                // Сравнение по маске
                case '=':
                    break;

                // Дополнительные команды
                case '+':
                    switch (secondLetter)
                    {
                        // Выдать содержимое документа полностью во внутреннем представлении
                        case '0':
                            // TODO: implement properly
                            result.Append(ManagedClient64.DecodeNewLines(Context.Record.ToString()));
                            break;

                        // Очистить (опустошить) все глобальные переменные
                        case '1':
                            switch (thirdLetter)
                            {

                                // Сложение списков (групп переменных)
                                case 'A':
                                    break;

                                // Исключение неоригинальных значений из группы переменных
                                case 'G':
                                    break;

                                // Исключение неоригинальных значений из списка
                                case 'I':
                                    break;

                                // Групповая мультираскодировка переменных
                                case 'K':
                                    break;

                                // Перемножение двух списков (групп переменных)
                                case 'M':
                                    break;

                                // Групповая мультираскодировка списка
                                case 'O':
                                    break;

                                // Чтение глобальных переменных
                                case 'R':
                                    break;

                                // Вычитание списков (групп переменных)
                                case 'S':
                                    break;

                                // Сортировка группы переменных
                                case 'T':
                                    break;

                                // Сортировка списка
                                case 'V':
                                    break;

                                // Запись в глобальные переменные
                                case 'W':
                                    break;
                            }
                            break;

                        // Выдача метки, порядкового номера и значения поля в соответствии с индексом 
                        // (номером повторения) повторяющейся группы.
                        case '4':
                            break;

                        // Выдача элемента списка/справочника в соответствии с индексом 
                        // (номером повторения) повторяющейся группы.
                        case '5':
                            break;

                        // Выдать статус записи. Если запись логически удаленная, 
                        // возвращается 0, в противном случае - 1
                        case '6':
                            result.Append(Context.Record.Deleted ? "0" : "1");
                            break;

                        // Работа с индивидуальными повторяющимися глобальными переменными
                        case '7':
                            switch (thirdLetter)
                            {
                                // Очистить (опустошить) все глобальные переменные
                                case '\0':
                                    break;

                                // Логическое сложение повторений двух переменных
                                case 'A':
                                    break;

                                // Исключение неоригинальных повторений переменной
                                case 'G':
                                    break;

                                // Логическое перемножение повторений двух переменных
                                case 'M':
                                    break;

                                // Чтение глобальной переменной
                                case 'R':
                                    break;

                                // Логическое вычитание повторений двух переменных
                                case 'S':
                                    break;

                                // Сортировка повторений переменной
                                case 'T':
                                    break;

                                // Добавление повторений глобальной переменной
                                case 'U':
                                    break;

                                // Запись глобальной переменной
                                case 'W':
                                    break;
                            }
                            break;

                        // Подключение функций пользователя
                        case '8':
                            break;

                        // Группа технических форматных выходов
                        case '9':
                            switch (thirdLetter)
                            {
                                // Вернуть номер текущего повторения в повторяющейся группе (исходные данные не задаются)
                                case '0':
                                    result.Append
                                        (
                                            (Group == null ? 0 : Group.GroupIndex).ToInvariantString()
                                        );
                                    break;

                                // Вернуть имя файла из заданного полного пути/имени
                                case '1':
                                    result.Append(Path.GetFileName(format2));
                                    break;

                                // Вернуть путь из заданного полного пути/имени
                                case '2':
                                    result.Append(Path.GetDirectoryName(format2));
                                    break;

                                // Вернуть расширение из заданного полного пути/имени
                                case '3':
                                    result.Append(Path.GetExtension(format2));
                                    break;

                                // Вернуть имя диска из заданного полного пути
                                case '4':
                                    if (format2.StartsWith("\\"))
                                    {
                                        result.Append(format2);
                                    }
                                    else if ((format2.Length > 1) && (format2[1]==':'))
                                    {
                                        result.Append(format2.Substring(0, 2));
                                    }
                                    break;

                                // Вернуть длину исходной строки
                                case '5':
                                    result.Append(format2.Length.ToInvariantString());
                                    break;

                                // Вернуть фрагмент строки: +96A*SSS.NNN#<строка>
                                case '6':
                                    break;

                                // Вернуть строку в верхнем регистре
                                case '7':
                                    result.Append(format2.ToUpper());
                                    break;

                                // Заменить в заданной строке один символ на другой (регистр учитывается)
                                case '8':
                                    break;

                                // Групповая установка глобальных переменных (для ИРБИС-Навигатора)
                                case '9':
                                    break;

                                // Вставить данные из заданного текстового файла
                                case 'C':
                                    break;

                                // Сохранить заданный внутренний двоичный объект в заданном файле
                                case 'D':
                                    break;

                                // Вернуть ANSI-символ с заданным кодом
                                case 'F':
                                    break;

                                // Преобразовать заданную строку в список слов
                                case 'G':
                                    break;

                                // !AAAA!/BBBB/',<данные>
                                case 'I':
                                    break;

                                // Полный путь и имя файла - представить заданный ДВОИЧНЫЙ файл в виде
                                case 'J':
                                    break;

                                // Полный путь и имя файла - удалить заданный файл
                                case 'K':
                                    break;

                                // Преобразование римского числа в арабское
                                case 'R':
                                    break;

                                // Под каким ИРБИСом выполняется: 32 или 64
                                case 'V':
                                    result.Append("64");
                                    break;

                                // Преобразование арабского числа в римское
                                case 'X':
                                    break;
                            }
                            break;

                        // Возвращает порядковый номер заданного поля в записи.
                        case 'E':
                            break;

                        // Команда постредактуры: очистить результат расформатирования от RTF-конструкций. 
                        // Имеет смысл использовать один раз в любом месте формата
                        case 'F':
                            break;

                        // Формирование ссылки (гиперссылки)
                        case 'I':
                            break;

                        // Выдать количество повторений поля
                        case 'N':
                            result.Append(GetFieldRepetitionCount(format2));
                            break;
                    }
                    break;

                // Команда постредактуры: очистить результат расформатирования 
                // от двойных разделителей (двойных точек или двойных конструкций <. - >). 
                // Имеет смысл использовать один раз в любом месте формата
                case '!':
                    break;

                // Выдать содержимое документа полностью в формате RTF
                case '0':
                    // TODO: implement properly
                    result.Append(ManagedClient64.DecodeNewLines(Context.Record.ToString()));
                    break;

                // Вернуть заданный подэлемент
                case '1':
                    break;

                // Выдача данных, связанных с ДАТОЙ и ВРЕМЕНЕМ
                case '3':
                {
                    DateTime now = DateTime.Now;
                    switch (secondLetter)
                    {
                        case '0':
                            result.AppendFormat("{0:yyyy}", now);
                            break;
                        case '1':
                            result.AppendFormat("{0:MM}", now);
                            break;
                        case '2':
                            result.AppendFormat("{0:dd}", now);
                            break;
                        case '3':
                            result.AppendFormat("{0:yy}", now);
                            break;
                        case '4':
                            result.AppendFormat("{0:M}", now);
                            break;
                        case '5':
                            result.AppendFormat("{0:d}", now);
                            break;
                        case '9':
                            result.AppendFormat("{0:T}", now);
                            break;
                        default:
                            result.AppendFormat("{0:yyyyMMdd}", now);
                            break;
                    }
                }
                    break;

                // ФОРМАТИРОВАНИЕ ПРЕДЫДУЩЕЙ КОПИИ ТЕКУЩЕЙ ЗАПИСИ
                case '4':
                    break;

                // Выполнить формат
                case '6':
                    result.Append(ExecuteFormatName(format1));
                    break;

                // Расформатирование группы связанных документов из другой БД 
                // (отношение «от одного к многим»).
                case '7':
                    break;

                // Удалить двойные кавычки из заданной строки
                case '9':
                    result.Append(format1.Replace("\"", string.Empty));
                    break;

                // Выдать заданное повторение поля
                case 'A':
                    result.Append(GetFieldRepetition(format1));
                    break;

                // Выдать библиографическую свертку документа
                case 'B':
                    break;

                // Контроль ISSN/ISBN
                // При положительном результате - 0
                // При отрицательном - 1
                case 'C':
                    break;

                // Форматирование документа из другой БД 
                // (REF на другую БД – отношение «от одного к одному»)
                case 'D':
                    break;

                // Вернуть заданное количество первых слов в строке
                case 'E':
                    break;

                // Вернуть конец  строки после заданного кол-ва первых слов
                case 'F':
                    break;

                // Вернуть часть строки до или начиная с заданного символа
                case 'G':
                    break;

                // Вернуть параметр из INI-файла
                case 'I':
                    result.Append(DecodeViaIniFile(format));
                    break;

                // Вернуть кол-во ссылок для заданного термина
                case 'J':
                    break;

                // Раскодировка через справочник (меню)
                case 'K':
                    result.Append(DecodeViaDictionary(format));
                    break;

                // Вернуть окончание термина
                case 'L':
                    break;

                // Отсортировать повторения заданного поля (имеется в виду строковая сортировка) 
                // - функция ничего не возвращает. Можно применять только в глобальной корректировке.
                case 'M':
                    break;

                // Выдать заданное оригинальное повторение поля
                case 'P':
                    break;

                // Вернуть заданную строку в нижнем регистре
                case 'Q':
                    result.Append(format.ToLowerInvariant());
                    break;

                // Генерация случайного числа
                case 'R':
                    // TODO: implement properly
                    result.Append(new Random().Next());
                    break;

                // Универсальный счетчик
                case 'S':
                    break;

                // Транслитерирование кириллических символов с помощью латиницы
                case 'T':
                    break;

                // Куммуляция номеров журналов
                case 'U':
                    break;

                // Декуммуляция номеров журналов
                case 'V':
                    break;

                // Контроль куммуляции
                case 'W':
                    break;

                // Удаление из заданной строки фрагментов, выделенных угловыми скобками <>
                case 'X':
                    break;

                // Размножение экземпляров (функция ничего не возвращает). 
                // Можно применять только в глобальной корректировке
                case 'Z':
                    break;
            }

            return result.ToString();
        }

        #endregion
    }
}
