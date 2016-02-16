/* PftFormatter.cs
 */

#region Using directives

using System;
using Antlr4.Runtime;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Форматтер файлов PFT.
    /// </summary>
    public sealed class PftFormatter
    {
        #region Properties

        /// <summary>
        /// Контекст форматирования. Во время парсинга не нужен.
        /// </summary>
        public PftContext Context { get; set; }

        /// <summary>
        /// Корневой элемент синтаксического дерева - собственно программа.
        /// </summary>
        public PftProgram Program { get; set; }

        /// <summary>
        /// Кеш форматов.
        /// </summary>
        public PftCache Cache { get; set; }

        /// <summary>
        /// Нормальный результат расформатирования.
        /// </summary>
        public string Output { get { return Context.Text; } }

        /// <summary>
        /// Поток ошибок.
        /// </summary>
        public string Error { get { return Context.Output.ErrorText; } }

        /// <summary>
        /// Поток предупреждений.
        /// </summary>
        public string Warning { get { return Context.Output.WarningText; } }

        public bool HaveError { get { return Context.Output.HaveError; } }

        public bool HaveWarning { get { return Context.Output.HaveWarning; } }

        #endregion

        #region Construction

        public PftFormatter()
        {
            Context = new PftContext(null);
        }

        public PftFormatter
            (
                PftContext context
            )
        {
            Context = context;
        }

        public PftFormatter
            (
                ManagedClient64 client
            )
        {
            Context = new PftContext(null)
            {
                Client = client
            };
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public PftFormatter ParseInput
            (
                string input
            )
        {
            AntlrInputStream stream = new AntlrInputStream(input);
            PftLexer lexer = new PftLexer(stream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            PftParser parser = new PftParser(tokens);
            PftParser.ProgramContext programContext = parser.program();
            Program = new PftProgram(programContext);
            
            if (!Program.Validate(false))
            {
                throw new ArgumentException();
            }

            return this;
        }

        public string Format
            (
                IrbisRecord record
            )
        {
            Context.ClearAll();
            Context.Record = record;
            Program.Execute(Context);
            return Context.Text;
        }

        #endregion
    }
}
