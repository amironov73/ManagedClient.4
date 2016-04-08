/* ParallelFormatter.cs -- форматирование записей в несколько потоков.
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Форматирование записей в несколько потоков.
    /// </summary>
    [PublicAPI]
    public class ParallelFormatter
        : IEnumerable<string>,
        IDisposable
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ParallelFormatter
            (
                int parallelism,
                string connectionString,
                int[] mfnList,
                string format
            )
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Форматирование всех записей.
        /// </summary>
        /// <returns></returns>
        public List<string> FormatAll()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable<T> members

        public IEnumerator<string> GetEnumerator()
        {
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IDisposable members

        public void Dispose()
        {

        }

        #endregion
    }
}
