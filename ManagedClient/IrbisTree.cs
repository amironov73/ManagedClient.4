/* IrbisTree.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Дерево (иерархический справочник).
    /// </summary>
    [Serializable]
    public sealed class IrbisTree
    {
        #region Properties

        /// <summary>
        /// Имя (чисто для идентификации).
        /// </summary>
        public string Name { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public static IrbisTree ParseLines
            (
                string[] lines
            )
        {
            return null;
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            return string.Format
                (
                    "Name: {0}", 
                    Name
                );
        }

        #endregion
    }
}
