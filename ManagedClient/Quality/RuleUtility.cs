/* RuleUtility.cs --
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace ManagedClient.Quality
{
    /// <summary>
    /// Утилиты для правил.
    /// </summary>
    public static class RuleUtility
    {
        #region Private members

        private static readonly char[] _delimiters = {';', ',', ' ', '\t'};

        // ReSharper disable PossibleMultipleEnumeration

        private static IEnumerable<RecordField> _GetField1
            (
                IEnumerable<RecordField> fields,
                string oneSpec
            )
        {
            if (string.IsNullOrEmpty(oneSpec))
            {
                return new RecordField[0];
            }
            if (oneSpec.Contains('x'))
            {
                oneSpec = oneSpec.Replace("x", "[0-9]");
            }
            if (oneSpec.Contains('X'))
            {
                oneSpec = oneSpec.Replace("X", "[0-9]");
            }
            return oneSpec.Contains('[')
                ? fields.GetFieldRegex(oneSpec)
                : fields.GetField(oneSpec);
        }

        private static IEnumerable<RecordField> _GetField2
            (
                IEnumerable<RecordField> fields,
                string allSpec
            )
        {
            List<RecordField> result = new List<RecordField>();
            
            string[] parts = allSpec.Split
                (
                    _delimiters,
                    StringSplitOptions.RemoveEmptyEntries
                );
            foreach (string oneSpec in parts)
            {
                result.AddRange(_GetField1(fields, oneSpec));
            }

            return result.ToArray();
        }

        #endregion

        #region Public methods

        public static RecordField[] GetFieldBySpec
            (
                this IEnumerable<RecordField> fields,
                string allSpec
            )
        {
            if (string.IsNullOrEmpty(allSpec))
            {
                return new RecordField[0];
            }
            
            List<RecordField> result = new List<RecordField>();

            string[] parts = allSpec.Split('!');
            if (parts.Length > 2)
            {
                throw new FormatException("allSpec");
            }

            string include = parts[0].Trim(_delimiters);
            string exclude = parts.Length == 2
                ? parts[1].Trim(_delimiters)
                : string.Empty;
            if (string.IsNullOrEmpty(include))
            {
                if (!string.IsNullOrEmpty(exclude))
                {
                    result.AddRange(fields);
                }
            }
            else
            {
                result.AddRange(_GetField2(fields, include));
            }

            result = result
                .Distinct()
                .ToList();
            if (result.Count != 0)
            {
                result = result
                    .Except(_GetField2(fields, exclude))
                    .ToList();
            }

            return result.ToArray();
        }

        public static void RenumberFields
            (
                IEnumerable<RecordField> fields
            )
        {
            List<string> seen = new List<string>();

            foreach (RecordField field in fields)
            {
                int count = 1;
                foreach (string s in seen)
                {
                    if (s == field.Tag)
                    {
                        count++;
                    }
                }
                field.Repeat = count;
            }

        }

        #endregion
    }
}
