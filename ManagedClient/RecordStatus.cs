﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RecordStatus.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Статус записи
    /// </summary>
    [Flags]
    public enum RecordStatus : byte
    {
        /// <summary>
        /// Запись логически удалена
        /// </summary>
        LogicallyDeleted = 1,

        /// <summary>
        /// Запись физически удалена
        /// </summary>
        PhysicallyDeleted = 2,

        /// <summary>
        /// Запись отсутствует
        /// </summary>
        Absent = 4,

        /// <summary>
        /// Запись не актуализирована
        /// </summary>
        NonActualized = 8,

        /// <summary>
        /// Последний экземпляр записи
        /// </summary>
        Last = 32,

        /// <summary>
        /// Запись заблокирована
        /// </summary>
        Locked = 64
    }
}
