/* PftValFunction.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Возвращает числовое значение своего аргумента. 
    /// Аргумент - это формат, который может содержать 
    /// любую допустимую команду форматирования. 
    /// Сначала вычисляется аргумент, чтобы получить строку текста. 
    /// Затем эта строка просматривается слева направо до тех пор, 
    /// пока не будет найдено числовое значение, представленное 
    /// в текстовом виде (которое может быть  представлено 
    /// в экспоненциальной форме). Функция VAL возвращает 
    /// это числовое значение, переведенное во внутреннее машинное 
    /// представление, удобное для  выполнения вычислений.
    /// Если не найдено ни одно числовое значение, то функция 
    /// возвращает значение ноль. 
    /// Если текст содержит более, чем одно числовое значение, 
    /// возвращается только первое.
    /// </summary>
    [Serializable]
    public sealed class PftValFunction
        : PftNumber
    {
        #region Properties

        public PftAst Argument { get; set; }

        #endregion

        #region Construction

        public PftValFunction()
        {
        }

        public PftValFunction(PftParser.ValFunctionContext node) 
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
            string argument = context.Evaluate(Argument);
            Value = ExtractNumber(argument);
        }

        #endregion
    }
}
