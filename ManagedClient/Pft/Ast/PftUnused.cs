/* PftUnused.cs
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
    /// Пропускаемые элементы формата, например,
    /// лишние условные или повторяющиеся литералы.
    /// </summary>
    [Serializable]
    public sealed class PftUnused
        : PftAst
    {
        #region Properties

        #endregion

        #region Construciton

        public PftUnused()
        {
        }

        public PftUnused
            (
                PftParser.UnusedContext context
            )
        {
            DiscoverChildren(context);
        }

        #endregion

        #region Public methods

        #endregion

        #region PftAst members

        #endregion
    }
}
