/* PftSemanticException.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Antlr4.Runtime;

#endregion

namespace ManagedClient.Pft
{
    [Serializable]
    public sealed class PftSemanticException
        : PftException
    {
        #region Properties

        #endregion

        #region Construction

        public PftSemanticException()
        {
        }

        public PftSemanticException
            (
                string message
            )
            : base(message)
        {
        }

        public PftSemanticException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion

        #region Public methods

        #endregion
    }
}
