// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CharacterClass.cs -- класс символов Unicode.
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Класс символов Unicode.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum CharacterClass
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0x00,

        /// <summary>
        /// Control character.
        /// </summary>
        ControlCharacter = 0x01,

        /// <summary>
        /// Digit.
        /// </summary>
        Digit = 0x02,

        /// <summary>
        /// Basic Latin.
        /// </summary>
        BasicLatin = 0x04,

        /// <summary>
        /// Cyrillic.
        /// </summary>
        Cyrillic = 0x08
    }
}
