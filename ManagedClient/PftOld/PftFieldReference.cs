/* PftFieldReference.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Antlr4.Runtime.Tree;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Ссылка на поле записи.
    /// </summary>
    [Serializable]
    public sealed class PftFieldReference
        : PftFieldOrGlobal
    {
        #region Properties

        /// <summary>
        /// Собственно ссылка на поле.
        /// </summary>
        public FieldReference Field { get; set; }

        #endregion

        #region Construction

        public PftFieldReference()
        {
        }

        public PftFieldReference(PftParser.FieldReferenceContext node)
            : base(node)
        {
            string leftConditional = _JoinTerminals(node.leftHand().CONDITIONAL());
            string leftRepeatable = _JoinTerminals(node.leftHand().REPEATABLE());
            bool leftPlus = node.leftHand().PLUS().Length != 0;
            string rightConditional = _JoinTerminals(node.rightHand().CONDITIONAL());
            string rightRepeatable = _JoinTerminals(node.rightHand().REPEATABLE());
            bool rightPlus = node.rightHand().PLUS().Length != 0;

            StringBuilder all = new StringBuilder();
            _Append(all, '"', leftConditional);
            _Append(all, '|', leftRepeatable);
            if (leftPlus)
            {
                all.Append('+');
            }
            all.Append(node.FIELD().GetText());
            if (rightPlus)
            {
                all.Append('+');
            }
            _Append(all, '|', rightRepeatable);
            _Append(all, '"', rightConditional);

            Field = new FieldReference(all.ToString());
        }

        #endregion

        #region Private members

        private static void _Append
            (
            StringBuilder builder,
            char delimiter,
            string text
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                builder.Append(delimiter);
                builder.Append(text);
                builder.Append(delimiter);
            }
        }

        private static string _JoinTerminals
            (
            IEnumerable<ITerminalNode> nodes
            )
        {
            return string.Join
                (
                    string.Empty,
                    nodes.Select(tn => _StripEnds(tn.GetText())).ToArray()
                );
        }

        private static string _StripEnds(string text)
        {
            return text.Substring
                (
                    1,
                    text.Length - 2
                );
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
        /// Возможность вычислить значение без записи
        /// в выходной буфер.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string Evaluate
            (
                PftContext context
            )
        {
            string result;

            if (Group != null)
            {
                if (Group.GroupIndex >= Group.GroupItems.Length)
                {
                    result = string.Empty;
                }
                else
                {
                    RecordField[] fields =
                    {
                        Group.GroupItems[Group.GroupIndex]
                    };
                    result = Field.FormatSingle(fields);
                }
            }
            else
            {
                result = Field.FormatSingle(context.Record);
            }

            return result;
        }

        #endregion
    }
}