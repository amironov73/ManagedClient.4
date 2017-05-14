// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AsyncExtensions.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Threading.Tasks;

using JetBrains.Annotations;

#endregion

// ReSharper disable ConvertClosureToMethodGroup

namespace ManagedClient
{
    /// <summary>
    /// Extension for async/Tasks.
    /// </summary>
    public static class AsyncExtensions
    {
        /// <summary>
        /// ConfigureAwait(false).
        /// </summary>
        [NotNull]
        public static Task SafeConfigure
            (
                [NotNull] this Task task
            )
        {
#if (FW45 || FW46) && !FW4 && !FW35

            task.ConfigureAwait(false);

#endif

            return task;
        }

        /// <summary>
        /// ConfigureAwait(false).
        /// </summary>
        [NotNull]
        public static Task<T> SafeConfigure<T>
        (
            [NotNull] this Task<T> task
        )
        {
#if (FW45 || FW46) && !FW4 && !FW35

            task.ConfigureAwait(false);

#endif

            return task;
        }


        /// <summary>
        /// Connect.
        /// </summary>
        public static Task ConnectAsync
            (
                [NotNull] this ManagedClient64 client
            )
        {
            return Task.Factory.StartNew(client.Connect)
                .SafeConfigure();
        }

        /// <summary>
        /// Disconnect.
        /// </summary>
        public static Task DisconnectAsync
            (
                [NotNull] this ManagedClient64 client
            )
        {
            return Task.Factory.StartNew(client.Dispose)
                .SafeConfigure();
        }

        /// <summary>
        /// Format record.
        /// </summary>
        public static Task<string> FormatRecordAsync
            (
                [NotNull] this ManagedClient64 client,
                [NotNull] string format,
                int mfn
            )
        {
            return Task.Factory.StartNew
                (
                    () => client.FormatRecord(format, mfn)
                )
                .SafeConfigure();
        }

        /// <summary>
        /// Format record.
        /// </summary>
        public static Task<string> FormatRecordAsync
            (
                [NotNull] this ManagedClient64 client,
                [NotNull] string format,
                [NotNull] IrbisRecord record
            )
        {
            return Task.Factory.StartNew
                (
                    () => client.FormatRecord(format, record)
                )
                .SafeConfigure();
        }

        /// <summary>
        /// Get max MFN.
        /// </summary>
        public static Task<int> GetMaxMfnAsync
            (
                [NotNull] this ManagedClient64 client
            )
        {
            return Task.Factory.StartNew
                (
                    () => client.GetMaxMfn()
                )
                .SafeConfigure();
        }

        /// <summary>
        /// List available databases.
        /// </summary>
        public static Task<IrbisDatabaseInfo[]> ListDatabasesAsync
            (
                [NotNull] this ManagedClient64 client,
                [NotNull] string menuName
            )
        {
            return Task.Factory.StartNew
                (
                    () => client.ListDatabases(menuName)
                )
                .SafeConfigure();
        }

        /// <summary>
        /// List terms.
        /// </summary>
        public static Task<SearchTermInfo[]> ListTermsAsync
            (
                [NotNull] this ManagedClient64 client,
                [NotNull] string start,
                int count
            )
        {
            return Task.Factory.StartNew
                (
                    () => client.GetSearchTerms(start, count)
                )
                .SafeConfigure();
        }

        /// <summary>
        /// Load menu.
        /// </summary>
        public static Task<IrbisMenu> LoadMenuAsync
            (
                [NotNull] this ManagedClient64 client,
                [NotNull] string menuName
            )
        {
            return Task.Factory.StartNew
                (
                    () => IrbisMenu.Read(client, menuName)
                )
                .SafeConfigure();
        }

        /// <summary>
        /// Load INI-file.
        /// </summary>
        public static Task<IniFile> LoadIniFileAsync
            (
                [NotNull] this ManagedClient64 client,
                [NotNull] string fileName
            )
        {
            return Task.Factory.StartNew
                (
                    () =>
                    {
                        string text = client.ReadTextFile(fileName);
                        IniFile result = IniFile.ParseText<IniFile>(text);

                        return result;
                    }
                )
                .SafeConfigure();
        }

        /// <summary>
        /// No operation.
        /// </summary>
        public static Task NoOpAsync
            (
                [NotNull] this ManagedClient64 client
            )
        {
            return Task.Factory.StartNew(client.NoOp)
                .SafeConfigure();
        }

        /// <summary>
        /// Read record.
        /// </summary>
        public static Task<IrbisRecord> ReadRecordAsync
            (
                [NotNull] this ManagedClient64 client,
                int mfn
            )
        {
            return Task.Factory.StartNew
                (
                    () => client.ReadRecord(mfn)
                )
                .SafeConfigure();
        }

        /// <summary>
        /// Read text file.
        /// </summary>
        public static Task<string> ReadTextFileAsync
            (
                [NotNull] this ManagedClient64 client,
                [NotNull] string fileName
            )
        {
            return Task.Factory.StartNew
                (
                    () => client.ReadTextFile(fileName)
                )
                .SafeConfigure();
        }

        /// <summary>
        /// Search.
        /// </summary>
        public static Task<int[]> SearchAsync
            (
                [NotNull] this ManagedClient64 client,
                [NotNull] string expression
            )
        {
            return Task.Factory.StartNew
                (
                    () => client.Search(expression)
                )
                .SafeConfigure();
        }

        /// <summary>
        /// Write the record.
        /// </summary>
        public static Task WriteRecordAsync
            (
                [NotNull] this ManagedClient64 client,
                [NotNull] IrbisRecord record
            )
        {
            return Task.Factory.StartNew
                (
                    () => client.WriteRecord(record, false, true)
                )
                .SafeConfigure();
        }
    }
}
