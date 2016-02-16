/* IrbisTextHighlighter.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Highlight
{
    public abstract class IrbisTextHighlighter
    {
        #region Properties
        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public abstract void PrepareText(string text);

        public abstract void HighlightText(string keyword);

        public abstract string GetResultText();

        #endregion
    }
}
