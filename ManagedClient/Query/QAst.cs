// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* QAst.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Antlr4.Runtime.Tree;

#endregion

namespace ManagedClient.Query
{
    using IQP = IrbisQueryParser;

    public enum EndingKind
    {
        Default = 0,

        NoTrim = 0,

        TrimRight = 1,

        Morphology = 2
    }

    [Serializable]
    public class QAst
    {
        #region Properties

        public string Text { get; set; }

        public List<QAst> Children { get { return _children; } }

        #endregion

        #region Construction

        public QAst()
        {
            _children = new List<QAst>();
        }

        [CLSCompliant(false)]
        public QAst(IParseTree node)
            : this()
        {
            Text = node.GetText();
        }

        #endregion

        #region Private members

        [CLSCompliant(false)]
        protected List<QAst> _children;

        #endregion

        #region Public methods

        public virtual void Execute()
        {
        }

        public virtual QAst Optimize()
        {
            if (_children.Count == 1)
            {
                return _children[0].Optimize();
            }
            return this;
        }

        public List<T> GetDescendants<T>()
            where T : QAst
        {
            List<T> result = new List<T>();

            foreach (QAst child in Children)
            {
                if (child is T)
                {
                    result.Add((T)child);
                }
                result.AddRange(child.GetDescendants<T>());
            }

            return result;
        }

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
            foreach (QAst child in Children)
            {
                child.PrintDebug(writer, level + 1);
            }
        }

        public bool Walk
            (
                Predicate<QAst> predicate
            )
        {
            if (predicate(this))
            {
                foreach (QAst child in Children)
                {
                    child.Walk(predicate);
                }
                return true;
            }
            return false;
        }

        public bool Walk<T>
            (
                Func<QAst, T, bool> func,
                T arg
            )
        {
            if (func(this, arg))
            {
                foreach (QAst child in Children)
                {
                    child.Walk(func, arg);
                }
                return true;
            }
            return false;
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (QAst child in Children)
            {
                result.Append(child);
            }
            return result.ToString();
        }

