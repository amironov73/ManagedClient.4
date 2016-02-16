/* PftNumber.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Antlr4.Runtime.Tree;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Общий предок для числовых значений.
    /// </summary>
    [Serializable]
    public abstract class PftNumber
        : PftAst
    {
        #region Properties

        public double Value { get; set; }

        #endregion

        #region Construction

        protected PftNumber()
        {
        }

        protected PftNumber
            (
                double value
            )
        {
            Value = value;
        }

        protected PftNumber(IParseTree node) 
            : base(node)
        {
        }

        #endregion

        #region Private members

        private static Regex _GetRegex()
        {
            return new Regex(@"-?[0-9]*(\.[0-9]*)?([Ee]-?[0-9]+)?");
        }

        #endregion

        #region Public methods

        public static double ExtractNumber
            (
                string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0.0;
            }

            Regex regex = _GetRegex();
            MatchCollection matches = regex.Matches(text);
            if (matches.Count == 0)
            {
                return 0.0;
            }
            text = matches[0].Value;
            double result;
            double.TryParse(text, out result);
            return result;
        }

        public static double[] ExtractNumbers
            (
                string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return new[] {0.0};
            }

            List<double> result = new List<double>();

            Regex regex = _GetRegex();
            MatchCollection matches = regex.Matches(text);
            foreach (Match match in matches)
            {
                text = match.Value;
                double value;
                if (double.TryParse(text, out value))
                {
                    result.Add(value);
                }
            }

            return (result.Count == 0)
                ? new [] {0.0}
                : result.ToArray();
        }

        #endregion
    }
}
