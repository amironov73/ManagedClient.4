// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FieldFilter.cs
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Filters fields and subfields.
    /// </summary>
    public static class FieldFilter
    {
        #region Private members

        private static readonly RecordField[] EmptyFieldArray
            = new RecordField[0];

        private static readonly SubField[] EmptySubFieldArray
            = new SubField[0];

        private static readonly string[] EmptyStringArray
            = new string[0];

        #endregion

        #region Public methods

        /// <summary>
        /// Get all subfields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] AllSubFields
            (
                this IEnumerable<RecordField> fields
            )
        {
            return fields
                .NonNullItems()
                .SelectMany(field => field.SubFields)
                .NonNullItems()
                .ToArray();
        }

        // ==========================================================

        /// <summary>
        /// Filter fields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] string tag
            )
        {
            return fields
                .NonNullItems()
                .Where(field => field.Tag.SameString(tag))
                .ToArray();
        }

        /// <summary>
        /// Filter fields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetField
            (
                [NotNull] this RecordFieldCollection fields,
                [NotNull] string tag
            )
        {
            int count = fields.Count;
            List<RecordField> result = null;
            for (int i = 0; i < count; i++)
            {
                if (fields[i].Tag == tag)
                {
                    if (ReferenceEquals(result, null))
                    {
                        result = new List<RecordField>();
                    }
                    result.Add(fields[i]);
                }
            }

            return ReferenceEquals(result, null)
                ? EmptyFieldArray
                : result.ToArray();
        }

        /// <summary>
        /// Filter fields.
        /// </summary>
        [NotNull]
        public static RecordField GetField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] string tag,
                int occurrence
            )
        {
            return fields
                .GetField(tag)
                .GetOccurrence(occurrence);
        }

        /// <summary>
        /// Filter fields.
        /// </summary>
        [CanBeNull]
        public static RecordField GetField
            (
                [NotNull] this RecordFieldCollection fields,
                [NotNull] string tag,
                int occurrence
            )
        {
            int count = fields.Count;
            for (int i = 0; i < count; i++)
            {
                if (fields[i].Tag == tag)
                {
                    if (occurrence == 0)
                    {
                        return fields[i];
                    }
                    occurrence--;
                }
            }

            return null;
        }

        /// <summary>
        /// Filter fields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                params string[] tags
            )
        {
            return fields
                .NonNullItems()
                .Where(field => field.Tag.OneOf(tags))
                .ToArray();
        }

        /// <summary>
        /// Filter fields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetField
            (
                [NotNull] this RecordFieldCollection fields,
                params string[] tags
            )
        {
            int count = fields.Count;
            List<RecordField> result = null;
            for (int i = 0; i < count; i++)
            {
                if (fields[i].Tag.OneOf(tags))
                {
                    if (ReferenceEquals(result, null))
                    {
                        result = new List<RecordField>();
                    }
                    result.Add(fields[i]);
                }
            }

            return ReferenceEquals(result, null)
                ? EmptyFieldArray
                : result.ToArray();
        }

        /// <summary>
        /// Filter fields.
        /// </summary>
        [CanBeNull]
        public static RecordField GetField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] string[] tags,
                int occurrence
            )
        {
            return fields
                .GetField(tags)
                .GetOccurrence(occurrence);
        }

        /// <summary>
        /// Filter fields.
        /// </summary>
        [CanBeNull]
        public static RecordField GetField
            (
                [NotNull] this RecordFieldCollection fields,
                [NotNull] string[] tags,
                int occurrence
            )
        {
            int count = fields.Count;
            for (int i = 0; i < count; i++)
            {
                if (fields[i].Tag.OneOf(tags))
                {
                    if (occurrence == 0)
                    {
                        return fields[i];
                    }
                    occurrence--;
                }
            }

            return null;
        }

        /// <summary>
        /// Filter fields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] Func<RecordField, bool> predicate
            )
        {
            return fields
                .Where(predicate)
                .ToArray();
        }

        /// <summary>
        /// Filter fields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] Func<SubField, bool> predicate
            )
        {
            return fields
                .NonNullItems()
                .Where(field => field.SubFields.Any(predicate))
                .ToArray();
        }

        /// <summary>
        /// Filter fields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] char[] codes,
                [NotNull] Func<SubField, bool> predicate
            )
        {
            return fields
                .Where(field => field.SubFields
                    .NonNullItems()
                    .Any(sub => sub.Code.OneOf(codes)
                                && predicate(sub)))
                .ToArray();
        }

        /// <summary>
        /// Filter fields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] char[] codes,
                params string[] values
            )
        {
            return fields
                .Where(field => field.SubFields
                    .NonNullItems()
                    .Any(sub => sub.Code.OneOf(codes)
                                && sub.Text.OneOf(values))
                )
                .ToArray();
        }

        /// <summary>
        /// Filter fields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                char code,
                [NotNull] string value
            )
        {
            return fields
                .NonNullItems()
                .Where(field => field.SubFields
                    .NonNullItems()
                    .Any(sub => sub.Code.SameChar(code)
                                && sub.Text.SameString(value)))
                .ToArray();
        }

        /// <summary>
        /// Filter fields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] string[] tags,
                [NotNull] char[] codes,
                [NotNull] string[] values
            )
        {
            return fields
                .NonNullItems()
                .Where(field => field.Tag.OneOf(tags))
                .Where(field => field.SubFields
                    .Any(sub => sub.Code.OneOf(codes)
                                && sub.Text.OneOf(values)))
                .ToArray();
        }

        /// <summary>
        /// Filter fields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] Func<RecordField, bool> fieldPredicate,
                [NotNull] Func<SubField, bool> subPredicate
            )
        {
            return fields
                .NonNullItems()
                .Where(fieldPredicate)
                .Where(field => field.SubFields.Any(subPredicate))
                .ToArray();
        }

        // ==========================================================

        /// <summary>
        /// Filter fields.
        /// </summary>
        public static RecordField[] GetFieldRegex
            (
                this IEnumerable<RecordField> fields,
                string tagRegex
            )
        {
            Regex regex = new Regex(tagRegex);
            return fields
                .NonNullItems()
                .Where(field => regex.IsMatch(field.Tag.ThrowIfNull()))
                .ToArray();
        }

        /// <summary>
        /// Filter fields.
        /// </summary>
        public static RecordField GetFieldRegex
            (
                this IEnumerable<RecordField> fields,
                string tagRegex,
                int occurrence
            )
        {
            return fields
                .GetFieldRegex(tagRegex)
                .GetOccurrence(occurrence);
        }

        /// <summary>
        /// Filter fields.
        /// </summary>
        public static RecordField[] GetFieldRegex
            (
                this IEnumerable<RecordField> fields,
                string[] tags,
                string textRegex
            )
        {
            Regex regex = new Regex(textRegex);
            return fields
                .GetField(tags)
                .Where(field => !ReferenceEquals(field.Text, null))
                .Where(field => regex.IsMatch(field.Text))
                .ToArray();
        }

        /// <summary>
        /// Filter fields.
        /// </summary>
        public static RecordField GetFieldRegex
            (
                this IEnumerable<RecordField> fields,
                string[] tags,
                string textRegex,
                int occurrence
            )
        {
            return fields
                .GetFieldRegex(tags, textRegex)
                .GetOccurrence(occurrence);
        }

        /// <summary>
        /// Filter fields.
        /// </summary>
        public static RecordField[] GetFieldRegex
            (
                this IEnumerable<RecordField> fields,
                string[] tags,
                char[] codes,
                string textRegex
            )
        {
            Regex regex = new Regex(textRegex);
            return fields
                .GetField(tags)
                .Where(field => field.FilterSubFields(codes)
                    .Where(sub => !ReferenceEquals(sub.Text, null))
                    .Any(sub => regex.IsMatch(sub.Text)))
                .ToArray();
        }

        /// <summary>
        /// Filter fields.
        /// </summary>
        public static RecordField GetFieldRegex
            (
                this IEnumerable<RecordField> fields,
                string[] tags,
                char[] codes,
                string textRegex,
                int occurrence
            )
        {
            return fields
                .GetFieldRegex(tags, codes, textRegex)
                .GetOccurrence(occurrence);
        }

        // ==========================================================

        /// <summary>
        /// Get field text.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static string[] GetFieldText
            (
                [NotNull] this RecordFieldCollection fields
            )
        {
            int count = fields.Count;
            List<string> result = null;
            for (int i = 0; i < count; i++)
            {
                string text = fields[i].Text;
                if (!string.IsNullOrEmpty(text))
                {
                    if (ReferenceEquals(result, null))
                    {
                        result = new List<string>();
                    }
                    result.Add(text);
                }
            }

            return ReferenceEquals(result, null)
                ? EmptyStringArray
                : result.ToArray();
        }

        /// <summary>
        /// Get field text.
        /// </summary>
        [CanBeNull]
        public static string GetFieldText
            (
                [CanBeNull] this RecordField field
            )
        {
            return ReferenceEquals(field, null)
                ? null
                : field.Text;
        }

        /// <summary>
        /// Get field text.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static string[] GetFieldText
            (
                [NotNull] this IEnumerable<RecordField> fields
            )
        {
            return fields
                .NonNullItems()
                .Select(field => field.GetFieldText())
                .NonEmptyLines()
                .ToArray();
        }

        // ==========================================================

        /// <summary>
        /// Filter subfields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] GetSubField
            (
                [NotNull] this IEnumerable<SubField> subFields,
                params char[] codes
            )
        {
            return subFields
                .Where(sub => sub.Code.OneOf(codes))
                .ToArray();
        }

        /// <summary>
        /// Filter subfields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] GetSubField
            (
                [NotNull] this SubFieldCollection subFields,
                params char[] codes
            )
        {
            int count = subFields.Count;
            List<SubField> result = null;
            for (int i = 0; i < count; i++)
            {
                if (subFields[i].Code.OneOf(codes))
                {
                    if (ReferenceEquals(result, null))
                    {
                        result = new List<SubField>();
                    }
                    result.Add(subFields[i]);
                }
            }

            return ReferenceEquals(result, null)
                ? EmptySubFieldArray
                : result.ToArray();
        }

        /// <summary>
        /// Filter subfields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] GetSubField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                params char[] codes
            )
        {
            return fields
                .NonNullItems()
                .AllSubFields()
                .Where(sub=>sub.Code.OneOf(codes))
                .ToArray();
        }

        /// <summary>
        /// Filter subfields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] GetSubField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] string tag,
                char code
            )
        {
            return fields
                .NonNullItems()
                .GetField(tag)
                .GetSubField(code);
        }

        /// <summary>
        /// Filter subfields.
        /// </summary>
        [CanBeNull]
        public static SubField GetSubField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] string tag,
                int fieldOccurrence,
                char code,
                int subOccurrence
            )
        {
            return fields
                .NonNullItems()
                .GetField(tag)
                .GetOccurrence(fieldOccurrence)
                .GetSubField(code)
                .GetOccurrence(subOccurrence);
        }

        /// <summary>
        /// Filter subfields.
        /// </summary>
        [CanBeNull]
        public static SubField GetSubField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] string tag,
                char code,
                int occurrence
            )
        {
            return fields
                .NonNullItems()
                .GetField(tag)
                .GetSubField(code)
                .GetOccurrence(occurrence);
        }

        /// <summary>
        /// Filter subfields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] GetSubField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                char code
            )
        {
            return fields
                .NonNullItems()
                .AllSubFields()
                .NonNullItems()
                .Where(sub => sub.Code.SameChar(code))
                .ToArray();
        }

        /// <summary>
        /// Filter subfields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] GetSubField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] Func<RecordField, bool> fieldPredicate,
                [NotNull] Func<SubField, bool> subPredicate
            )
        {
            return fields
                .NonNullItems()
                .Where(fieldPredicate)
                .NonNullItems()
                .GetSubField()
                .Where(subPredicate)
                .ToArray();
        }

        /// <summary>
        /// Filter subfields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] GetSubField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] string[] tags,
                [NotNull] char[] codes
            )
        {
            return fields
                .NonNullItems()
                .GetField(tags)
                .NonNullItems()
                .GetSubField(codes)
                .ToArray();
        }

        // ==========================================================

        /// <summary>
        /// Filter subfields.
        /// </summary>
        public static SubField[] GetSubFieldRegex
            (
                this IEnumerable<SubField> subFields,
                string codeRegex
            )
        {
            Regex regex = new Regex(codeRegex);
            return subFields
                .Where(sub => regex.IsMatch(sub.Code.ToString()))
                .ToArray();
        }

        /// <summary>
        /// Filter subfields.
        /// </summary>
        public static SubField[] GetSubFieldRegex
            (
                this IEnumerable<SubField> subFields,
                char[] codes,
                string textRegex
            )
        {
            Regex regex = new Regex(textRegex);
            return subFields
                .GetSubField(codes)
                .Where(sub => !ReferenceEquals(sub.Text, null)
                              && regex.IsMatch(sub.Text))
                .ToArray();
        }

        /// <summary>
        /// Filter subfields.
        /// </summary>
        public static SubField[] GetSubFieldRegex
            (
                this IEnumerable<RecordField> fields,
                string[] tags,
                char[] codes,
                string textRegex
            )
        {
            Regex regex = new Regex(textRegex);
            return fields
                .GetField(tags)
                .AllSubFields()
                .Where(sub => !ReferenceEquals(sub.Text, null)
                              && regex.IsMatch(sub.Text))
                .ToArray();
        }

        // ==========================================================


        /// <summary>
        /// Get subfield text.
        /// </summary>
        [CanBeNull]
        public static string GetSubFieldText
            (
                [CanBeNull] this SubField subField
            )
        {
            return ReferenceEquals(subField, null)
                       ? null
                       : subField.Text;
        }

        /// <summary>
        /// Get subfield text.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static string[] GetSubFieldText
            (
                [NotNull] this IEnumerable<SubField> subFields
            )
        {
            return subFields
                .NonNullItems()
                .Select(sub=>sub.Text)
                .NonEmptyLines()
                .ToArray();
        }

        /// <summary>
        /// Filter subfields.
        /// </summary>
        [CanBeNull]
        public static string GetSubFieldText
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] string tag,
                char code
            )
        {
            return fields
                .NonNullItems()
                .GetField(tag)
                .GetSubField(code)
                .FirstOrDefault()
                .GetSubFieldText();
        }

        // ==========================================================

        #endregion
    }
}
