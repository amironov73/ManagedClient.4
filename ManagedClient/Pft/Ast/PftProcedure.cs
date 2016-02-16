/* PftProcedure.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Pft.Ast
{
    using Debugging;

    [Serializable]
    public sealed class PftProcedure
        : PftAst
    {
        #region Properties

        public string Name { get; set; }

        public List<PftArgument> Arguments{ get { return _arguments; } }

        #endregion

        #region Construction

        public PftProcedure()
        {
            _arguments = new List<PftArgument>();
        }

        #endregion

        #region Private members

        private readonly List<PftArgument> _arguments;

        #endregion

        #region Public methods

        public void Call
            (
                PftContext context,
                List<PftAst> argValues
            )
        {            
            OnBeforeExecution(context);

            OnAfterExecution(context);
        }

        #endregion
    }
}
