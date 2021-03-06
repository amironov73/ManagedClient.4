﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TermLink.cs
 */

#region Using directives

using System;
using System.IO;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Term link.
    /// </summary>
    [Serializable]
    [MoonSharpUserData]
    public sealed class TermLink
    {
        #region Properties

        /// <summary>
        /// MFN.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Tag.
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// Occurrence.
        /// </summary>
        public int Occurrence { get; set; }

        /// <summary>
        /// Index.
        /// </summary>
        public int Index { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Read the <see cref="TermLink"/>
        /// from the <see cref="Stream"/>.
        /// </summary>
        [NotNull]
        public static TermLink Read
            (
                [NotNull] Stream stream
            )
        {
            TermLink result = new TermLink
            {
                Mfn = stream.ReadInt32Network(),
                Tag = stream.ReadInt32Network(),
                Occurrence = stream.ReadInt32Network(),
                Index = stream.ReadInt32Network()
            };

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return string.Format
                (
                    "Mfn: {0}, Tag: {1}, "
                    + "Occurrence: {2}, Index: {3}",
                    Mfn,
                    Tag,
                    Occurrence,
                    Index
                );
        }

        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        private bool Equals
            (
                [NotNull] TermLink other
            )
        {
            return Mfn == other.Mfn
                && Tag == other.Tag
                && Occurrence == other.Occurrence
                && Index == other.Index;
        }

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals
            (
                object obj
            )
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;

            return obj is TermLink
                && Equals((TermLink)obj);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            unchecked
            {
                // ReSharper disable NonReadonlyMemberInGetHashCode

                int hashCode = Mfn;
                hashCode = (hashCode * 397) ^ Tag;
                hashCode = (hashCode * 397) ^ Occurrence;
                hashCode = (hashCode * 397) ^ Index;

                // ReSharper restore NonReadonlyMemberInGetHashCode

                return hashCode;
            }
        }

        #endregion
    }
}
