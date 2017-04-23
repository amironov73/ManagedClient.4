// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DatabaseChangedEventArgs.cs
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Arguments for <see cref="ManagedClient64.DatabaseChanged"/>
    /// event.
    /// </summary>
    [Serializable]
    public sealed class DatabaseChangedEventArgs
        : EventArgs
    {
        #region Properties

        /// <summary>
        /// Old database name.
        /// </summary>
        [CanBeNull]
        public string OldDatabase { get; set; }

        /// <summary>
        /// New database name.
        /// </summary>
        [CanBeNull]
        public string NewDatabase { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DatabaseChangedEventArgs()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public DatabaseChangedEventArgs
            (
                [CanBeNull] string oldDatabase, 
                [CanBeNull] string newDatabase
            )
        {
            OldDatabase = oldDatabase;
            NewDatabase = newDatabase;
        }

        #endregion
    }
}
