/* PftFormatExit.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Форматные выходы &amp;umarci и &amp;unifor.
    /// </summary>
    [Serializable]
    public sealed class PftFormatExit
        : PftGroupItem
    {
        #region Properties

        /// <summary>
        /// Имя выхода.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Формат.
        /// </summary>
        public PftStatement Statement { get; set; }

        #endregion

        #region Construction

        public PftFormatExit()
        {
        }

        public PftFormatExit(PftParser.FormatExitContext node)
            : base(node)
        {
            Name = node.FORMATEXIT().GetText().Substring(1).ToLower();

            switch (Name)
            {
                case "umarci":
                    break;
                case "unifor":
                    break;
                case "uf":
                    Name = "unifor";
                    break;
                default:
                    throw new ArgumentException();
            }

            Statement = new PftStatement(node.statement());
        }

        #endregion

        #region PftAst members

        public override void Execute
            (
                PftContext context
            )
        {
            string text = Evaluate(context);
            context.Write(text);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Возможность вычислить значение,
        /// не выводя его в контекст.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string Evaluate
            (
                PftContext context
            )
        {
            switch (Name)
            {
                case "unifor":
                    return EvaluateUnifor(context);
                case "umarci":
                    return EvaluateUmarci(context);
            }
            throw new ArgumentException();
        }

        /// <summary>
        /// Возможность вычислить значение,
        /// не выводя его в контекст.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string EvaluateUnifor
            (
                PftContext context
            )
        {
            context = context.Push();
            Unifor unifor = new Unifor(context,Group);
            Statement.Execute(context);
            string format = context.ToString();
            return unifor.Evaluate(format);
        }

        /// <summary>
        /// Возможность вычислить значение,
        /// не выводя его в контекст.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string EvaluateUmarci
            (
                PftContext context
            )
        {
            context = context.Push();
            Umarci umarci = new Umarci(context,Group);
            Statement.Execute(context);
            string format = context.ToString();
            return umarci.Evaluate(format);
        }

        #endregion
    }
}