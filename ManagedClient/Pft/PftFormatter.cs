/* PftFormatter.cs
 */

#region Using directives

using System;
using System.Text.RegularExpressions;

using Antlr4.Runtime;

#endregion

namespace ManagedClient.Pft
{
    using Abstraction;
    using Ast;

    /// <summary>
    /// Локальный форматтер: интерпретатор PFT-скриптов.
    /// </summary>
    public sealed class PftFormatter
        : IDisposable
    {
        #region Events

        #endregion

        #region Properties

        /// <summary>
        /// Уровень абстракции от платформы.
        /// </summary>
        public PftAbstractionLayer AbstractionLayer { get { return _abstractionLayer; } }

        /// <summary>
        /// Контекст форматирования. Во время парсинга не нужен.
        /// </summary>
        public PftContext Context { get; set; }

        /// <summary>
        /// Корневой элемент синтаксического дерева - собственно программа.
        /// </summary>
        public PftProgram Program { get; set; }

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

        /// <summary>
        /// Форматные выходы.
        /// </summary>
        public PftFormatExitManager FormatExits { get; set; }

        #endregion

        #region Construction

        public PftFormatter()
            : this
            (
                new PftAbstractionLayer(),
                new PftContext(null,null)
            )
        {
        }

        public PftFormatter
            (
                PftAbstractionLayer abstractionLayer,
                PftContext context
            )
        {
            if (ReferenceEquals(abstractionLayer, null))
            {
                throw new ArgumentNullException("abstractionLayer");
            }
            if (ReferenceEquals(context, null))
            {
                throw new ArgumentNullException("context");
            }

            _abstractionLayer = abstractionLayer;
            Context = context;
            Context._SetFormatter(this);
        }

        public PftFormatter
            (
                PftAbstractionLayer abstractionLayer,
                ManagedClient64 client
            )
            : this
            (
                abstractionLayer,
                new PftContext(null,null) { Client = client }
            )
        {            
        }

        public PftFormatter
            (
                ManagedClient64 client
            )
            : this
            (
                new PftAbstractionLayer(),
                client
            )
        {            
        }

        #endregion

        #region Private members

        private readonly PftAbstractionLayer _abstractionLayer;

        private string _InlineEvaluator
            (
                Match match
            )
        {
            string formatName = match.Value;
            formatName = formatName
                .Substring
                (
                    1,
                    formatName.Length - 2
                );
            CheckConnection();
            string result = Context.Client.ReadTextFile(formatName);
            return result;
        }

        #endregion

        #region Public methods

        public PftFormatter CheckConnection ()
        {
            ManagedClient64 client = Context.Client;
            if (ReferenceEquals(client, null)
                || !client.Connected)
            {
                throw new PftNotConnectedException();
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

        public string ResolveInline
            (
                string input
            )
        {
            string result = Regex.Replace
                (
                    input,
                    "\x1C.*?\x1D",
                    _InlineEvaluator
                );
            return result;
        }

        #endregion

        #region IDisposable members

        public void Dispose()
        {

        }

        #endregion
    }
}
