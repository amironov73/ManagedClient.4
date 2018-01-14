// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* XrfFile64.cs
 */

#region Using directives

using System;
using System.IO;

using JetBrains.Annotations;

#endregion

namespace ManagedClient.Direct
{
    /// <summary>
    /// Файл перекрестных ссылок XRF представляет собой
    /// таблицу ссылок на записи файла документов.
    /// Первая ссылка соответствует записи файла документов
    /// с номером 1, вторая – 2  и тд.
    /// </summary>
    public sealed class XrfFile64
        : IDisposable
    {
        #region Constants
        #endregion

        #region Properties

        /// <summary>
        /// File name.
        /// </summary>
        [NotNull]
        public string FileName { get; private set; }

        /// <summary>
        /// All the data in the memory?
        /// </summary>
        public bool InMemory { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public XrfFile64
            (
                [NotNull] string fileName,
                bool inMemory
            )
        {
            FileName = fileName;
            InMemory = inMemory;

            _stream = new FileStream
                (
                    fileName,
                    FileMode.Open,
                    FileAccess.Read,
                   FileShare.ReadWrite
                );

            if (InMemory)
            {
                byte[] buffer = new byte[(int)_stream.Length];
                Stream memory = new MemoryStream(buffer);
                _stream.Read(buffer, 0, buffer.Length);
                _stream.Close();
                _stream = memory;
            }
        }

        #endregion

        #region Private members

        private readonly Stream _stream;

        private long _GetOffset
            (
                int mfn
            )
        {
            // ibatrak умножение в Int32 с преобразованием результата в Int64,
            // при больших mfn вызывает переполнение и результат
            // становится отрицательным
            long result = (long)XrfRecord64.RecordSize * (mfn - 1);

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Read the record.
        /// </summary>
        [NotNull]
        public XrfRecord64 ReadRecord
            (
                int mfn
            )
        {
            if (mfn <= 0)
            {
                throw new ArgumentOutOfRangeException("mfn");
            }

            long offset = _GetOffset(mfn);
            if (offset >= _stream.Length)
            {
                throw new ArgumentOutOfRangeException("mfn");
            }

            if (_stream.Seek(offset, SeekOrigin.Begin) != offset)
            {
                throw new IOException();
            }

            long ofs = _stream.ReadInt64Network();
            int flags = _stream.ReadInt32Network();

            XrfRecord64 result = new XrfRecord64
            {
                Mfn = mfn,
                Offset = ofs,
                Status = (RecordStatus)flags
            };

            return result;
        }

        /// <summary>
        /// Write the record.
        /// </summary>
        public void WriteRecord
            (
                [NotNull] XrfRecord64 record
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Lock/unlock the record.
        /// </summary>
        public void LockRecord
            (
                int mfn,
                bool flag
            )
        {
            XrfRecord64 record = ReadRecord(mfn);
            if (flag != record.Locked)
            {
                WriteRecord(record);
            }
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            _stream.Dispose();
        }

        #endregion
    }
}
