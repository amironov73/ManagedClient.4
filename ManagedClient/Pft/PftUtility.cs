/* PftUtility.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Pft
{
    public static class PftUtility
    {
        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Расформатирует текст скрипта локально.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="formatSource"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        public static string FormatSource
            (
                ManagedClient64 client,
                string formatSource,
                IrbisRecord record
            )
        {
            using (PftFormatter formatter = new PftFormatter(client))
            {
                string input = formatter.ResolveInline(formatSource);
                string result = formatter
                    .ParseInput(input)
                    .Format(record);
                return result;
            }
        }

        /// <summary>
        /// Расформатирует скрипт из файла локально.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="formatName"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        public static string FormatLocalFile
            (
                ManagedClient64 client,
                string formatName,
                IrbisRecord record
            )
        {
            string formatSource = File.ReadAllText
                (
                    formatName,
                    Encoding.Default
                );
            return FormatSource
                (
                    client,
                    formatSource,
                    record
                );
        }

        /// <summary>
        /// Расформатирует скрипт, скачанный с сервера.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="formatName"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        public static string FormatRemoteFile
            (
                ManagedClient64 client,
                string formatName,
                IrbisRecord record
            )
        {
            string formatSource = client.ReadTextFile(formatName);
            if (string.IsNullOrEmpty(formatSource))
            {
                return string.Empty;
            }
            return FormatSource
                (
                    client,
                    formatSource,
                    record
                );
        }

        /// <summary>
        /// Убирает из литерала переводы строки
        /// и отрезает символы-ограничители.
        /// </summary>
        /// <param name="rawText"></param>
        /// <returns></returns>
        public static string PrepareLiteral
            (
                string rawText
            )
        {
            if (string.IsNullOrEmpty(rawText))
            {
                return string.Empty;
            }
            string result = rawText.Replace
                (
                    "\r",
                    string.Empty
                ).Replace
                (
                    "\n",
                    string.Empty
                );
            result = result.Substring
                (
                    1,
                    result.Length - 2
                );
            return result;
        }

        #endregion
    }
}
