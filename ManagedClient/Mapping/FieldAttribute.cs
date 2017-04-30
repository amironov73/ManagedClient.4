// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FieldAttribute.cs
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace ManagedClient.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [PublicAPI]
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]
    public sealed class FieldAttribute
        : Attribute
    {
        #region Properties

        /// <summary>
        /// Tag.
        /// </summary>
        [NotNull]
        public string Tag { get; private set; }

        /// <summary>
        /// Occurrence.
        /// </summary>
        public int Occurence { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldAttribute 
            (
                [NotNull] string tag 
            )
        {
            Tag = tag;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldAttribute 
            ( 
                [NotNull] string tag, 
                int occurence 
            )
        {
            Tag = tag;
            Occurence = occurence;
        }

        #endregion
    }
}
