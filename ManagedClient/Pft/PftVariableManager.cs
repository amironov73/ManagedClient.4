/* PftVariableManager.cs
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
    using Debugging;

    [Serializable]
    public sealed class PftVariableManager
    {
        #region Properties

        #endregion

        #region Construction

        public PftVariableManager()
        {
            _dictionary = new Dictionary<string, PftVariable>();
        }

        #endregion

        #region Private members

        private readonly Dictionary<string, PftVariable> _dictionary;

        #endregion

        #region Public methods

        public void Clear()
        {
            _dictionary.Clear();
        }

        public void CreateProcedure
            (
                string name,
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
