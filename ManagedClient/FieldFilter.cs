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

#endregion

namespace ManagedClient
{
    public static class FieldFilter
    {
        #region Public methods

        public static RecordField[] GetField
            (
                this IEnumerable<RecordField> fields,
                string tag
            )
        {
            return fields
                .NonNullItems()
                .Where(field => field.Tag.SameString(tag))
                .ToArray();
        }

        public static RecordField GetField
            (
                this IEnumerable<RecordField> fields,
                string tag,
                int occurrence
            )
        {
            return fields
                .GetField(tag)
                .GetOccurrence(occurrence);
        }

        public static RecordField[] GetField
            (
                this IEnumerable<RecordField> fields,
                params string[] tags
            )
        {
            return fields
                .NonNullItems()
                .Where(field => field.Tag.OneOf(tags))
                .ToArray();
        }

        public static RecordField GetField
            (
                this IEnumerable<RecordField> fields,
                string[] tags,
                int occurrence
            )
        {
            return fields
                .GetField(tags)
                .GetOccurrence(occurrence);
        }

        public static RecordField[] GetFieldRegex
            (
                this IEnumerable<RecordField> fields,
                string tagRegex
            )
        {
            Regex regex = new Regex(tagRegex);
            return fields
                .NonNullItems()
                .Where(field => regex.IsMatch(field.Tag))
                .ToArray();
        }

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

        public static SubField[] GetSubField
            (
                this IEnumerable<SubField> subFields,
                char[] codes
            )
        {
            return subFields
                .Where(sub => sub.Code.OneOf(codes))
                .ToArray();
        }

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

        public static string GetFieldText
            (
                this RecordField field
            )
        {
            return (ReferenceEquals(field, null))
                       ? null
                       : field.Text;
        }

        public static string[] GetFieldText
            (
                this IEnumerable<RecordField> fields
            )
        {
            return fields
                .NonNullItems()
                .Select(field => field.GetFieldText())
                .NonEmptyLines()
                .ToArray();
        }

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

        public static SubField[] GetSubField
            (
                this IEnumerable<RecordField> fields,
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

        public static SubField[] GetSubField
            (
                this IEnumerable<RecordField> fields,
                params char[] codes
            )
        {
            return fields
                .NonNullItems()
                .AllSubFields()
                .Where(sub=>sub.Code.OneOf(codes))
                .ToArray();
        }

        public static SubField[] GetSubField
            (
                this IEnumerable<RecordField> fields,
                string tag,
                char code
            )
        {
            return fields
                .NonNullItems()
                .GetField(tag)
                .GetSubField(code);
        }

        public static SubField GetSubField
            (
                this IEnumerable<RecordField> fields,
                string tag,
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

        public static SubField GetSubField
            (
                this IEnumerable<RecordField> fields,
                string tag,
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

        public static string GetSubFieldText
            (
                this SubField subField
            )
        {
            return (subField == null)
                       ? null
                       : subField.Text;
        }

        public static string[] GetSubFieldText
            (
                this IEnumerable<SubField> subFields
            )
        {
            return subFields
                .NonNullItems()
                .Select(sub=>sub.Text)
                .NonEmptyLines()
                .ToArray();
        }

        public static string GetSubFieldText
            (
                this IEnumerable<RecordField> fields,
                string tag,
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

        public static RecordField[] GetField
            (
                this IEnumerable<RecordField> fields,
                Func<RecordField, bool> predicate
            )
        {
            return fields
                .Where(predicate)
                .ToArray();
        }

        public static RecordField[] GetField
            (
                this IEnumerable<RecordField> fields,
                Func<SubField, bool> predicate
            )
        {
            return fields
                .NonNullItems()
                .Where(field => field.SubFields.Any(predicate))
                .ToArray();
        }

        public static RecordField[] GetField
            (
                this IEnumerable<RecordField> fields,
                char[] codes,
                Func<SubField, bool> predicate
            )
        {
            return fields
                .Where(field => field.SubFields
                    .NonNullItems()
                    .Any(sub => sub.Code.OneOf(codes)
                        && predicate(sub)))
                .ToArray();
        }

        public static RecordField[] GetField
            (
                this IEnumerable<RecordField> fields,
                char[] codes,
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

        public static RecordField[] GetField
            (
                this IEnumerable<RecordField> fields,
                char code,
                string value
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

        public static RecordField[] GetField
            (
                this IEnumerable<RecordField> fields,
                string[] tags,
                char[] codes,
                string[] values
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

        public static RecordField[] GetField
            (
                this IEnumerable<RecordField> fields,
                Func<RecordField, bool> fieldPredicate,
                Func<SubField, bool> subPredicate
            )
        {
            return fields
                .NonNullItems()
                .Where(fieldPredicate)
                .Where(field => field.SubFields.Any(subPredicate))
                .ToArray();
        }

        public static SubField[] GetSubField
            (
                this IEnumerable<RecordField> fields,
                Func<RecordField, bool> fieldPredicate,
                Func<SubField, bool> subPredicate
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

        public static SubField[] GetSubField
            (
                this IEnumerable<RecordField> fields,
                string[] tags,
                char[] codes
            )
        {
            return fields
                .NonNullItems()
                .GetField(tags)
                .NonNullItems()
                .GetSubField(codes)
                .ToArray();
        }

        #endregion
    }
}
