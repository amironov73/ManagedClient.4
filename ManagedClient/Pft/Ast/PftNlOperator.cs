/* PftNlOperator.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft.Ast
{
    /// <summary>
    /// Перевод строки, специфичный для платформы.
    /// </summary>
    [Serializable]
    public sealed class PftNlOperator
        : PftAst
    {
        #region Properties

        #endregion

        #region Construction

        public PftNlOperator()
        {
        }

        // ReSharper disable UnusedParameter.Local
        public PftNlOperator
            (
                PftParser.NlOperatorContext context
            )
        {
            // Nothing to do
        }
        // ReSharper restore UnusedParameter.Local

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region PftAst members

        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            context.Write
                (
                    this,
                    Environment.NewLine
                );

            OnAfterExecution(context);
        }

        #endregion
    }
}
