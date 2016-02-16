/* PftBreakStatement.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Прерывание обработки повторяющейся группы
    /// </summary>
    [Serializable]
    public sealed class PftBreakStatement
        : PftGroupItem
    {
        #region Properties

        #endregion

        #region Construciton

        public PftBreakStatement()
        {
        }

        public PftBreakStatement(PftParser.BreakContext node) 
            : base(node)
        {
        }

        #endregion

        #region PftAst members

        public override void Execute
            (
                PftContext context
            )
        {
            if (Group != null)
            {
                Group.BreakEncountered = true;
            }
        }

        #endregion
    }
}
