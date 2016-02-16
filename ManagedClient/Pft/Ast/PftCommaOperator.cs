/* PftCommaOperator.cs
 */

#region Using directives

using System;
using System.IO;

#endregion

namespace ManagedClient.Pft.Ast
{
    /// <summary>
    /// Запятая.
    /// Пустой оператор. 
    /// Может быть безболезненно удалён в большинстве случаев.
    /// </summary>
    [Serializable]
    public sealed class PftCommaOperator
        : PftAst
    {
        #region Properties

        #endregion

        #region Construction

        public PftCommaOperator()
        {
        }

        public PftCommaOperator
            (
                PftParser.CommaOperatorContext context
            )
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region PftAst members

        public override void Write
            (
                StreamWriter writer
            )
        {
            // Добавляем пробел для читабельности
            writer.Write(", ");
        }

        #endregion
    }
}
