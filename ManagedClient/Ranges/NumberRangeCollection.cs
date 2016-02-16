/* NumberRangeCollection.cs
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Antlr4.Runtime;

#endregion

namespace ManagedClient.Ranges
{
    [Serializable]
    public sealed class NumberRangeCollection
        : IEnumerable<NumberText>
    {
        #region Constants

        public const string DefaultDelimiter = ",";

        #endregion

        #region Properties

        public string Delimiter { get; set; }

        #endregion

        #region Construction

        public NumberRangeCollection ()
        {
            Delimiter = DefaultDelimiter;
            _items = new List<NumberRange>();
        }

        private NumberRangeCollection
            (
                NumberRangesParser.ProgramContext program
            )
            : this()
        {
            foreach (NumberRangesParser.ItemContext itemContext 
                in program.item())
            {
                NumberRange range = new NumberRange(itemContext);
                _items.Add(range);
            }
        }

        #endregion

        #region Private members

        private readonly List<NumberRange> _items;

        #endregion

        #region Public methods

        public NumberRangeCollection Add
            (
                NumberRange range
            )
        {
            if (ReferenceEquals(range, null))
            {
                throw new ArgumentNullException("range");
            }
            _items.Add(range);
            return this;
        }

        public NumberRangeCollection Add
            (
                string start,
                string stop
            )
        {
            if (string.IsNullOrEmpty(start))
            {
                throw new ArgumentNullException("start");
            }
            if (string.IsNullOrEmpty(stop))
            {
                throw new ArgumentNullException("stop");
            }

            return Add
                (
                    new NumberRange(start, stop)
                );
        }

        public NumberRangeCollection Add
            (
                string startAndStop
            )
        {
            if (string.IsNullOrEmpty(startAndStop))
            {
                throw new ArgumentNullException("startAndStop");
            }
            return Add
                (
                    new NumberRange(startAndStop)
                );
        }

        public bool Contains
            (
                NumberText number
            )
        {
            if (ReferenceEquals(number, null))
            {
                throw new ArgumentNullException("number");
            }
            return _items.Any(item => item.Contains(number));
        }

        public static NumberRangeCollection Parse
            (
                string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("text");
            }

            AntlrInputStream stream = new AntlrInputStream(text);
            NumberRangesLexer lexer = new NumberRangesLexer(stream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            NumberRangesParser parser = new NumberRangesParser(tokens);
            NumberRangesParser.ProgramContext tree = parser.program();
            NumberRangeCollection result = new NumberRangeCollection(tree);
            return result;
        }

        public static NumberRangeCollection Cumulate
            (
                List<NumberText> numbers
            )
        {
            NumberRangeCollection result 
                = new NumberRangeCollection();

            if (numbers.Count != 0)
            {
                numbers.Sort();

                NumberText previous = numbers[0];
                NumberText last = previous.Copy();
                for (int i = 1; i < numbers.Count; i++)
                {
                    NumberText current = numbers[i];
                    NumberText next = last + 1;
                    if (current != next)
                    {
                        result.Add
                            (
                                new NumberRange
                                    (
                                        previous,
                                        last
                                    )
                            );
                        previous = current.Copy();
                    }
                    last = current;
                }
                result.Add
                    (
                        new NumberRange
                            (
                                previous,
                                last
                            )
                    );
            }

            return result;
        }

        public static NumberRangeCollection Cumulate
            (
                IEnumerable<string> texts
            )
        {
            if (ReferenceEquals(texts, null))
            {
                throw new ArgumentNullException("texts");
            }

            List<NumberText> numbers = texts
                .Select(text => new NumberText(text))
                .ToList();

            return Cumulate(numbers);
        }

        public void For
            (
                Action<NumberText> action
            )
        {
            foreach (NumberRange range in _items)
            {
                foreach (NumberText number in range)
                {
                    action
                        (
                            number
                        );
                }
            }
        }

        #endregion

        #region IEnumerable<NumberText> members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<NumberText> GetEnumerator()
        {
            foreach (NumberRange range in _items)
            {
                foreach (NumberText number in range)
                {
                    yield return number;
                }
            }
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            bool first = true;
            foreach (NumberRange item in _items)
            {
                string text = item.ToString();
                if (!string.IsNullOrEmpty(text))
                {
                    if (!first)
                    {
                        result.Append(Delimiter);
                    }
                    result.Append(text);
                    first = false;
                }
            }
            return result.ToString();
        }

        #endregion
    }
}
