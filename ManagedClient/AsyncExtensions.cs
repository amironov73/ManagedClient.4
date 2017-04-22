// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AsyncExtensions.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Threading.Tasks;

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
                this ManagedClient64 client
            )
        {
            return Task.Factory.StartNew(client.Connect);
        }

        /// <summary>
        /// Disconnect.
        /// </summary>
        public static Task DisconnectAsync
            (
                this ManagedClient64 client
            )
        {
            return Task.Factory.StartNew(client.Dispose);
        }

        /// <summary>
        /// Format record.
        /// </summary>
        public static Task<string> FormatRecordAsync
            (
                this ManagedClient64 client,
                string format,
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
                this ManagedClient64 client,
                string format,
                IrbisRecord record
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
                this ManagedClient64 client
            )
        {
            return Task.Factory.StartNew(client.GetMaxMfn);
        }

        /// <summary>
        /// List available databases.
        /// </summary>
        public static Task<IrbisDatabaseInfo[]> ListDatabasesAsync
            (
                this ManagedClient64 client,
                string menuName
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
                this ManagedClient64 client,
                string start,
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
                this ManagedClient64 client,
                string menuName
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
                this ManagedClient64 client,
                string fileName
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
        public static Task NopAsync
            (
                this ManagedClient64 client
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
                this ManagedClient64 client,
                string fileName
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
                this ManagedClient64 client,
                string expression
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
                this ManagedClient64 client,
                IrbisRecord record
            )
        {
            return Task.Factory.StartNew
                (
                    () => client.WriteRecord(record, false, true)
                );
        }
    }
}
