/* PftDebugEventArgs.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Pft.Debugging
{
    [Serializable]
    public sealed class PftDebugEventArgs
        : EventArgs
    {
        #region Properties

        public bool CancelExecution { get; set; }

        public PftContext Context { get; set; }

        public PftAst Node { get; set; }

        public PftVariable Variable { get; set; }

        #endregion

        #region Construction

        public PftDebugEventArgs()
        {
        }

        public PftDebugEventArgs
            (
                PftContext context,
                PftAst node
            )
        {
            Context = context;
            Node = node;
        }

        #endregion

        #region Public methods

        #endregion
    }
}
