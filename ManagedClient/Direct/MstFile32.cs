﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MstFile32.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#endregion

namespace ManagedClient.Direct
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MstFile32
        : IDisposable
    {
        #region Constants
        #endregion

        #region Properties

        /// <summary>
        /// Preload length.
        /// </summary>
        public static int PreloadLength = 6 * 1024;

        /// <summary>
        /// Control record.
        /// </summary>
        public MstControlRecord32 ControlRecord { get; private set; }

        /// <summary>
        /// File name.
        /// </summary>
        public string FileName { get; private set; }

        #endregion

        #region Construction

        public MstFile32(string fileName)
        {
            FileName = fileName;

            _stream = new FileStream
                (
                    fileName,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite
                );

            _ReadControlRecord();
        }

        #endregion

        #region Private members

        private readonly FileStream _stream;

        private void _ReadControlRecord()
        {
            ControlRecord = new MstControlRecord32
            {
                Zero = _stream.ReadInt32Host(),
                NextMfn = _stream.ReadInt32Host(),
                NextBlock = _stream.ReadInt32Host(),
                NextOffset= _stream.ReadInt16Host(),
                Type = _stream.ReadInt16Host()
            };
        }

        #endregion

        #region Public methods

        public MstRecord32 ReadRecord(long offset)
        {
            if (_stream.Seek(offset, SeekOrigin.Begin) != offset)
            {
                throw new IOException();
            }

            //new ObjectDumper()
            //    .DumpStream(_stream,offset,64)
            //    .WriteLine();

            //Encoding encoding = new UTF8Encoding(false, true);
            Encoding encoding = Encoding.Default;

            MstRecordLeader32 leader = MstRecordLeader32.Read(_stream);

            List<MstDictionaryEntry32> dictionary
                = new List<MstDictionaryEntry32>();

            for (int i = 0; i < leader.Nvf; i++)
            {
                MstDictionaryEntry32 entry = new MstDictionaryEntry32
                {
                    Tag = _stream.ReadInt16Host(),
                    Position = _stream.ReadInt16Host(),
                    Length = _stream.ReadInt16Host()
                };
                dictionary.Add(entry);
            }

            foreach (MstDictionaryEntry32 entry in dictionary)
            {
                long endOffset = offset + leader.Base + entry.Position;
                _stream.Seek(endOffset, SeekOrigin.Begin);
                entry.Bytes = _stream.ReadBytes(entry.Length);
                if (entry.Bytes != null)
                {
                    entry.Text = encoding.GetString(entry.Bytes);
                }
            }

            MstRecord32 result = new MstRecord32
            {
                Leader = leader,
                Dictionary = dictionary
            };
            return result;
        }

        private static void _AppendStream
            (
                Stream source,
                Stream target,
                int amount
            )
        {
            if (amount <= 0)
            {
                throw new IOException();
                //return false;
            }
            long savedPosition = target.Position;
            target.Position = target.Length;

            byte[] buffer = new byte[amount];
            int readed = source.Read(buffer, 0, amount);
            if (readed <= 0)
            {
                throw new IOException();
                //return false;
            }
            target.Write(buffer, 0, readed);
            target.Position = savedPosition;
            //return true;
        }

        public MstRecord32 ReadRecord2
            (
                long offset
            )
        {
            if (_stream.Seek(offset, SeekOrigin.Begin) != offset)
            {
                throw new IOException();
            }

            Encoding encoding = Encoding.Default;

            MemoryStream memory = new MemoryStream(PreloadLength);
            _AppendStream(_stream, memory, PreloadLength);
            memory.Position = 0;

            MstRecordLeader32 leader = MstRecordLeader32.Read(memory);
            int amountToRead = (int)(leader.Length - memory.Length);
            if (amountToRead > 0)
            {
                _AppendStream(_stream, memory, amountToRead);
            }

            List<MstDictionaryEntry32> dictionary
                = new List<MstDictionaryEntry32>();

            for (int i = 0; i < leader.Nvf; i++)
            {
                MstDictionaryEntry32 entry = new MstDictionaryEntry32
                {
                    Tag = memory.ReadInt16Host(),
                    Position = memory.ReadInt16Host(),
                    Length = memory.ReadInt16Host()
                };
                dictionary.Add(entry);
            }

            foreach (MstDictionaryEntry32 entry in dictionary)
            {
                long endOffset = leader.Base + entry.Position;
                memory.Seek(endOffset, SeekOrigin.Begin);
                entry.Bytes = memory.ReadBytes(entry.Length);
                if (entry.Bytes != null)
                {
                    entry.Text = encoding.GetString(entry.Bytes);
                }
            }

            MstRecord32 result = new MstRecord32
            {
                Leader = leader,
                Dictionary = dictionary
            };
            return result;
        }


        /// <summary>
        /// Блокировка базы данных в целом.
        /// </summary>
        /// <param name="flag"></param>
        public void LockDatabase
            (
                bool flag
            )
        {
        }

        /// <summary>
        /// Чтение флага блокировки базы данных в целом.
        /// </summary>
        public bool ReadDatabaseLockedFlag()
        {
            return false;
        }

        #endregion

        #region IDisposable members

        public void Dispose()
        {
            if (_stream != null)
            {
                _stream.Dispose();
            }
        }

        #endregion

        #region Object members

        #endregion
    }
}
