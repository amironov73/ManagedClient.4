/* PftRightHand.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Pft.Ast
{
    [Serializable]
    public sealed class PftRightHand
        : PftAst
    {
        #region Properties

        #endregion

        #region Construction

        public PftRightHand()
        {
        }

        public PftRightHand
            (
                PftParser.RightHandContext context
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
