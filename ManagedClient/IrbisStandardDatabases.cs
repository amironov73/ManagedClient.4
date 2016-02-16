/* IrbisStandardDatabases.cs
 */

namespace ManagedClient
{
    /// <summary>
    /// Стандартные базы данных, входящие в дистрибутив ИРБИС64.
    /// </summary>
    public static class IrbisStandardDatabases
    {
        #region Constants

        /// <summary>
        /// Электронный каталог.
        /// </summary>
        public const string ElectronicCatalog = "IBIS";

        /// <summary>
        /// Комплектование.
        /// </summary>
        public const string Acquisition = "CMPL";

        /// <summary>
        /// Читатели.
        /// </summary>
        public const string Readers = "RDR";

        /// <summary>
        /// Заказы на литературу.
        /// </summary>
        public const string Requests = "RQST";

        #endregion
    }
}
