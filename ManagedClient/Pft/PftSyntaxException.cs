/* PftSyntaxException.cs
 */

#region Using directives

using System;

using Antlr4.Runtime;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Исключение, возникающее при разборе PFT-скрипта.
    /// </summary>
    public sealed class PftSyntaxException
        : PftException
    {
        #region Properties

        #endregion

        #region Construction

        public PftSyntaxException()
        {
        }

        public PftSyntaxException
            (
                string message
            )
            : base(message)
        {
        }

        public PftSyntaxException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public PftSyntaxException
            (
                ParserRuleContext context
            )
        {
            // TODO показать контекст удобоваримым способом            
        }

        #endregion

        #region Public methods

        #endregion
    }
}
