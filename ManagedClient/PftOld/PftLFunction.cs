/* PftLFunction.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Использует текст, полученный в результате вычисления аргумента, 
    /// в качестве термина доступа для инвертированного файла и возвращает 
    /// MFN первой ссылки на этот термин, если она есть. Перед поиском 
    /// в инвертированном файле термин автоматически переводится в прописные 
    /// буквы. Если термин не найден, то функция принимает значение ноль. 
    /// Функция L обычно используется вместе с функцией REF .
    /// </summary>
    [Serializable]
    public sealed class PftLFunction
        : PftNumber
    {
        #region Properties

        public PftAst Argument { get; set; }

        #endregion

        #region Construction

        public PftLFunction()
        {
        }

        public PftLFunction(PftParser.LFunctionContext node)
            : base(node)
        {
            Argument = PftNonGrouped.DispatchContext(node.nonGrouped());
        }

        #endregion

        #region PftAst members

        public override void Execute
            (
                PftContext context
            )
        {
            string format = context.Evaluate(Argument).ToUpper();
            int[] mfns = context.Client.Search(format);
            Value = (mfns.Length != 0)
                ? mfns[0]
                : 0.0;
        }

        #endregion
    }
}
