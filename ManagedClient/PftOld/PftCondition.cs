/* PftCondition.cs
 */

#region

using System;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Условие в условном операторе.
    /// Абстрактный класс, реальные условия
    /// см. в потомках.
    /// </summary>
    [Serializable]
    public abstract class PftCondition
        : PftAst
    {
        #region Properties

        #endregion

        #region Construction

        internal PftCondition()
        {           
        }

        internal PftCondition(PftParser.ConditionContext node)
            : base(node)
        {
        }

        #endregion

        #region PftAst members

        #endregion

        #region Public methods

        public static PftCondition DispatchContext
            (
                PftParser.ConditionContext condition
            )
        {
            // ReSharper disable CanBeReplacedWithTryCastAndCheckForNull

            if (condition is PftParser.ConditionAndOrContext)
            {
                return new PftConditionAndOr((PftParser.ConditionAndOrContext)condition);
            }
            if (condition is PftParser.ConditionNotContext)
            {
                return new PftConditionNot((PftParser.ConditionNotContext)condition);
            }
            if (condition is PftParser.ConditionParenContext)
            {
                return new PftConditionParen((PftParser.ConditionParenContext)condition);
            }
            if (condition is PftParser.ConditionStringContext)
            {
                return new PftConditionString((PftParser.ConditionStringContext)condition);
            }
            if (condition is PftParser.ConditionArithContext)
            {
                return new PftArithCondition((PftParser.ConditionArithContext)condition);
            }
            if (condition is PftParser.ConditionFieldContext)
            {
                return new PftFieldPresence((PftParser.ConditionFieldContext)condition);
            }
            throw new ArgumentException();

            // ReSharper restore CanBeReplacedWithTryCastAndCheckForNull
        }

        /// <summary>
        /// Вычисление истинности условия.
        /// Реализации см. в потомках.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract bool Evaluate
            (
                PftContext context
            );

        #endregion
    }
}