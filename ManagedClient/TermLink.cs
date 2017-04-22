/* TermLink.cs
 */

#region Using directives

using System;
using System.IO;

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

        public static TermLink Read(Stream stream)
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

        private bool Equals(TermLink other)
        {
            return (Mfn == other.Mfn) 
                && (Tag == other.Tag) 
                && (Occurrence == other.Occurrence) 
                && (Index == other.Index);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return (obj is TermLink) 
                && Equals((TermLink) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Mfn;
                hashCode = (hashCode*397) ^ Tag;
                hashCode = (hashCode*397) ^ Occurrence;
                hashCode = (hashCode*397) ^ Index;
                return hashCode;
            }
        }

        #endregion
    }
}
