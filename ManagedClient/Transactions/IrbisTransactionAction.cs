/* IrbisTransactionAction.cs
 */

namespace ManagedClient.Transactions
{
    /// <summary>
    /// Отслеживаемое действие при транзакции.
    /// </summary>
    public enum IrbisTransactionAction
    {
        CreateRecord = (byte)'N',

        ModifyRecord = (byte)'W',

        DeleteRecord = (byte)'D'
    }
}
