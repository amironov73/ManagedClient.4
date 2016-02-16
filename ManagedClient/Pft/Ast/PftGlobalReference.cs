/* PftGlobalReference.cs
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
    /// Ссылка на глобальную переменную.
    /// </summary>
    [Serializable]
    public sealed class PftGlobalReference
        : PftGroupMember
    {
        #region Properties

        #endregion

        #region Construction

        public PftGlobalReference()
        {
        }

        public PftGlobalReference
            (
                PftParser.GlobalReferenceContext context
            )
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region PftAst members

        #endregion

        #region PftGroupMember members

        #endregion
    }
}
