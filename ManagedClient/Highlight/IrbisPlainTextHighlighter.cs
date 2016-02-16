/* IrbisPlainTextHighlighter.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Highlight
{
    public sealed class IrbisPlainTextHighlighter
        : IrbisTextHighlighter
    {
        #region Private members

        private string _savedText;

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

            _savedText = text;
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

            // Nothing to do here
        }

        public override string GetResultText()
        {
            return _savedText;
        }

        #endregion
    }
}
