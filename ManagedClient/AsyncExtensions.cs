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

namespace ManagedClient
{
    /// <summary>
    /// Extension for async/Tasks.
    /// </summary>
    public static class AsyncExtensions
    {
        /// <summary>
        /// Connect.
        /// </summary>
        public static Task ConnectAsync
            (
                [NotNull] this ManagedClient64 client
            )
        {
            return Task.Factory.StartNew(client.Connect);
        }

        /// <summary>
        /// Disconnect.
        /// </summary>
        public static Task DisconnectAsync
            (
                [NotNull] this ManagedClient64 client
            )
        {
            return Task.Factory.StartNew(client.Dispose);
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
                );
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
                );
        }

        /// <summary>
        /// Get max MFN.
        /// </summary>
        public static Task<int> GetMaxMfnAsync
            (
                [NotNull] this ManagedClient64 client
            )
        {
            return Task.Factory.StartNew(client.GetMaxMfn);
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
                );
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
                );
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
                );
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
                );
        }

        /// <summary>
        /// No operation.
        /// </summary>
        public static Task NoOpAsync
            (
                [NotNull] this ManagedClient64 client
            )
        {
            return Task.Factory.StartNew(client.NoOp);
        }

        /// <summary>
        /// Read record.
        /// </summary>
        public static Task<IrbisRecord> ReadRecordAsync
            (
                this ManagedClient64 client,
                int mfn
            )
        {
            return Task.Factory.StartNew
                (
                    () => client.ReadRecord(mfn)
                );
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
                );
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
                );
        }

        /// <summary>
        /// Write the record.
        /// </summary>
        public static Task WriteRecordAsync
            (
                [NotNull]this ManagedClient64 client,
                [NotNull] IrbisRecord record
            )
        {
            return Task.Factory.StartNew
                (
                    () => client.WriteRecord(record, false, true)
                );
        }
    }
}
