// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MorphologyEntry.cs
 */

#region Using directives

using System;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Morphology
{
    /// <summary>
    /// Entry of the morphology database.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    public sealed class MorphologyEntry
    {
        #region Properties

        /// <summary>
        /// Main term. Field 10.
        /// </summary>
        public string MainTerm { get; set; }

        /// <summary>
        /// Dictionary term. Field 11.
        /// </summary>
        public string Dictionary { get; set; }

        /// <summary>
        /// Language name. Field 12.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Forms of the word. Repeatable field 20.
        /// </summary>
        public string[] Forms { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the record.
        /// </summary>
        [NotNull]
        public static MorphologyEntry Parse
            (
                [NotNull] IrbisRecord record
            )
        {
            if (ReferenceEquals(record, null))
            {
                throw new ArgumentException("record");
            }

            MorphologyEntry result = new MorphologyEntry
            {
                MainTerm = record.FM("10"),
                Dictionary = record.FM("11"),
                Language = record.FM("12"),
                Forms = record.FMA("20")
            };

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return MainTerm;
        }

        #endregion
    }
}
