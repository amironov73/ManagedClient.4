/* DatabaseChangedEventArgs.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient
{
    [Serializable]
    public sealed class DatabaseChangedEventArgs
        : EventArgs
    {
        #region Properties

        /// <summary>
        /// Старая база.
        /// </summary>
        public string OldDatabase { get; set; }

        /// <summary>
        /// Новая база
        /// </summary>
        public string NewDatabase { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public DatabaseChangedEventArgs()
        {
        }

        /// <summary>
        /// Удобный конструктор.
        /// </summary>
        /// <param name="oldDatabase">Старая база.</param>
        /// <param name="newDatabase">Новая база.</param>
        public DatabaseChangedEventArgs
            (
                string oldDatabase, 
                string newDatabase
            )
        {
            OldDatabase = oldDatabase;
            NewDatabase = newDatabase;
        }

        #endregion
    }
}
