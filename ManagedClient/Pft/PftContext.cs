/* PftContext.cs
 */

using System.Collections;
using System.Collections.Generic;

namespace ManagedClient.Pft
{
    /// <summary>
    /// Контекст форматирования
    /// </summary>
    public sealed class PftContext
    {
        #region Properties

        /// <summary>
        /// Форматтер
        /// </summary>
        public PftFormatter Formatter { get { return _formatter; } }

        /// <summary>
        /// Родительский контекст.
        /// </summary>
        public PftContext Parent { get { return _parent; } }

        /// <summary>
        /// Клиент для связи с сервером.
        /// </summary>
        public ManagedClient64 Client { get; set; }

        /// <summary>
        /// Текущая форматируемая запись.
        /// </summary>
        public IrbisRecord Record { get; set; }

        /// <summary>
        /// Выходной буфер, в котором накапливается результат
        /// форматирования, а также ошибки и предупреждения.
        /// </summary>
        public PftOutputBuffer Output { get; internal set; }

        /// <summary>
        /// Накопленный текст в основном потоке выходного буфера,
        /// т. е. собственно результат расформатирования записи.
        /// </summary>
        public string Text { get { return Output.ToString(); } }

        /// <summary>
        /// Режим вывода полей.
        /// </summary>
        public PftFieldOutputMode FieldOutputMode { get; set; }

        /// <summary>
        /// Режим перевода текста в верхний регистр при выводе полей.
        /// </summary>
        public bool UpperMode { get; set; }

        /// <summary>
        /// Глобальные переменные.
        /// </summary>
        public PftGlobalManager Globals { get; private set; }

        /// <summary>
        /// Нормальные переменные.
        /// </summary>
        public PftVariableManager Variables { get; set; }

        /// <summary>
        /// Процедуры.
        /// </summary>
        public PftProcedureManager Procedures { get; set; }

        #endregion

        #region Construction

        public PftContext
            (
                PftFormatter formatter,
                PftContext parent
            )
        {
            _formatter = formatter;

            _parent = parent;

            PftOutputBuffer parentBuffer = (parent == null)
                ? null
                : parent.Output;

            Output = new PftOutputBuffer(parentBuffer);

            Globals = (parent == null)
                ? new PftGlobalManager()
                : parent.Globals;

            // Переменные в каждом контексте свои
            Variables = new PftVariableManager();

            // Процедуры в каждом контексте свои
            Procedures = new PftProcedureManager();

            Record = (parent == null)
                ? new IrbisRecord()
                : parent.Record;

            Client = (parent == null)
                ? new ManagedClient64()
                : parent.Client;
        }

        #endregion

        #region Private members

        private PftFormatter _formatter;

        private readonly PftContext _parent;

        internal void _SetFormatter
            (
                PftFormatter formatter
            )
        {
            _formatter = formatter;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Полная очистка всех потоков: и основного,
        /// и предупреждений, и ошибок.
        /// </summary>
        /// <returns></returns>
        public PftContext ClearAll()
        {
            Output.ClearText();
            Output.ClearError();
            Output.ClearWarning();
            return this;
        }

        /// <summary>
        /// Очистка основного выходного потока.
        /// </summary>
        /// <returns></returns>
        public PftContext ClearText()
        {
            Output.ClearText();
            return this;
        }

        /// <summary>
        /// Временное переключение контекста (например,
        /// при вычислении строковых функций).
        /// </summary>
        /// <returns></returns>
        public PftContext Push()
        {
            PftContext result = new PftContext(Formatter,this);
            return result;
        }

        public void Pop()
        {
            if (!ReferenceEquals(Parent, null))
            {
                // Nothing to do?
            }
        }

        public PftContext Write
            (
                PftAst node,
                string format,
                params object[] arg
            )
        {
            if (!string.IsNullOrEmpty(format))
            {
                Output.Write(format, arg);
            }
            return this;
        }

        public PftContext Write
            (
                PftAst node,
                string value
            )
        {
            if (!string.IsNullOrEmpty(value))
            {
                Output.Write(value);
            }
            return this;
        }

        public PftContext WriteLine
            (
                PftAst node,
                string format,
                params object[] arg
            )
        {
            if (!string.IsNullOrEmpty(format))
            {
                Output.WriteLine(format, arg);
            }
            return this;
        }

        public PftContext WriteLine
            (
                PftAst node,
                string value
            )
        {
            if (!string.IsNullOrEmpty(value))
            {
                Output.WriteLine(value);
            }
            return this;
        }

        public PftContext WriteLine
            (
                PftAst node
            )
        {
            Output.WriteLine();
            return this;
        }

        /// <summary>
        /// Вычисление выражения во временной копии контекста.
        /// </summary>
        /// <param name="ast"></param>
        /// <returns></returns>
        public string Evaluate
            (
                PftAst ast
            )
        {
            PftContext copy = Push();
            ast.Execute(copy);
            string result = copy.ToString();
            Pop();
            return result;
        }

        /// <summary>
        /// Вычисление выражения во временной копии контекста.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public string Evaluate
            (
                IEnumerable<PftAst> items
            )
        {
            PftContext copy = Push();
            foreach (PftAst ast in items)
            {
                ast.Execute(copy);
            }
            string result = copy.ToString();
            Pop();
            return result;
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> 
        /// that represents this instance.</returns>
        public override string ToString()
        {
            return Output.ToString();
        }

        #endregion
    }
}