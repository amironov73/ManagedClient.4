/* ReaderUtility.cs -- методы для работы с БД читателей.
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace ManagedClient.Readers
{
    /// <summary>
    /// Методы для работы с БД читателей.
    /// </summary>
    public static class ReaderUtility
    {
        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Загрузка читателей из базы.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="readers"></param>
        /// <param name="dbName"></param>
        public static List<ReaderInfo> LoadReaders
            (
                ManagedClient64 client,
                List<ReaderInfo> readers,
                string dbName
            )
        {
            try
            {
                client.PushDatabase(dbName);
                readers.Capacity += client.GetMaxMfn();

                BatchRecordReader batch = new BatchRecordReader
                    (
                        client,
                        1500
                    );

                Parallel.ForEach
                    (
                        batch,
                        record =>
                        {
                            if (!record.Deleted)
                            {
                                ReaderInfo reader = ReaderInfo.Parse(record);
                                if (reader != null)
                                {
                                    lock (readers)
                                    {
                                        readers.Add(reader);
                                    }
                                }
                            }
                        }
                    );
            }
            finally
            {
                client.PopDatabase();
            }

            return readers;
        }

        public static List<ReaderInfo> MergeReaders
            (
                List<ReaderInfo> readers
            )
        {
            var grouped = readers
                .Where(r => !string.IsNullOrEmpty(r.Ticket))
                .GroupBy(r => r.Ticket);

            List<ReaderInfo> result = new List<ReaderInfo>(readers.Count);

            foreach (var grp in grouped)
            {
                ReaderInfo first = grp.First();
                first.Visits = grp
                    .SelectMany(r => r.Visits)
                    .ToArray();
                result.Add(first);
            }

            return result;
        }

        public static List<ReaderInfo> LoadReaders
            (
                ManagedClient64 client,
                string[] databases
            )
        {
            List<ReaderInfo> result = new List<ReaderInfo>();

            foreach (string database in databases)
            {
                LoadReaders
                    (
                        client,
                        result,
                        database
                    );
            }
            if (databases.Length > 1)
            {
                result = MergeReaders(result);
            }

            return result;
        }

        public static int CountEvents
            (
                List<ReaderInfo> readers,
                DateTime fromDay,
                DateTime toDay,
                bool visit
            )
        {
            string fromDayString = new IrbisDate(fromDay).AsString;
            string toDayString = new IrbisDate(toDay).AsString;
            int result = readers
                .SelectMany(r => r.Visits)
                .Count(v => (v.DateGivenString.SafeCompare(fromDayString) >= 0)
                    && (v.DateGivenString.SafeCompare(toDayString) <= 0)
                    && (v.IsVisit == visit));
            return result;
        }

        public static int CountEvents
            (
                List<ReaderInfo> readers,
                DateTime fromDay,
                DateTime toDay,
                string department,
                bool visit
            )
        {
            string fromDayString = new IrbisDate(fromDay).AsString;
            string toDayString = new IrbisDate(toDay).AsString;
            int result = readers
                .SelectMany(r => r.Visits)
                .Count(v => (v.DateGivenString.SafeCompare(fromDayString) >= 0)
                    && (v.DateGivenString.SafeCompare(toDayString) <= 0)
                    && v.Department.SameString(department)
                    && (v.IsVisit == visit));
            return result;
        }

        public static VisitInfo[] GetEvents
            (
                this List<ReaderInfo> readers
            )
        {
            return readers
                .SelectMany(r => r.Visits)
                .ToArray();
        }

        public static VisitInfo[] GetEvents
            (
                this VisitInfo[] events,
                string department
            )
        {
            return events
                .AsParallel()
                .Where(v => v.Department.SameString(department))
                .ToArray();
        }

        public static VisitInfo[] GetEvents
            (
                this VisitInfo[] events,
                bool visit
            )
        {
            return events
                .AsParallel()
                .Where(v => v.IsVisit == visit)
                .ToArray();
        }

        public static VisitInfo[] GetEvents
            (
                this VisitInfo[] events,
                DateTime day
            )
        {
            string dayString = day.ToString("yyyyMMdd");
            VisitInfo[] result = events
                .AsParallel()
                .Where(v => v.DateGivenString.SameString(dayString))
                .ToArray();
            return result;
        }

        public static VisitInfo[] GetEvents
            (
                this VisitInfo[] events,
                DateTime fromDay,
                DateTime toDay
            )
        {
            string fromDayString = new IrbisDate(fromDay).AsString;
            string toDayString = new IrbisDate(toDay).AsString;
            VisitInfo[] result = events
                .AsParallel()
                .Where(v => (v.DateGivenString.SafeCompare(fromDayString) >= 0)
                            && (v.DateGivenString.SafeCompare(toDayString) <= 0))
                .ToArray();
            return result;
        }

        #endregion
    }
}
