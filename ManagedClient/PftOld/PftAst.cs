/* PftAst.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Antlr4.Runtime;

#endregion

namespace ManagedClient.Pft
{
    using Debugging;

    /// <summary>
    /// Абстрактный элемент синтаксического дерева.
    /// </summary>
    [Serializable]
    public abstract class PftAst
    {
        #region Events

        /// <summary>
        /// Вызывается непосредственно перед выполнением.
        /// </summary>
        public event EventHandler<PftDebugEventArgs> BeforeExecution;

        /// <summary>
        /// Вызывается непосредственно после выполнения.
        /// </summary>
        public event EventHandler<PftDebugEventArgs> AfterExecution;

        #endregion

        #region Properties

        /// <summary>
        /// Точка останова (прерывания).
        /// </summary>
        public PftBreakpoint Breakpoint { get; set; }

        public string Text
        {
            get { return Node.GetText(); }
        }

        /// <summary>
        /// Стартовая позиция в исходном тексте.
        /// </summary>
        public int Position { get { return Node.Start.StartIndex; } }

        public int LineNumber { get { return Node.start.Line; } }

        public List<PftAst> Children { get { return _children; } }

        public ParserRuleContext Node { get; set; }

        #endregion

        #region Construction

        protected PftAst()
        {
            _children = new List<PftAst>();
        }

        protected PftAst
            (
                ParserRuleContext node
            )
            : this()
        {
            Node = node;
        }

        #endregion

        #region Private members

        protected List<PftAst> _children;

        protected void OnBeforeExecution
            (
                PftContext context
            )
        {
            var handler = BeforeExecution;
            if (handler != null)
            {
                PftDebugEventArgs eventArgs = new PftDebugEventArgs
                    (
                        context,
                        this
                    );
                handler(this, eventArgs);
            }
        }

        protected void OnAfterExecution
            (
                PftContext context
            )
        {
            var handler = AfterExecution;
            if (handler != null)
            {
                PftDebugEventArgs eventArgs = new PftDebugEventArgs
                    (
                        context,
                        this
                    );
                handler(this, eventArgs);
            }
        }

        #endregion

        #region Public methods

        public PftAst[] GetFinalChildren()
        {
            if (Children.Count == 0)
            {
                return new PftAst[0];
            }

            return Children
                .SelectMany(child => child.GetFinalChildren())
                .ToArray();
        }

        public PftAst GetLeftmostChild()
        {
            return GetFinalChildren()
                .OrderBy( child => child.Position )
                .FirstOrDefault();
        }

        /// <summary>
        /// Собственно форматирование.
        /// Включает в себя результат
        /// форматирования всеми потомками.
        /// </summary>
        /// <param name="context"></param>
        /// <returns><c>true</c>if any field data present.</returns>
        public virtual bool Execute
            (
                PftContext context
            )
        {
            bool result = false;

            OnBeforeExecution(context);
            foreach (PftAst child in Children)
            {
                if (child.Execute(context))
                {
                    result = true;
                }
            }
            OnAfterExecution(context);

            return result;
        }

        /// <summary>
        /// Список полей, задействованных в форматировании
        /// данным элементом и всеми его потомками, включая
        /// косвенных.
        /// </summary>
        /// <returns></returns>
        public virtual string[] GetAffectedFields()
        {
            string[] result = new string[0];

            foreach (PftAst child in Children)
            {
                string[] sub = child.GetAffectedFields();
                if ((sub != null)
                    && (sub.Length != 0))
                {
                    result = result
                        .Union(sub)
                        .Distinct()
                        .ToArray();
                }
            }

            return result;
        }

        /// <summary>
        /// Оптимизация дерева потомков.
        /// На данный момент не реализована.
        /// </summary>
        /// <returns></returns>
        public virtual PftAst Optimize()
        {
            if (_children.Count == 1)
            {
                return _children[0].Optimize();
            }
            return this;
        }

        /// <summary>
        /// Получение списка потомков (как прямых,
        /// так и косвенных) определенного типа.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetDescendants<T>()
            where T : PftAst
        {
            List<T> result = new List<T>();

            foreach (PftAst child in Children)
            {
                if (child is T)
                {
                    result.Add((T)child);
                }
                result.AddRange(child.GetDescendants<T>());
            }

            return result;
        }

        /// <summary>
        /// Семантическая валидация поддерева.
        /// На данный момент не реализована
        /// </summary>
        /// <param name="throwOnError"></param>
        /// <returns></returns>
        public virtual bool Validate
            (
                bool throwOnError
            )
        {
            bool result = Children
                .All
                (
                    child => child.Validate(throwOnError)
                );
            if (!result && throwOnError)
            {
                throw new ArgumentException();
            }
            return result;
        }

        /// <summary>
        /// Отладочный вывод "ступеньками" или "деревом".
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="level"></param>
        public void PrintDebug
            (
                TextWriter writer,
                int level
            )
        {
            for (int i = 0; i < level; i++)
            {
                writer.Write("| ");
            }
            writer.WriteLine
                (
                    "{0}: {1}",
                    GetType().Name,
                    Text
                );
            foreach (PftAst child in Children)
            {
                child.PrintDebug(writer, level + 1);
            }
        }

        /// <summary>
        /// Формирование исходного текста по AST.
        /// Применяется, например, для красивой 
        /// распечатки программы на языке PFT.
        /// </summary>
        /// <param name="writer"></param>
        public virtual void Write
            (
                StreamWriter writer
            )
        {
            foreach (PftAst child in Children)
            {
                child.Write(writer);
            }
        }

        public static PftAst DispatchContext
            (
                ParserRuleContext context
            )
        {
            throw new PftException();
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (PftAst child in Children)
            {
                result.Append(child);
            }
            return result.ToString();
        }

        #endregion
    }
}
