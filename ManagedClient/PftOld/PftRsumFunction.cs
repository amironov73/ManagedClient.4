/* PftRsumFunction.cs
 */

#region Using directives

using System;
using System.Linq;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Возвращает сумму одного или более числовых значений. 
    /// Сначала система вычисляет аргумент, представленный форматом, 
    /// чтобы получить строку текста. Затем эта строка просматривается 
    /// слева направо, как и в функции VAL, и все содержащиеся 
    /// в ней числовые значения складываются. Полученная сумма является 
    /// значением функции. Отдельные числовые значения должны быть разделены, 
    /// по крайней мере, одним нечисловым символом, поэтому надо предусмотреть, 
    /// чтобы такое разделение в формате присутствовало. Функция RSUM может 
    /// использоваться для вычисления суммы всех числовых значений, 
    /// содержащихся во всех экземплярах данного повторяющегося поля.
    /// </summary>
    [Serializable]
    public sealed class PftRsumFunction
        : PftNumber
    {
        #region Properties

        public PftAst Argument { get; set; }

        #endregion

        #region Construction

        public PftRsumFunction()
        {
        }

        public PftRsumFunction(PftParser.RsumFunctionContext node) 
            : base(node)
        {
            Argument = PftNonGrouped.DispatchContext(node.nonGrouped());
            Children.Add(Argument);
        }

        #endregion

        #region PftAst members

        public override void Execute
            (
                PftContext context
            )
        {
            string text = context.Evaluate(Argument);
            double[] values = ExtractNumbers(text);
            Value = values.Sum();
        }

        #endregion
    }
}
