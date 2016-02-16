/* PftGroupStatement.cs
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
    /// Простая группа элементов формата (в круглых скобках).
    /// </summary>
    [Serializable]
    public sealed class PftGroupStatement
        : PftAst
    {
        #region Properties

        #endregion

        #region Construction

        public PftGroupStatement()
        {
        }

        public PftGroupStatement
            (
                PftParser.GroupStatementContext context
            )
        {
            DiscoverChildren(context);
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region PftAst members

        #endregion
    }
}
