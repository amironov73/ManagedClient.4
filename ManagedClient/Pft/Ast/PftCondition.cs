/* PftCondition.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Pft.Ast
{
    /// <summary>
    /// Условие в условном операторе.
    /// Абстрактный класс, реальные условия
    /// см. в потомках.
    /// </summary>
    [Serializable]
    public abstract class PftCondition
        : PftAst
    {
        #region Properites

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Вычисление истинности условия.
        /// Реализации см. в потомках.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract bool Evaluate
            (
                PftContext context
            );

        #endregion

        #region PftAst members

        #endregion
    }
}
