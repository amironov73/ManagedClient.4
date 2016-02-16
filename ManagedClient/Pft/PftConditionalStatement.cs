/* PftConditionalStatement.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedClient.Pft.Ast;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Условный оператор: if then else fi.
    /// </summary>
    [Serializable]
    public sealed class PftConditionalStatement
        : PftAst
    {
        #region Properties

        /// <summary>
        /// Собственно условие.
        /// </summary>
        public PftCondition Condition
        {
            get { return _condition; }
            set { _condition = ChangeChild(_condition, value); }
        }

        /// <summary>
        /// Ветка ТОГДА.
        /// </summary>
        public PftCompositeElementList ThenBranch { get { return _thenBranch; } }

        /// <summary>
        /// Ветка ИНАЧЕ.
        /// </summary>
        public PftCompositeElementList ElseBranch { get { return _elseBranch; } }

        #endregion

        #region Construction

        public PftConditionalStatement()
        {
            _thenBranch = new PftCompositeElementList(Children);
            _elseBranch = new PftCompositeElementList(Children);
        }

        public PftConditionalStatement
            (
                PftParser.ConditionalStatementContext context
            )
        {
            Condition = PftDispatcher.DispatchCondition(context.condition());
            foreach (PftParser.CompositeElementContext element 
                in context.thenBranch.compositeElement())
            {
                ThenBranch.Add((PftCompositeElement) PftDispatcher.DispatchFormat(element));
            }
            foreach (PftParser.CompositeElementContext element 
                in context.elseBranch.compositeElement())
            {
                ElseBranch.Add((PftCompositeElement) PftDispatcher.DispatchFormat(element));
            }
        }

        #endregion

        #region Private members

        private PftCondition _condition;
        private readonly PftCompositeElementList _thenBranch;
        private readonly PftCompositeElementList _elseBranch;

        #endregion

        #region Public methods

        #endregion

        #region PftAst members

        #endregion
    }
}
