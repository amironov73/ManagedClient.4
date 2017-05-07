﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IfpRecord.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Direct
{
    //
    // Extract from official documentation:
    // http://sntnarciss.ru/irbis/spravka/wtcp006005030.htm
    //
    // Файл IFP содержит список ссылок для каждого термина словаря.
    //
    // Список ссылок может быть представлен в 2-х различных форматах.
    // Выбор формата размещения ссылок осуществляется при загрузке 
    // словаря из файла Lk1 (этот файл формируется после отбора
    // и сортировки терминов) в зависимости от общего числа ссылок
    // для данного термина. Обыкновенный формат – это заголовок блока
    // и набор упорядоченных ссылок. По превышении определенного
    // числа ссылок (MIN_POSTINGS_IN_BLOCK - в данной реализации 256)
    // формат включает специальный блок и набор блоков обыкновенного
    // формата размер которых определяется по следующей схеме:
    // блоки 4,8,16,32 Kb для общего числа ссылок соответственно
    // 256-32000 ; 32000-64000 ; 64000-128000 ; 128000 и более.
    // 
    // Такая схема оптимизирует работу с диском в процессе
    // инвертирования записи в базах данных, характеризующихся
    // большим количеством ссылок на термин.
    //
    // Обыкновенный формат записи IFP
    //
    // Запись состоит из заголовка и упорядоченного набора ссылок.
    //
    // Ссылка имеет следующий формат:
    // Число бит Параметр
    // 1         PMFN – номер записи;
    // 32        PTAG – идентификатор поля назначенный
    //           при отборе терминов в словарь;
    // 32        POCC – номер повторения;
    // 32        PCNT – номер термина в поле.
    //
    // Заголовок имеет следующий формат:
    // Число бит Параметр
    // 2         LOW – младшее слово смещения
    //           на следующую запись(если нет  0);
    // 32        HIGH – старшее слово смещения
    //           на следующую запись(если нет  0);
    // 3         TOTP – общее число ссылок для данного
    //           термина(только в первой записи);
    //           число ссылок в данном блоке(в следующих записях);
    // 32        SEGP - число ссылок в данном блоке;
    // 32        SEGC – вместимость записи в ссылках.
    // Признак последнего блока – LOW= HIGH = -1
    //
    // Специальный формат записи IFP
    //
    // В этом случае первой записью является специальный блок,
    // который представляет собой заголовок (обыкновенного формата),
    // в котором смещения имеют специальные значения = -1001,
    // и набор входов следующего формата:
    // Число бит Параметр
    // 4         POSTING – 1-я ссылка из записи обыкновенного формата;
    // 5         LOW – младшее слово смещения на следующую
    //           запись(если нет  0);
    // 32        HIGH– младшее слово смещения на следующую запись
    //           (если нет  0);
    //
    // Число входов кратно 4. Записи, на которые ссылается
    // специальный блок, связаны между собой как описано выше.
    // Причем общее количество ссылок для данного термина сохраняется
    // только в специальном блоке.
    //
    // Модификация записей файла IFP
    // При выполнении актуализации инверсного файла могут
    // создаваться новые дополнительные записи при добавлении
    // новых ссылок.В этом случае создается новая запись размером
    // равным общему количеству ссылок, если нет специального
    // блока, и размером, равным количеству ссылок в данной записи,
    // если есть. Новая запись создается таким образом, чтобы
    // не нарушалась возрастающая последовательность следования
    // ссылок. Новая запись связывается с существующими через
    // поле NXT_, ссылки распределяются равномерно между старой
    // и новой записью.
    //

    /// <summary>
    /// IFP file record.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    public sealed class IfpRecord
    {
        #region Constants

        /// <summary>
        /// Число ссылок на термин, после превышения которого
        /// используется специальный блок ссылок.
        /// </summary>
        public const int MinPostingsInBlock = 256;

        #endregion

        #region Nested classes

        /// <summary>
        /// ibatrak структура с описанием спец блока
        /// </summary>
        private class SpecialPostingBlockEntry
        {

            /// <summary>
            /// 1-я ссылка из записи обыкновенного формата
            /// </summary>
            public int Posting { get; set; }

            /// <summary>
            /// младшее слово смещения на следующую запись (если нет 0)
            /// </summary>
            public int Low { get; set; }

            /// <summary>
            /// старшее слово смещения на следующую запись (если нет 0)
            /// </summary>
            public int High { get; set; }

            /// <summary>
            /// Offset.
            /// </summary>
            public long Offset
            {
                get
                {
                    return unchecked((uint)High << 4) | (uint)Low;
                }
                set
                {
                    unchecked
                    {
                        Low = (int)(value & 0xFFFFFFFF);
                        High = (int)(value >> 32);
                    }
                }
            }

            /// <inheritdoc cref="object.ToString"/>
            public override string ToString()
            {
                return string.Format
                (
                    "Offset:{0}, Posting: {1}",
                    Offset,
                    Posting
                );
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Low part of the offset.
        /// </summary>
        public int LowOffset { get; set; }

        /// <summary>
        /// High part of the offset.
        /// </summary>
        public int HighOffset { get; set; }

        /// <summary>
        /// ibatrak ссылка на следующую запись
        /// </summary>
        public long FullOffset
        {
            get
            {
                return unchecked(((long)HighOffset << 32) + LowOffset);
            }
        }

        /// <summary>
        /// Total link count.
        /// </summary>
        public int TotalLinkCount { get; set; }

        /// <summary>
        /// Block link count.
        /// </summary>
        public int BlockLinkCount { get; set; }

        /// <summary>
        /// Capacity.
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Links.
        /// </summary>
        [NotNull]
        public List<TermLink> Links { get { return _links; } }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private readonly List<TermLink> _links = new List<TermLink>();

        #endregion

        #region Public methods

        /// <summary>
        /// ibatrak чтение заголовка записи IFP (для подсчета количества ссылок)
        /// </summary>
        public static IfpRecord ReadLeader
        (
            Stream stream,
            long offset
        )
        {
            stream.Position = offset;

            IfpRecord result = new IfpRecord
            {
                LowOffset = stream.ReadInt32Network(),
                HighOffset = stream.ReadInt32Network(),
                TotalLinkCount = stream.ReadInt32Network(),
                BlockLinkCount = stream.ReadInt32Network(),
                Capacity = stream.ReadInt32Network()
            };

            return result;
        }

        /// <summary>
        /// Read the record.
        /// </summary>
        public static IfpRecord Read
            (
                [NotNull] Stream stream,
                long offset
            )
        {
            stream.Position = offset;

            IfpRecord result = new IfpRecord
            {
                LowOffset = stream.ReadInt32Network(),
                HighOffset = stream.ReadInt32Network(),
                TotalLinkCount = stream.ReadInt32Network(),
                BlockLinkCount = stream.ReadInt32Network(),
                Capacity = stream.ReadInt32Network()
            };

            // ibatrak чтение вложенных записей в спец блоке
            // Специальный формат записи .ifp
            // В этом случае первой записью является специальный блок,
            // который представляет собой заголовок (обыкновенного формата),
            // в котором смещения имеют специальные значения = -1001
            if (result.LowOffset == -1001 && result.HighOffset == -1001)
            {
                // irbis64.dll делает так

                // читает первые 24 байта блока спец ссылок
                // (это 2 записи)
                // записи с реальными адресами идут через одну
                // берет вторую запись, адрес из нее в файле
                // и читает записеи IFP
                // по количеству result.BlockLinkCount

                // первую запись спец блока просто пропускаем,
                // читаем сразу вторую
                stream.Position += 12;

                SpecialPostingBlockEntry entry = new SpecialPostingBlockEntry
                {
                    Posting = stream.ReadInt32Network(),
                    Low = stream.ReadInt32Network(),
                    High = stream.ReadInt32Network()
                };

                stream.Position = entry.Offset;

                IfpRecord[] nestedRecords
                    = new IfpRecord[result.BlockLinkCount];
                for (int i = 0; i < result.BlockLinkCount; i++)
                {
                    var nestedRecord = Read(stream, stream.Position);
                    nestedRecords[i] = nestedRecord;

                    // Last record in the list must have
                    // negative offset values
                    if (nestedRecord.LowOffset == -1
                        && nestedRecord.HighOffset == -1)
                    {
                        if (i != result.BlockLinkCount - 1)
                        {
                            throw new InvalidOperationException
                            (
                                "IFP reading error"
                            );
                        }
                        break;
                    }
                    if (nestedRecord.FullOffset < 0)
                    {
                        throw new InvalidOperationException
                        (
                            "IFP reading error"
                        );
                    }

                    stream.Position = nestedRecord.FullOffset;
                }

                TermLink[] links = nestedRecords
                    .SelectMany(r => r.Links)
                    .ToArray();
                if (links.Length != result.TotalLinkCount)
                {
                    throw new InvalidOperationException
                    (
                        "IFP reading error"
                    );
                }
                result.Links.AddRange(links);

                return result;
            }
            //ibatrak до сюда

            for (int i = 0; i < result.BlockLinkCount; i++)
            {
                TermLink link = TermLink.Read(stream);
                result.Links.Add(link);
            }

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (TermLink link in Links)
            {
                builder.AppendLine(link.ToString());
            }

            return string.Format
                (
                    "LowOffset: {0}, HighOffset: {1}, TotalLinkCount: {2}, "
                    + "BlockLinkCount: {3}, Capacity: {4}\r\nItems: {5}", 
                    LowOffset, 
                    HighOffset,
                    TotalLinkCount,
                    BlockLinkCount,
                    Capacity,
                    builder
                );
        }

        #endregion
    }
}
