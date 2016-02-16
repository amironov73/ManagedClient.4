﻿/* MstControlRecord32.cs
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace ManagedClient.Direct
{
    /// <summary>
    /// Первая запись в файле документов – управляющая 
    /// запись, которая формируется (в момент определения 
    /// базы данных или при ее инициализации) и поддерживается 
    /// автоматически.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public sealed class MstControlRecord32
    {
        #region Constants

        /// <summary>
        /// Размер управляющей записи.
        /// </summary>
        public const int RecordSize = 16;

        #endregion

        #region Properties

        /// <summary>
        /// Всегда 0.
        /// </summary>
        public int Zero { get; set; }

        /// <summary>
        /// Номер записи файла документов, назначаемый 
        /// для следующей записи, создаваемой в базе данных.
        /// </summary>
        public int NextMfn { get; set; }

        /// <summary>
        /// Номер последнего блока файла документов.
        /// </summary>
        public int NextBlock { get; set; }

        /// <summary>
        /// Смещение следующей доступной позиции в последнем блоке.
        /// </summary>
        public short NextOffset { get; set; }

        /// <summary>
        /// Всегда 0 для файлов баз данных пользователя.
        /// </summary>
        public short Type { get; set; }

        #endregion
    }
}
