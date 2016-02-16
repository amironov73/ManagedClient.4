/* PftConditionNot.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Условие НЕ
    /// </summary>
    [Serializable]
    public sealed class PftConditionNot
        : PftCondition
    {
        #region Properties

        public PftCondition Inner { get; set; }

        #endregion

        #region Construction

        public PftConditionNot()
        {
        }

        public PftConditionNot(PftParser.ConditionNotContext node)
            : base(node)
        {
            Inner = DispatchContext(node.condition());
            Children.Add(Inner);
        }

        #endregion

        #region PftCondition members

        public override bool Evaluate
            (
                PftContext context
            )
        {
            return !Inner.Evaluate(context);
        }

        #endregion
    }
}