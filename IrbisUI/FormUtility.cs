// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FormUtility.cs
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using ManagedClient.Output;

#endregion

namespace IrbisUI
{
    /// <summary>
    /// Utility for <see cref="Form"/>.
    /// </summary>
    public static class FormUtility
    {
        #region Public methods

        /// <summary>
        /// Show version information in form title.
        /// </summary>
        public static void ShowVersionInfoInTitle
            (
                this Form form
            )
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            Version vi = assembly.GetName().Version;
            FileVersionInfo fvi = FileVersionInfo
                .GetVersionInfo(assembly.Location);
            FileInfo fi = new FileInfo(assembly.Location);
            form.Text += string.Format
                (
                    ": version {0} (file {1}) from {2}",
                    vi,
                    fvi.FileVersion,
                    fi.LastWriteTime.ToShortDateString()
                );
        }

        /// <summary>
        /// Print system information in abstract output.
        /// </summary>
        public static void PrintSystemInformation
            (
                this AbstractOutput output
            )
        {
            output.WriteLine
                (
                    "OS version: {0}",
                    Environment.OSVersion
                );
            output.WriteLine
                (
                    "Framework version: {0}",
                    Environment.Version
                );
            Assembly assembly = Assembly.GetEntryAssembly();
            Version vi = assembly.GetName().Version;
            FileInfo fi = new FileInfo(assembly.Location);
            output.WriteLine
                (
                    "Application version: {0} ({1})",
                    vi,
                    fi.LastWriteTime.ToShortDateString()
                );
            output.WriteLine
                (
                    "Memory: {0} Mb",
                    GC.GetTotalMemory(false) / 1024
                );
        }

        #endregion
    }
}
