// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IsoReader.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.ImportExport
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IsoReader
        : IEnumerable<IrbisRecord>,
        IDisposable
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IsoReader
            (
                [NotNull] string fileName,
                [NotNull] Encoding encoding
            )
        {
            _stream = new FileStream
                (
                    fileName,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read
                );
            _encoding = encoding;
            _ownStream = true;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IsoReader
            (
                [NotNull] Stream stream,
                [NotNull] Encoding encoding
            )
        {
            _stream = stream;
            _encoding = encoding;
            _ownStream = false;
        }

        #endregion

        #region Private members

        private readonly bool _ownStream;

        private readonly Stream _stream;

        private readonly Encoding _encoding;

        #endregion

        #region Public methods

        #endregion

        #region IEnumerable<T> members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" />
        /// object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" />
        /// that can be used to iterate through the collection.</returns>
        public IEnumerator<IrbisRecord> GetEnumerator()
        {
            while (true)
            {
                IrbisRecord record = Iso2709.ReadRecord(_stream, _encoding);

                if (ReferenceEquals(record, null))
                {
                    break;
                }

                yield return record;
            }
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            if (_ownStream
                && !ReferenceEquals(_stream, null))
            {
                _stream.Dispose();
            }
        }

        #endregion
    }
}

