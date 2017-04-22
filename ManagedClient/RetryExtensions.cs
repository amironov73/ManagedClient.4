// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RetryExtensions.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Threading;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Retry command specified number of times.
    /// </summary>
    public static class RetryExtensions
    {
        /// <summary>
        /// Number of retry attempts.
        /// </summary>
        public static int RetryCount = 5;

        /// <summary>
        /// Delay between attempts, milliseconds.
        /// </summary>
        public static int DelayInterval = 300;

        private static void _HandleException(int attempt,
            Exception exception)
        {
            // Log exception here
            Debug.WriteLine("Attempt " + attempt + ", Exception "
                            + exception.Message);

            Thread.Sleep(DelayInterval);
        }

        /// <summary>
        /// Try the action.
        /// </summary>
        public static void Try
            (
                Action action
            )
        {
            for (int attempt = 0; attempt < RetryCount; attempt++)
            {
                try
                {
                    action();
                    return;
                }
                catch (Exception ex)
                {
                    _HandleException(attempt, ex);
                }
            }

            throw new Exception("All attempts failed");
        }

        /// <summary>
        /// Try the action.
        /// </summary>
        public static void Try<T1>
            (
                Action<T1> action,
                T1 argument1
            )
        {
            for (int attempt = 0; attempt < RetryCount; attempt++)
            {
                try
                {
                    action(argument1);
                    return;
                }
                catch (Exception ex)
                {
                    _HandleException(attempt, ex);
                }
            }

            throw new Exception("All attempts failed");
        }

        /// <summary>
        /// Try the action.
        /// </summary>
        public static void Try<T1, T2>
            (
                Action<T1, T2> action,
                T1 argument1,
                T2 argument2
            )
        {
            for (int attempt = 0; attempt < RetryCount; attempt++)
            {
                try
                {
                    action(argument1, argument2);
                    return;
                }
                catch (Exception ex)
                {
                    _HandleException(attempt, ex);
                }
            }

            throw new Exception("All attempts failed");
        }

        /// <summary>
        /// Try the action.
        /// </summary>
        public static void Try<T1, T2, T3>
            (
                Action<T1, T2, T3> action,
                T1 argument1,
                T2 argument2,
                T3 argument3
            )
        {
            for (int attempt = 0; attempt < RetryCount; attempt++)
            {
                try
                {
                    action(argument1, argument2, argument3);
                    return;
                }
                catch (Exception ex)
                {
                    _HandleException(attempt, ex);
                }
            }

            throw new Exception("All attempts failed");
        }

        /// <summary>
        /// Try the function.
        /// </summary>
        public static TResult Try<TResult>
            (
                Func<TResult> func
            )
        {
            for (int attempt = 0; attempt < RetryCount; attempt++)
            {
                try
                {
                    TResult result = func();
                    return result;
                }
                catch (Exception ex)
                {
                    _HandleException(attempt, ex);
                }
            }

            throw new Exception("All attempts failed");
        }

        /// <summary>
        /// Try the function.
        /// </summary>
        public static TResult Try<TResult, T1>
            (
                Func<T1, TResult> func,
                T1 argument1
            )
        {
            for (int attempt = 0; attempt < RetryCount; attempt++)
            {
                try
                {
                    TResult result = func(argument1);
                    return result;
                }
                catch (Exception ex)
                {
                    _HandleException(attempt, ex);
                }
            }

            throw new Exception("All attempts failed");
        }

        /// <summary>
        /// Try the function.
        /// </summary>
        public static TResult Try<TResult, T1, T2>
            (
                Func<T1, T2, TResult> func,
                T1 argument1,
                T2 argument2
            )
        {
            for (int attempt = 0; attempt < RetryCount; attempt++)
            {
                try
                {
                    TResult result = func(argument1, argument2);
                    return result;
                }
                catch (Exception ex)
                {
                    _HandleException(attempt, ex);
                }
            }

            throw new Exception("All attempts failed");
        }

        /// <summary>
        /// Try the function.
        /// </summary>
        public static TResult Try<TResult, T1, T2, T3>
            (
                Func<T1, T2, T3, TResult> func,
                T1 argument1,
                T2 argument2,
                T3 argument3
            )
        {
            for (int attempt = 0; attempt < RetryCount; attempt++)
            {
                try
                {
                    TResult result = func
                        (
                            argument1,
                            argument2,
                            argument3
                        );

                    return result;
                }
                catch (Exception ex)
                {
                    _HandleException(attempt, ex);
                }
            }

            throw new Exception("All attempts failed");
        }

        /// <summary>
        /// Connect to the server.
        /// </summary>
        public static void ConnectRetry
            (
                this ManagedClient64 client
            )
        {
            Action action = client.Connect;
            Try(action);
        }

        /// <summary>
        /// Disconnect.
        /// </summary>
        public static void DisconnectRetry
            (
                this ManagedClient64 client
            )
        {
            Try(client.Dispose);
        }

        /// <summary>
        /// Format the record.
        /// </summary>
        public static string FormatRecordRetry
            (
                this ManagedClient64 client,
                string format,
                int mfn
            )
        {
            Func<string, int, string> func = client.FormatRecord;

            return Try(func, format, mfn);
        }

        /// <summary>
        /// Format the record.
        /// </summary>
        public static string FormatRecordRetry
            (
                this ManagedClient64 client,
                string format,
                IrbisRecord record
            )
        {
            Func<string, IrbisRecord, string> func = client.FormatRecord;

            return Try(func, format, record);
        }

        /// <summary>
        /// Get max MFN.
        /// </summary>
        public static int GetMaxMfnRetry
            (
                this ManagedClient64 client
            )
        {
            Func<int> func = client.GetMaxMfn;

            return Try(func);
        }

        /// <summary>
        /// List databases.
        /// </summary>
        public static IrbisDatabaseInfo[] ListDatabasesRetry
            (
                this ManagedClient64 client,
                string menuName
            )
        {
            Func<string, IrbisDatabaseInfo[]> func = client.ListDatabases;

            return Try(func, menuName);
        }

        /// <summary>
        /// List terms.
        /// </summary>
        public static SearchTermInfo[] ListTermsRetry
            (
                this ManagedClient64 client,
                string start,
                int count
            )
        {
            Func<string, int, SearchTermInfo[]> func = client.GetSearchTerms;

            return Try(func, start, count);
        }

        /// <summary>
        /// Load menu.
        /// </summary>
        public static IrbisMenu LoadMenuRetry
            (
                this ManagedClient64 client,
                string menuName
            )
        {
            Func<ManagedClient64, string, IrbisMenu> func = IrbisMenu.Read;

            return Try(func, client, menuName);
        }

        /// <summary>
        /// Load INI-file.
        /// </summary>
        public static IniFile LoadIniFileRetry
            (
                this ManagedClient64 client,
                string fileName
            )
        {
            Func<ManagedClient64, string, IniFile> func
                = (theClient, theFile) =>
            {
                string text = theClient.ReadTextFile(theFile);
                IniFile result = IniFile.ParseText<IniFile>(text);

                return result;
            };

            return Try(func, client, fileName);
        }

        /// <summary>
        /// No operation.
        /// </summary>
        public static void NopRetry
            (
                this ManagedClient64 client
            )
        {
            Try(client.NoOp);
        }

        /// <summary>
        /// Read record from server.
        /// </summary>
        public static IrbisRecord ReadRecordRetry
            (
                this ManagedClient64 client,
                int mfn
            )
        {
            Func<int, IrbisRecord> func = client.ReadRecord;

            return Try(func, mfn);
        }

        /// <summary>
        /// Read text file.
        /// </summary>
        public static string ReadTextFileRetry
            (
                this ManagedClient64 client,
                string specification
            )
        {
            Func<string, string> func = client.ReadTextFile;

            return Try(func, specification);
        }

        /// <summary>
        /// Search.
        /// </summary>
        public static int[] SearchRetry
            (
                this ManagedClient64 client,
                string expression
            )
        {
            Func<string, object[], int[]> func = client.Search;

            return Try(func, expression, new object[0]);
        }

        /// <summary>
        /// Write the record.
        /// </summary>
        public static void WriteRecordRetry
            (
                this ManagedClient64 client,
                IrbisRecord record
            )
        {
            Action<IrbisRecord, bool, bool> action = client.WriteRecord;
            Try(action, record, false, true);
        }
    }
}
