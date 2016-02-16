/* PftAst.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

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
        /// Список потомков. Может быть пустым.
        /// </summary>
        public List<PftAst> Children { get { return _children; } }

        /// <summary>
        /// Номер строки, на которой в скрипте расположена
        /// соответствующая конструкция языка.
        /// </summary>
        public int LineNumber { get; set; }

        public string Text { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        protected List<PftAst> _children;

        protected T ChangeChild<T>
            (
                T fromItem,
                T toItem
            )
            where T: PftAst
        {
            if (!ReferenceEquals(fromItem, null))
            {
                Children.Remove(fromItem);
            }
            if (!ReferenceEquals(toItem, null))
            {
                Children.Add(toItem);
            }
            return toItem;
        }

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

        public void DiscoverChildren
            (
                ParserRuleContext context
            )
        {
            foreach (IParseTree node in context.children)
            {
                ParserRuleContext ctx = node as ParserRuleContext;
                if (ctx != null)
                {
                    PftAst child = PftDispatcher.DispatchFormat(ctx);
                    if (!ReferenceEquals(child, null))
                    {
                        Children.Add(child);
                    }
                }
            }                        
        }

        /// <summary>
        /// Собственно форматирование.
        /// Включает в себя результат
        /// форматирования всеми потомками.
        /// </summary>
        /// <param name="context"></param>
        public virtual void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);
            
            foreach (PftAst child in Children)
            {
                child.Execute(context);
            }

            OnAfterExecution(context);
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
        /// Построение массива потомков-листьев 
        /// (т. е. не имеющих собственных потомков).
        /// </summary>
        /// <remarks>Если у узла нет потомков,
        /// он возвращает массив из одного элемента:
        /// самого себя.</remarks>
        /// <returns></returns>
        public PftAst[] GetLeafs()
        {
            if (Children.Count == 0)
            {
                return new [] { this };
            }

            return Children
                .SelectMany(child => child.GetLeafs())
                .ToArray();
        }

        /// <summary>
        /// Оптимизация дерева потомков.
        /// На данный момент не реализована.
        /// </summary>
        /// <remarks>Если возвращает <c>null</c>,
        /// это означает, что данный узел и всех
        /// его потомков можно безболезненно удалить.
        /// </remarks>
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
        /// Поддерживает ли многопоточность,
        /// т. е. может ли быть запущен одновременно
        /// с соседними элементами.
        /// </summary>
        /// <returns></returns>
        public virtual bool SupportsMultithreading()
        {
            return (Children.Count != 0)
                && Children.All
                (
                    child => child.SupportsMultithreading()
                );
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

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
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
