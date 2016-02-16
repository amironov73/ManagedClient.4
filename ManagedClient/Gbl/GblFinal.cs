/* GblFinal.cs
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

namespace ManagedClient.Gbl
{
    /// <summary>
    /// Результат выполнения глобальной корректировки.
    /// </summary>
    [Serializable]
    public sealed class GblFinal
    {
        #region Properties

        /// <summary>
        /// Момент начала обработки.
        /// </summary>
        public DateTime TimeStarted { get; set; }

        /// <summary>
        /// Всего времени затрачено (с момента начала обработки).
        /// </summary>
        public TimeSpan TimeElapsed { get; set; }

        /// <summary>
        /// Отменено пользователем.
        /// </summary>
        public bool Canceled { get; set; }

        /// <summary>
        /// Исключение (если возникло).
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Предполагалось обработать записей.
        /// </summary>
        public int RecordsSupposed { get; set; }

        /// <summary>
        /// Обработано записей.
        /// </summary>
        public int RecordsProcessed { get; set; }

        /// <summary>
        /// Успешно обработано записей.
        /// </summary>
        public int RecordsSucceeded { get; set; }

        /// <summary>
        /// Ошибок при обработке записей.
        /// </summary>
        public int RecordsFailed { get; set; }

        /// <summary>
        /// Результаты для каждой записи.
        /// </summary>
        public List<GblResult> Results { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format
                (
                    "RecordsProcessed: {0}, Canceled: {1}", 
                    RecordsProcessed, 
                    Canceled
                );
        }

        #endregion
    }
}
