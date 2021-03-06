﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SubFieldAttribute.cs
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace ManagedClient.Mapping
{
    /// <summary>
    /// Задаёт отображение подполя на свойство.
    /// </summary>
    [Serializable]
    [PublicAPI]
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]
    public sealed class SubFieldAttribute
        : Attribute
    {
        #region Properties

        /// <summary>
        /// Code.
        /// </summary>
        public char Code { get; set; }

        /// <summary>
        /// Occurrence.
        /// </summary>
        public int Occurence { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SubFieldAttribute
            (
                char code
            )
        {
            Code = code;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SubFieldAttribute
            (
                char code,
                int occurence
            )
        {
            Code = code;
            Occurence = occurence;
        }

        #endregion
    }
}
