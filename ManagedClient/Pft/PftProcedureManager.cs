/* PftProcedureManager.cs
 */

#region Using directives

using System;
using System.Collections.Generic;

using Antlr4.Runtime;

#endregion

namespace ManagedClient.Pft
{
    using Ast;

    [Serializable]
    public sealed class PftProcedureManager
    {
        #region Properties

        #endregion

        #region Construction

        public PftProcedureManager()
        {
            _dictionary = new Dictionary<string, PftProcedure>();
        }

        #endregion

        #region Private members

        private readonly Dictionary<string, PftProcedure> _dictionary;

        #endregion

        #region Public methods

        public void Clear()
        {
            _dictionary.Clear();
        }

        public void CreateProcedure
            (
                ParserRuleContext context
            )
        {
            
        }

        public void Delete
            (
                string name
            )
        {
            _dictionary.Remove(name);
        }

        public bool Have
            (
                string name
            )
        {
            return _dictionary.ContainsKey(name);
        }

        #endregion
    }
}
