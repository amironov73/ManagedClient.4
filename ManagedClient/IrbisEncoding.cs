// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisEncoding.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using CM = System.Configuration.ConfigurationManager;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Encoding used by IRBIS
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class IrbisEncoding
    {
        #region Properties

        /// <summary>
        /// Default single-byte encoding.
        /// </summary>
        [NotNull]
        public static Encoding Ansi { get { return _ansi; } }

        /// <summary>
        /// OEM encoding.
        /// </summary>
        public static Encoding Oem { get { return _oem; } }

        /// <summary>
        /// UTF8 encoding.
        /// </summary>
        public static Encoding Utf8 { get { return _utf8; } }

        #endregion

        #region Private members

        private static Encoding _ansi = Encoding.GetEncoding(1251);

        private static Encoding _oem = Encoding.GetEncoding(866);

        private static Encoding _utf8 = new UTF8Encoding
            (
                false, // don't emit UTF-8 prefix
                true   // throw on invalid bytes
            );

        #endregion

        #region Public methods

        /// <summary>
        /// Get encoding by name.
        /// </summary>
        [NotNull]
        public static Encoding ByName
            (
                [CanBeNull] string name
            )
        {
            if (string.IsNullOrEmpty(name))
            {
                return Utf8;
            }

            if (name.SameString("Ansi"))
            {
                return Ansi;
            }
            if (name.SameString("Dos")
                || name.SameString("MsDos")
                || name.SameString("Oem"))
            {
                return Oem;
            }
            if (name.SameString("Utf")
                || name.SameString("Utf8")
                || name.SameString("Utf-8"))
            {
                return Utf8;
            }

            Encoding result = Encoding.GetEncoding(name);

            return result;
        }

        /// <summary>
        /// Get encoding from config file.
        /// </summary>
        [NotNull]
        public static Encoding FromConfig
            (
                [NotNull] string key
            )
        {
            string name = CM.AppSettings[key];
            Encoding result = ByName(name);

            return result;
        }

        /// <summary>
        /// Relax UTF-8 decoder, do not throw exceptions
        /// on invalid bytes.
        /// </summary>
        public static void RelaxUtf8()
        {
            _utf8 = new UTF8Encoding
                (
                    false, // don't emit UTF-8 prefix,
                    false  // don't throw on invalid bytes
                );
        }

        /// <summary>
        /// Override default single-byte encoding.
        /// </summary>
        public static void SetAnsiEncoding
            (
                [NotNull] Encoding encoding
            )
        {
            if (!encoding.IsSingleByte)
            {
                throw new ArgumentOutOfRangeException("encoding");
            }

            _ansi = encoding;
        }

        /// <summary>
        /// Override OEM encoding.
        /// </summary>
        public static void SetOemEncoding
            (
                [NotNull] Encoding encoding
            )
        {
            if (!encoding.IsSingleByte)
            {
                throw new ArgumentOutOfRangeException("encoding");
            }

            _oem = encoding;
        }

        #endregion
    }
}
