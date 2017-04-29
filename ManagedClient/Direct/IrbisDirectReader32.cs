// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisDirectReader32.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using JetBrains.Annotations;

#endregion

namespace ManagedClient.Direct
{
    /// <summary>
    /// Direct access to IRBIS32 databases.
    /// </summary>
    public sealed class IrbisDirectReader32
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// MST file.
        /// </summary>
        [NotNull]
        public MstFile32 Mst { get; private set; }

        /// <summary>
        /// XRF file.
        /// </summary>
        [NotNull]
        public XrfFile32 Xrf { get; private set; }

        /// <summary>
        /// Inverted file.
        /// </summary>
        [NotNull]
        public InvertedFile32 InvertedFile { get; private set; }

        /// <summary>
        /// Database name.
        /// </summary>
        public string Database { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisDirectReader32
            (
                [NotNull] string masterFile
            )
        {
            Database = Path.GetFileNameWithoutExtension(masterFile);
            Mst = new MstFile32
                (
                    Path.ChangeExtension
                        (
                            masterFile,
                            ".mst"
                        )
                );
            Xrf = new XrfFile32
                (
                    Path.ChangeExtension
                    (
                        masterFile,
                        ".xrf"
                    )
                );
            InvertedFile = new InvertedFile32
                (
                    Path.ChangeExtension
                    (
                        masterFile,
                        ".ifp"
                    )
                );
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Get maximal MFN.
        /// </summary>
        /// <returns></returns>
        public int GetMaxMfn()
        {
            return Mst.ControlRecord.NextMfn - 1;
        }

        /// <summary>
        /// Read the record.
        /// </summary>
        [CanBeNull]
        public IrbisRecord ReadRecord
            (
                int mfn
            )
        {
            XrfRecord32 xrfRecord = Xrf.ReadRecord(mfn);
            if ((xrfRecord.Status & RecordStatus.PhysicallyDeleted) != 0)
            {
                return null;
            }

            MstRecord32 mstRecord = Mst.ReadRecord2
                (
                    xrfRecord.AbsoluteOffset
                );
            IrbisRecord result = mstRecord.DecodeRecord();
            result.Database = Database;

            return result;
        }

        /// <summary>
        /// Read all versions for the record.
        /// </summary>
        [NotNull]
        public IrbisRecord[] ReadAllRecordVersions
            (
                int mfn
            )
        {
            List<IrbisRecord> result = new List<IrbisRecord>();
            IrbisRecord lastVersion = ReadRecord(mfn);
            if (lastVersion != null)
            {
                result.Add(lastVersion);
                while (true)
                {
                    long offset = lastVersion.PreviousOffset;
                    if (offset == 0)
                    {
                        break;
                    }
                    MstRecord32 mstRecord = Mst.ReadRecord2(offset);
                    IrbisRecord previousVersion = mstRecord.DecodeRecord();
                    if (previousVersion != null)
                    {
                        result.Add(previousVersion);
                        lastVersion = previousVersion;
                    }
                }
            }

            return result.ToArray();
        }

        //public IrbisRecord ReadRecord2
        //    (
        //        int mfn
        //    )
        //{
        //    XrfRecord64 xrfRecord = Xrf.ReadRecord(mfn);
        //    MstRecord32 mstRecord = Mst.ReadRecord2(xrfRecord.Offset);
        //    IrbisRecord result = mstRecord.DecodeRecord();
        //    result.Database = Database;
        //    return result;
        //}

        /// <summary>
        /// Simple search.
        /// </summary>
        public int[] SearchSimple
            (
                string key
            )
        {
            int[] mfns = InvertedFile.SearchSimple(key);
            List<int> result = new List<int>();
            foreach (int mfn in mfns)
            {
                if (!Xrf.ReadRecord(mfn).Deleted)
                {
                    result.Add(mfn);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Search and read found records.
        /// </summary>
        public IrbisRecord[] SearchReadSimple
            (
                string key
            )
        {
            int[] mfns = InvertedFile.SearchSimple(key);
            List<IrbisRecord> result = new List<IrbisRecord>();
            foreach (int mfn in mfns)
            {
                try
                {
                    XrfRecord32 xrfRecord = Xrf.ReadRecord(mfn);
                    if (!xrfRecord.Deleted)
                    {
                        MstRecord32 mstRecord = Mst.ReadRecord2(xrfRecord.AbsoluteOffset);
                        if (!mstRecord.Deleted)
                        {
                            IrbisRecord irbisRecord = mstRecord.DecodeRecord();
                            irbisRecord.Database = Database;
                            result.Add(irbisRecord);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            return result.ToArray();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Mst.Dispose();
            Xrf.Dispose();
            InvertedFile.Dispose();
        }

        #endregion
    }
}
