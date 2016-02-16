/* IrbisTransactionItem.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Transactions
{
    /// <summary>
    /// Данные об элементе транзакции.
    /// </summary>
    [Serializable]
    public sealed class IrbisTransactionItem
    {
        #region Properties

        /// <summary>
        /// Момент времени.
        /// </summary>
        public DateTime Moment { get; set; }

        /// <summary>
        /// Произведенное действие: создание записи,
        /// модификация, удаление.
        /// </summary>
        public IrbisTransactionAction Action { get; set; }

        /// <summary>
        /// Имя базы данных, в которой происходило действие.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// MFN записи, с которой происходило действие.
        /// </summary>
        public int Mfn { get; set; }

        #endregion
    }
}
