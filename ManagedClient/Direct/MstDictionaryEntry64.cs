// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MstDictionaryEntry64.cs
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace ManagedClient.Direct
{
    /// <summary>
    /// Элемент справочника MST-файла,
    /// описывающий поле переменной длины.
    /// </summary>
    [Serializable]
    [ComVisible(false)]
    [StructLayout(LayoutKind.Sequential)]
    public sealed class MstDictionaryEntry64
    {
        #region Constants

        /// <summary>
        /// Длина элемента справочника MST-файла.
        /// </summary>
        public const int EntrySize = 12;

        #endregion

        #region Properties

        /// <summary>
        /// Tag.
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// Position.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Length.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Value as array of bytes.
        /// </summary>
        public byte[] Bytes { get; set; }

        /// <summary>
        /// Value as text string.
        /// </summary>
        public string Text { get; set; }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString ( )
        {
            return string.Format 
                ( 
                    "Tag: {0}, Position: {1}, Length: {2}, Text: {3}", 
                    Tag, 
                    Position, 
                    Length,
                    Text
                );
        }

        #endregion
    }
}
