/* IrbisDirectReader32.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

#endregion

namespace ManagedClient.Direct
{
    public sealed class IrbisDirectReader32
        : IDisposable
    {
        #region Properties

        public MstFile32 Mst { get; private set; }

        public XrfFile32 Xrf { get; private set; }

        public InvertedFile32 InvertedFile { get; private set; }

        public string Database { get; private set; }

        #endregion

        #region Construction

        public IrbisDirectReader32
            (
                string masterFile
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

        public int GetMaxMfn()
        {
            return Mst.ControlRecord.NextMfn - 1;
        }

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
            MstRecord32 mstRecord = Mst.ReadRecord2(xrfRecord.AbsoluteOffset);
            IrbisRecord result = mstRecord.DecodeRecord();
            result.Database = Database;
            return result;
        }

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

        public int[] SearchSimple(string key)
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

        public IrbisRecord[] SearchReadSimple(string key)
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

        public void Dispose()
        {
            if (Mst != null)
            {
                Mst.Dispose();
                Mst = null;
            }
            if (Xrf != null)
            {
                Xrf.Dispose();
                Xrf = null;
            }
            if (InvertedFile != null)
            {
                InvertedFile.Dispose();
                InvertedFile = null;
            }
        }

        #endregion
    }
}
