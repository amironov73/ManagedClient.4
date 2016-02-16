/* PftFFunction.cs
 */

#region Using directives

using System;
using System.Globalization;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Функция F преобразует числовое значение из его 
    /// внутреннего представления с плавающей точкой в символьную строку.
    /// Все три аргумента являются числовыми выражениями.
    /// Первый аргумент - выр-1, является числом, которое необходимо преобразовать.
    /// Второй аргумент  - выр-2, минимальная длина выходной строки, 
    /// выделяемая для результата,
    /// Третий аргумент  - выр-3, количество десятичных цифр.
    /// Второй и третий аргументы необязательны. Отметим, однако, 
    /// что если присутствует выр-3, то выр-2 не может быть опущено.
    /// </summary>
    [Serializable]
    public sealed class PftFFunction
        : PftAst
    {
        #region Properties

        /// <summary>
        /// Число, которое необходимо преобразовать.
        /// </summary>
        public PftNumber Argument1 { get; set; }

        /// <summary>
        /// Определяет минимальную длину, т. е. значением функции 
        /// будет символьная строка длиной как минимум выр-2 символов, 
        /// и если преобразуемое числовое значение требует выр-2 символов 
        /// или меньше, оно будет выровнено по правой границе в пределах 
        /// этой длины. Если количество символов, требуемое для представления 
        /// значения выр-1, больше данной длины, то используются 
        /// дополнительные позиции. В этом случае выходная строка будет длиннее, 
        /// чем выр-2 символов.
        /// </summary>
        public PftNumber Argument2 { get; set; }

        /// <summary>
        /// Определяет количество десятичных цифр дробной части Выр-1.
        /// Если оно опущено, то результат будет представлен в экспоненциальной форме. 
        /// Если при этом также опущено выр-2, то по умолчанию длина выходной строки 
        /// будет равна 16 символам.
        /// Если выр-3 присутствует, то результатом будет округленное представление 
        /// выр-1 с фиксированной точкой с выр-3 цифрами после десятичной точки.
        /// Если выр-3 равно нулю, то выр-1 округляется до ближайшего целого числа 
        /// и результатом будет целое число без десятичной точки.
        /// Если при преобразовании целых чисел и чисел с фиксированной точкой оказывается, 
        /// что целая часть числа слишком большая для ее представления, 
        /// то выходная строка заменяется последовательностью символов "*".
        /// Функция F может использоваться для выравнивания колонки чисел по десятичной 
        /// точке путем выбора соответствующей длины.
        /// </summary>
        public PftNumber Argument3 { get; set; }

        #endregion

        #region Construction

        public PftFFunction()
        {
        }

        public PftFFunction(PftParser.FFunctionContext node) 
            : base(node)
        {
            Argument1 = new PftArithExpression(node.arg1);
            Children.Add(Argument1);
            if (node.arg2 != null)
            {
                Argument2 = new PftArithExpression(node.arg2);
                Children.Add(Argument2);
            }
            if (node.arg3 != null)
            {
                Argument3 = new PftArithExpression(node.arg3);
                Children.Add(Argument3);
            }
        }

        #endregion

        #region PftAst members

        public override void Execute
            (
                PftContext context
            )
        {
            Argument1.Execute(context);

            int minLength = 1;
            if (Argument2 == null)
            {
                if (Argument3 == null)
                {
                    minLength = 16;
                }
            }
            else
            {
                Argument2.Execute(context);
                minLength = (int) Argument2.Value;
            }

            bool useE = true;
            int decimalPoints = 0;
            if (Argument3 != null)
            {
                useE = false;
                Argument3.Execute(context);
                decimalPoints = (int) Argument3.Value;
            }

            string format = useE
                ? string.Format("E{0}", minLength)
                : string.Format("F{0}", decimalPoints);

            context.Write
                (
                    Argument1.Value.ToString
                    (
                        format,
                        CultureInfo.InvariantCulture
                    )
                );
        }

        #endregion
    }
}
