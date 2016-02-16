/* PftBuffer.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class PftBuffer
    {
        #region Properties

        public PftBuffer Parent { get { return _parent; } }

        public AstTextCollection Normal { get { return _normal; } }

        public AstTextCollection Warning { get { return _warning; } }

        public AstTextCollection Error { get { return _error; } }

        #endregion

        #region Construction

        public PftBuffer
            (
                PftBuffer parent
            )
        {
            _parent = parent;

            _normal = new AstTextCollection();
            _warning = new AstTextCollection();
            _error = new AstTextCollection();
        }

        #endregion

        #region Private members

        private readonly PftBuffer _parent;

        private readonly AstTextCollection _normal;
        private readonly AstTextCollection _warning;
        private readonly AstTextCollection _error;

        #endregion

        #region Public methods

        #endregion
    }
}
