/* PftRmaxFunction.cs
 */

#region Using directives

using System;
using System.Linq;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Возвращает максимальное значение одного или нескольких числовых значений. 
    /// Сначала система вычисляет аргумент, представленный форматом, чтобы получить 
    /// строку текста. Затем эта строка просматривается слева направо, как и в функции 
    /// VAL, и из нее извлекаются все числа. Алгебраически наибольшее из извлеченных 
    /// чисел и будет результатом функции. Отдельные числовые значения должны быть 
    /// разделены, по крайней мере, одним нечисловым символом, поэтому надо предусмотреть, 
    /// чтобы такое разделение в формате присутствовало. Функция RMAX может использоваться 
    /// для вычисления максимального значения среди всех числовых значений, содержащихся 
    /// во всех экземплярах данного повторяющегося поля.
    /// </summary>
    [Serializable]
    public sealed class PftRmaxFunction
        : PftNumber
    {
        #region Properties

        public PftAst Argument { get; set; }

        #endregion

        #region Construction

        public PftRmaxFunction()
        {
        }

        public PftRmaxFunction(PftParser.RmaxFunctionContext node)
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
            Value = values.Max();
        }

        #endregion
    }
}
