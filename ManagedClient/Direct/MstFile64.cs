﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MstFile64.cs
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
    /// Record of MST file in IRBIS64.
    /// </summary>
    public sealed class MstFile64
        : IDisposable
    {
        #region Constants
        #endregion

        #region Properties

        /// <summary>
        /// Preload length.
        /// </summary>
        public static int PreloadLength = 10*1024;

        /// <summary>
        /// Control record.
        /// </summary>
        public MstControlRecord64 ControlRecord { get; private set; }

        /// <summary>
        /// File name.
        /// </summary>
        public string FileName { get; private set; }

        #endregion

        #region Construction

        public MstFile64 ( string fileName )
        {
            FileName = fileName;

            _stream = new FileStream 
                (
                    fileName,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite
                );

            _ReadControlRecord ();
        }

        #endregion

        #region Private members

        private bool _lockFlag;

        private readonly FileStream _stream;

        private void _ReadControlRecord ()
        {
            ControlRecord = new MstControlRecord64
                                {
                                    Reserv1 = _stream.ReadInt32Network (),
                                    NextMfn = _stream.ReadInt32Network (),
                                    NextPosition = _stream.ReadInt64Network (),
                                    Reserv2 = _stream.ReadInt32Network (),
                                    Reserv3 = _stream.ReadInt32Network (),
                                    Reserv4 = _stream.ReadInt32Network (),
                                    Blocked = _stream.ReadInt32Network ()
                                };
        }

        #endregion

        #region Public methods

        public MstRecord64 ReadRecord ( long offset )
        {
            if ( _stream.Seek ( offset, SeekOrigin.Begin ) != offset )
            {
                throw new IOException();
            }

            //new ObjectDumper()
            //    .DumpStream(_stream,offset,64)
            //    .WriteLine();

            Encoding encoding = new UTF8Encoding(false,true);

            MstRecordLeader64 leader = MstRecordLeader64.Read(_stream);

            List <MstDictionaryEntry64> dictionary 
                = new List < MstDictionaryEntry64 > ();

            for ( int i = 0; i < leader.Nvf; i++ )
            {
                MstDictionaryEntry64 entry = new MstDictionaryEntry64
                                               {
                                                   Tag = _stream.ReadInt32Network (),
                                                   Position = _stream.ReadInt32Network (),
                                                   Length = _stream.ReadInt32Network ()
                                               };
                dictionary.Add ( entry );
            }

            foreach ( MstDictionaryEntry64 entry in dictionary )
            {
                long endOffset = offset + leader.Base + entry.Position;
                _stream.Seek ( endOffset, SeekOrigin.Begin );
                entry.Bytes = _stream.ReadBytes ( entry.Length );
                if ( entry.Bytes != null )
                {
                    entry.Text = encoding.GetString ( entry.Bytes );
                }
            }

            MstRecord64 result = new MstRecord64
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
            target.Write(buffer,0,readed);
            target.Position = savedPosition;
            //return true;
        }

        public MstRecord64 ReadRecord2
            (
                long offset
            )
        {
            if (_stream.Seek(offset, SeekOrigin.Begin) != offset)
            {
                throw new IOException();
            }

            Encoding encoding = new UTF8Encoding(false, true);

            MemoryStream memory = new MemoryStream(PreloadLength);
            _AppendStream(_stream, memory, PreloadLength);
            memory.Position = 0;

            MstRecordLeader64 leader = MstRecordLeader64.Read(memory);
            int amountToRead = (int) (leader.Length - memory.Length);
            if (amountToRead > 0)
            {
                _AppendStream(_stream, memory, amountToRead);
            }

            List<MstDictionaryEntry64> dictionary
                = new List<MstDictionaryEntry64>();

            for (int i = 0; i < leader.Nvf; i++)
            {
                MstDictionaryEntry64 entry = new MstDictionaryEntry64
                {
                    Tag = memory.ReadInt32Network(),
                    Position = memory.ReadInt32Network(),
                    Length = memory.ReadInt32Network()
                };
                dictionary.Add(entry);
            }

            foreach (MstDictionaryEntry64 entry in dictionary)
            {
                long endOffset = leader.Base + entry.Position;
                memory.Seek(endOffset, SeekOrigin.Begin);
                entry.Bytes = memory.ReadBytes(entry.Length);
                if (entry.Bytes != null)
                {
                    entry.Text = encoding.GetString(entry.Bytes);
                }
            }

            MstRecord64 result = new MstRecord64
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
            byte[] buffer = new byte[4];

            _stream.Position = MstControlRecord64.LockFlagPosition;
            if (flag)
            {
                _stream.Lock(0, MstControlRecord64.RecordSize);
                buffer[0] = 1;
                _stream.Write(buffer, 0, buffer.Length);
            }
            else
            {
                _stream.Write(buffer, 0, buffer.Length);
                _stream.Unlock(0, MstControlRecord64.RecordSize);
            }
            _lockFlag = flag;
        }

        /// <summary>
        /// Чтение флага блокировки базы данных в целом.
        /// </summary>
        public bool ReadDatabaseLockedFlag()
        {
            byte[] buffer = new byte[4];

            _stream.Position = MstControlRecord64.LockFlagPosition;
            _stream.Read(buffer, 0, buffer.Length);
            return Convert.ToBoolean(BitConverter.ToInt32(buffer, 0));
        }

        #endregion

        #region IDisposable members

        public void Dispose ()
        {
            if ( _stream != null )
            {
                if (_lockFlag)
                {
                    LockDatabase(false);
                }

                _stream.Dispose ();
            }
        }

        #endregion

        #region Object members

        #endregion
    }
}
