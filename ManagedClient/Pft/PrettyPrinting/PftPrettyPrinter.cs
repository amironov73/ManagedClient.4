/* PftPrettyPrinter.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Pft.PrettyPrinting
{
    /// <summary>
    /// Красивое переформатирование PFT-скриптов.
    /// </summary>
    public sealed class PftPrettyPrinter
    {
        #region Properties

        public PftStyle Style { get; set; }

        #endregion

        #region Construction

        public PftPrettyPrinter()
        {
        }

        public PftPrettyPrinter
            (
                PftStyle style
            )
        {
            Style = style;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public string PrettyPrint
            (
                PftAst program
            )
        {
            StringWriter writer = new StringWriter();

            return writer.ToString();
        }

        #endregion
    }
}
