/* IrbisHyperTextHighlighter.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Highlight
{
    public sealed class IrbisHyperTextHighlighter
        : IrbisTextHighlighter
    {
        #region Private members
        #endregion

        #region IrbisTextHighlighter members

        public override void PrepareText
            (
                string text
            )
        {
            if (ReferenceEquals(text, null))
            {
                throw new ArgumentNullException("text");
            }

            throw new NotImplementedException();
        }

        public override void HighlightText
            (
                string keyword
            )
        {
            if (ReferenceEquals(keyword, null))
            {
                throw new ArgumentNullException("keyword");
            }

            throw new NotImplementedException();
        }

        public override string GetResultText()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
