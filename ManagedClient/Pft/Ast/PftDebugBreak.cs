/* PftDebugBreak.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft.Ast
{
    /// <summary>
    /// Программная точка останова.
    /// </summary>
    [Serializable]
    public sealed class PftDebugBreak
        : PftAst
    {
        #region Properties

        #endregion

        #region Construction

        public PftDebugBreak()
        {
        }

        // ReSharper disable UnusedParameter.Local
        public PftDebugBreak
            (
                PftParser.DebugBreakContext context
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

        #endregion
    }
}