        #endregion
    }

    public class QAstProgram : QAst
    {
        [CLSCompliant(false)]
        public QAstProgram
            (
                IQP.ProgramContext node
            )
            : base(node)
        {
            Children.Add(new QAstLevelThree(node.levelThree()));
        }

        public override QAst Optimize()
        {
            _children = _children.Select(c => c.Optimize()).ToList();
            return this;
        }
    }

    public class QAstLevelThree : QAst
    {
        [CLSCompliant(false)]
        public QAstLevelThree
            (
                IQP.LevelThreeContext node
            )
            : base(node)
        {
            if (node is IQP.LevelTwoOuterContext)
            {
                var levelTwo = (node as IQP.LevelTwoOuterContext).levelTwo();
                Children.Add(new QAstLevelTwo(levelTwo));
            }
            else if (node is IQP.ReferenceContext)
            {
                Children.Add(new QAstReference(node as IQP.ReferenceContext));
            }
            else if (node is IQP.StarOperator3Context)
            {
                var star = node as IQP.StarOperator3Context;
                Children.Add(new QAstStarOperator(star));
            }
            else if (node is IQP.PlusOperator3Context)
            {
                var plus = node as IQP.PlusOperator3Context;
                Children.Add(new QAstPlusOperator(plus));
            }
            else
            {
                throw new Exception();
            }
        }
    }

    public class QAstLevelTwo : QAst
    {
        [CLSCompliant(false)]
        public QAstLevelTwo
            (
                IQP.LevelTwoContext node
            )
            : base(node)
        {
            if (node is IQP.LevelOneOuterContext)
            {
                var levelOne = (node as IQP.LevelOneOuterContext).levelOne();
                Children.Add(new QAstLevelOne(levelOne));
            }
            else if (node is IQP.ParenOuterContext)
            {
                var paren = node as IQP.ParenOuterContext;
                Children.Add(new QAstParen(paren));
            }
            else if (node is IQP.StarOperator2Context)
            {
                var star = node as IQP.StarOperator2Context;
                Children.Add(new QAstStarOperator(star));
            }
            else if (node is IQP.PlusOperator2Context)
            {
                var plus = node as IQP.PlusOperator2Context;
                Children.Add(new QAstPlusOperator(plus));
            }
            else
            {
                throw new Exception();
            }
        }
    }

    public class QAstLevelOne : QAst
    {
        public QAstLevelOne()
        {
        }

        [CLSCompliant(false)]
        public QAstLevelOne
            (
                IQP.LevelOneContext node
            )
            : base(node)
        {
            if (node is IQP.EntryContext)
            {
                var entry = node as IQP.EntryContext;
                Children.Add(new QAstEntry(entry));
            }
            else if (node is IQP.DotOperatorContext)
            {
                var dot = node as IQP.DotOperatorContext;
                Children.Add(new QAstDotOperator(dot));
            }
            else if (node is IQP.FOperatorContext)
            {
                var f = node as IQP.FOperatorContext;
                Children.Add(new QAstFOperator(f));
            }
            else if (node is IQP.GOperatorContext)
            {
                var g = node as IQP.GOperatorContext;
                Children.Add(new QAstGOperator(g));
            }
            else if (node is IQP.StarOperator1Context)
            {
                var star = node as IQP.StarOperator1Context;
                Children.Add(new QAstStarOperator(star));
            }
            else if (node is IQP.PlusOperator1Context)
            {
                var plus = node as IQP.PlusOperator1Context;
                Children.Add(new QAstPlusOperator(plus));
            }
            else
            {
                throw new Exception();
            }
        }

        public override QAst Optimize()
        {
            _children = _children.Select(c => c.Optimize()).ToList();
            if (_children[0] is QAstLevelOne)
            {
                return _children[0];
            }
            return this;
        }
    }

    public class QAstEntry : QAst
    {
        public string Expression { get; set; }

        public bool Quoted { get; set; }

        public EndingKind Ending { get; set; }

        public List<string> Tags { get; private set; }

        public QAstEntry()
        {
            Tags = new List<string>();
        }

        public QAstEntry
            (
                IEnumerable<string> tags
            )
        {
            Tags = new List<string>();
            if (tags != null)
            {
                Tags.AddRange(tags);
            }
        }

        [CLSCompliant(false)]
        public QAstEntry
            (
                IQP.EntryContext node
            )
            : base(node)
        {
            Expression = Text;
            Tags = new List<string>();
            if (Text[0] == '"')
            {
                Quoted = true;
                int offset = Text.IndexOf('"', 1);
                Expression = Text.Substring(1, offset - 1);
                if (offset != (Text.Length - 1))
                {
                    _ExtractTags(Text.Substring(offset + 2));
                }
            }
            else
            {
                int offset = Text.IndexOf('/');
                if (offset > 0)
                {
                    Expression = Text.Substring(0, offset);
                    _ExtractTags(Text.Substring(offset + 1));
                }
            }

            if (Expression.EndsWith("$"))
            {
                Expression = Expression.Substring(0, Expression.Length - 1);
                Ending = EndingKind.TrimRight;
            }
            else if (Expression.EndsWith("@"))
            {
                Expression = Expression.Substring(0, Expression.Length - 1);
                Ending = EndingKind.Morphology;
            }
        }

        private void _ExtractTags(string tail)
        {
            Regex regex = new Regex(@"[0-9]+");
            MatchCollection matches = regex.Matches(tail);
            foreach (Match match in matches)
            {
                Tags.Add(match.Value);
            }
        }

        public override string ToString()
        {
            string result = Expression;
            switch (Ending)
            {
                case EndingKind.TrimRight:
                    result += "$";
                    break;
                case EndingKind.Morphology:
                    result += "@";
                    break;
            }
            if (Quoted)
            {
                result = "\"" + result + "\"";
            }
            if ((Tags != null) && (Tags.Count != 0))
            {
                result += ("/" + string.Join(",", Tags.ToArray()));
            }
            return result;
        }
    }

    public class QAstLevelOneOperator : QAst
    {
        public QAstLevelOne LeftOperand { get; set; }

        public QAstLevelOne RightOperand { get; set; }

        [CLSCompliant(false)]
        public QAstLevelOneOperator
            (
                IParseTree node,
                IQP.LevelOneContext left,
                IQP.LevelOneContext right
            )
            : base(node)
        {
            LeftOperand = new QAstLevelOne(left);
            Children.Add(LeftOperand);
            RightOperand = new QAstLevelOne(right);
            Children.Add(RightOperand);
        }

        public override QAst Optimize()
        {
            LeftOperand = (QAstLevelOne)LeftOperand.Optimize();
            RightOperand = (QAstLevelOne)RightOperand.Optimize();
            _children = new List<QAst> { LeftOperand, RightOperand };
            return this;
        }
    }

    public class QAstDotOperator : QAstLevelOneOperator
    {
        [CLSCompliant(false)]
        public QAstDotOperator
            (
                IQP.DotOperatorContext node
            )
            : base
            (
                node,
                node.left,
                node.right
            )
        {
        }

        public override string ToString()
        {
            return string.Format
                (
                    "{0} . {1}",
                    LeftOperand,
                    RightOperand
                );
        }
    }

    public class QAstFOperator : QAstLevelOneOperator
    {
        [CLSCompliant(false)]
        public QAstFOperator
            (
                IQP.FOperatorContext node
            )
            : base
            (
                node,
                node.left,
                node.right
            )
        {
        }

        public override string ToString()
        {
            return string.Format
                (
                    "{0} (F) {1}",
                    LeftOperand,
                    RightOperand
                );
        }
    }

    public class QAstGOperator : QAstLevelOneOperator
    {
        [CLSCompliant(false)]
        public QAstGOperator
            (
                IQP.GOperatorContext node
            )
            : base
            (
                node,
                node.left,
                node.right
            )
        {
        }

        public override string ToString()
        {
            return string.Format
                (
                    "{0} (G) {1}",
                    LeftOperand,
                    RightOperand
                );
        }
    }

    public class QAstAnyLevelOperator : QAst
    {
        public QAst LeftOperand { get; set; }

        public QAst RightOperand { get; set; }

        public QAstAnyLevelOperator()
        {
        }

        [CLSCompliant(false)]
        public QAstAnyLevelOperator
            (
                IParseTree node
            )
            : base(node)
        {
        }

        public override QAst Optimize()
        {
            LeftOperand = LeftOperand.Optimize();
            RightOperand = RightOperand.Optimize();
            _children = new List<QAst> { LeftOperand, RightOperand };
            return this;
        }
    }

    public class QAstPlusOperator : QAstAnyLevelOperator
    {
        public QAstPlusOperator()
        {
        }

        [CLSCompliant(false)]
        public QAstPlusOperator
            (
                IQP.PlusOperator1Context node
            )
            : base(node)
        {
            LeftOperand = new QAstLevelOne(node.left);
            Children.Add(LeftOperand);
            RightOperand = new QAstLevelOne(node.right);
            Children.Add(RightOperand);
        }

        [CLSCompliant(false)]
        public QAstPlusOperator
            (
                IQP.PlusOperator2Context node
            )
            : base(node)
        {
            LeftOperand = new QAstLevelTwo(node.left);
            Children.Add(LeftOperand);
            RightOperand = new QAstLevelTwo(node.right);
            Children.Add(RightOperand);
        }

        [CLSCompliant(false)]
        public QAstPlusOperator
            (
                IQP.PlusOperator3Context node
            )
            : base(node)
        {
            LeftOperand = new QAstLevelThree(node.left);
            Children.Add(LeftOperand);
            RightOperand = new QAstLevelThree(node.right);
            Children.Add(RightOperand);
        }

        public override string ToString()
        {
            return string.Format
                (
                    "{0} + {1}",
                    LeftOperand,
                    RightOperand
                );
        }
    }

    public class QAstStarOperator : QAstAnyLevelOperator
    {
        public string Operation { get; set; }

        [CLSCompliant(false)]
        public QAstStarOperator
            (
                IQP.StarOperator1Context node
            )
            : base(node)
        {
            LeftOperand = new QAstLevelOne(node.left);
            Children.Add(LeftOperand);
            Operation = node.op.Text;
            RightOperand = new QAstLevelOne(node.right);
            Children.Add(RightOperand);
        }

        [CLSCompliant(false)]
        public QAstStarOperator
            (
                IQP.StarOperator2Context node
            )
            : base(node)
        {
            LeftOperand = new QAstLevelTwo(node.left);
            Children.Add(LeftOperand);
            Operation = node.op.Text;
            RightOperand = new QAstLevelTwo(node.right);
            Children.Add(RightOperand);
        }

        [CLSCompliant(false)]
        public QAstStarOperator
            (
                IQP.StarOperator3Context node
            )
            : base(node)
        {
            LeftOperand = new QAstLevelThree(node.left);
            Children.Add(LeftOperand);
            Operation = node.op.Text;
            RightOperand = new QAstLevelThree(node.right);
            Children.Add(RightOperand);
        }

        public override string ToString()
        {
            return string.Format
                (
                    "{0} {1} {2}",
                    LeftOperand,
                    Operation,
                    RightOperand
                );
        }
    }

    public class QAstReference : QAst
    {
        public int Number { get; set; }

        [CLSCompliant(false)]
        public QAstReference
            (
                IQP.ReferenceContext node
            )
            : base(node)
        {
            Number = int.Parse(Text.Substring(1));
        }

        public override string ToString()
        {
            return Text;
        }
    }

    public class QAstParen : QAst
    {
        public QAstParen()
        {
        }

        [CLSCompliant(false)]
        public QAstParen
            (
                IQP.ParenOuterContext node
            )
            : base(node)
        {
            Children.Add(new QAstLevelTwo(node.levelTwo()));
        }

        public override QAst Optimize()
        {
            if (_children[0] is QAstParen)
            {
                return _children[0]; // Удаляем лишний уровень скобок
            }

            return this;
        }

        public override string ToString()
        {
            return string.Format
                (
                    "({0})",
                    base.ToString()
                );
        }
    }
}
