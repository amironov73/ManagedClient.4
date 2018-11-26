// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FieldFilter.cs
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Filters fields and subfields.
    /// </summary>
    [PublicAPI]
    public static class FieldFilter
    {
        #region Private members

        /// <summary>
        /// Empty array of <see cref="RecordField"/>.
        /// </summary>
        public static readonly RecordField[] EmptyFieldArray = new RecordField[0];

        /// <summary>
        /// Empty array of <see cref="SubField"/>.
        /// </summary>
        public static readonly SubField[] EmptySubFieldArray = new SubField[0];

        #endregion

        #region Public methods

        /// <summary>
        /// Get all subfields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] AllSubFields
            (
                [NotNull] this IEnumerable<RecordField> fields
            )
        {
            LocalList<SubField> result = new LocalList<SubField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    foreach (SubField subField in field.SubFields)
                    {
                        if (!ReferenceEquals(subField, null))
                        {
                            result.Add(subField);
                        }
                    }
                }
            }

            return result.ToArray();
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
            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (field.Tag.SameString(tag))
                    {
                        result.Add(field);
                    }
                }
            }

            return result.ToArray();
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
            LocalList<RecordField> result = new LocalList<RecordField>();
            int count = fields.Count;
            for (int i = 0; i < count; i++)
            {
                if (fields[i].Tag.SameString(tag))
                {
                    result.Add(fields[i]);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Filter fields.
        /// </summary>
        [CanBeNull]
        public static RecordField GetField
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] string tag,
                int occurrence
            )
        {
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (field.Tag.SameString(tag))
                    {
                        if (occurrence == 0)
                        {
                            return field;
                        }

                        occurrence--;
                    }
                }
            }

            return null;
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
                if (fields[i].Tag.SameString(tag))
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
            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (field.Tag.OneOf(tags))
                    {
                        result.Add(field);
                    }
                }
            }

            return result.ToArray();
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
            LocalList<RecordField> result = new LocalList<RecordField>();
            for (int i = 0; i < count; i++)
            {
                if (fields[i].Tag.OneOf(tags))
                {
                    result.Add(fields[i]);
                }
            }

            return result.ToArray();
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
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (field.Tag.OneOf(tags))
                    {
                        if (occurrence == 0)
                        {
                            return field;
                        }

                        occurrence--;
                    }
                }
            }
            return null;
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
            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (predicate(field))
                    {
                        result.Add(field);
                    }
                }
            }

            return result.ToArray();
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
            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    foreach (SubField subField in field.SubFields)
                    {
                        if (predicate(subField))
                        {
                            result.Add(field);
                            break;
                        }
                    }
                }
            }

            return result.ToArray();
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
            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    foreach (SubField subField in field.SubFields)
                    {
                        if (subField.Code.OneOf(codes) && predicate(subField))
                        {
                            result.Add(field);
                            break;
                        }
                    }
                }
            }

            return result.ToArray();
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
            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    foreach (SubField subField in field.SubFields)
                    {
                        if (subField.Code.OneOf(codes)
                            && subField.Text.OneOf(values))
                        {
                            result.Add(field);
                            break;
                        }
                    }
                }
            }

            return result.ToArray();
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
            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    foreach (SubField subField in field.SubFields)
                    {
                        if (subField.Code.SameChar(code)
                            && subField.Text.SameString(value))
                        {
                            result.Add(field);
                            break;
                        }
                    }
                }
            }

            return result.ToArray();
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
            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (field.Tag.OneOf(tags))
                    {
                        foreach (SubField subField in field.SubFields)
                        {
                            if (subField.Code.OneOf(codes)
                                && subField.Text.OneOf(values))
                            {
                                result.Add(field);
                                break;
                            }
                        }
                    }
                }
            }

            return result.ToArray();
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
            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (fieldPredicate(field))
                    {
                        foreach (SubField subField in field.SubFields)
                        {
                            if (subPredicate(subField))
                            {
                                result.Add(field);
                                break;
                            }
                        }
                    }
                }
            }

            return result.ToArray();
        }

        // ==========================================================

        /// <summary>
        /// Filter fields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetFieldRegex
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] string tagRegex
            )
        {
            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null)
                   && !string.IsNullOrEmpty(field.Tag))
                {
                    if (Regex.IsMatch(field.Tag, tagRegex))
                    {
                        result.Add(field);
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Filter fields.
        /// </summary>
        [CanBeNull]
        public static RecordField GetFieldRegex
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] string tagRegex,
                int occurrence
            )
        {
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null)
                    && !string.IsNullOrEmpty(field.Tag))
                {
                    if (Regex.IsMatch(field.Tag, tagRegex))
                    {
                        if (occurrence == 0)
                        {
                            return field;
                        }

                        occurrence--;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Filter fields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetFieldRegex
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] string[] tags,
                [NotNull] string textRegex
            )
        {
            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (field.Tag.OneOf(tags)
                        && !string.IsNullOrEmpty(field.Text))
                    {
                        if (Regex.IsMatch(field.Text, textRegex))
                        {
                            result.Add(field);
                        }
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Filter fields.
        /// </summary>
        [CanBeNull]
        public static RecordField GetFieldRegex
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] string[] tags,
                [NotNull] string textRegex,
                int occurrence
            )
        {
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (field.Tag.OneOf(tags)
                        && !string.IsNullOrEmpty(field.Text))
                    {
                        if (Regex.IsMatch(field.Text, textRegex))
                        {
                            if (occurrence == 0)
                            {
                                return field;
                            }

                            occurrence--;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Filter fields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetFieldRegex
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] string[] tags,
                [NotNull] char[] codes,
                [NotNull] string textRegex
            )
        {
            LocalList<RecordField> result = new LocalList<RecordField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (field.Tag.OneOf(tags))
                    {
                        foreach (SubField subField in field.SubFields)
                        {
                            if (subField.Code.OneOf(codes)
                                && !string.IsNullOrEmpty(subField.Text))
                            {
                                if (Regex.IsMatch(subField.Text, textRegex))
                                {
                                    result.Add(field);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Filter fields.
        /// </summary>
        [CanBeNull]
        public static RecordField GetFieldRegex
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] string[] tags,
                [NotNull] char[] codes,
                [NotNull] string textRegex,
                int occurrence
            )
        {
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (field.Tag.OneOf(tags))
                    {
                        foreach (SubField subField in field.SubFields)
                        {
                            if (subField.Code.OneOf(codes)
                                && !string.IsNullOrEmpty(subField.Text))
                            {
                                if (Regex.IsMatch(subField.Text, textRegex))
                                {
                                    if (occurrence == 0)
                                    {
                                        return field;
                                    }

                                    occurrence--;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return null;
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
            LocalList<string> result = new LocalList<string>();
            for (int i = 0; i < count; i++)
            {
                string text = fields[i].Text;
                if (!string.IsNullOrEmpty(text))
                {
                    result.Add(text);
                }
            }

            return result.ToArray();
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
            LocalList<string> result = new LocalList<string>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    string text = field.Text;
                    if (!string.IsNullOrEmpty(text))
                    {
                        result.Add(text);
                    }
                }
            }

            return result.ToArray();
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
            LocalList<SubField> result = new LocalList<SubField>();
            foreach (SubField subField in subFields)
            {
                if (!ReferenceEquals(subField, null))
                {
                    if (subField.Code.OneOf(codes))
                    {
                        result.Add(subField);
                    }
                }
            }

            return result.ToArray();
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
            LocalList<SubField> result = new LocalList<SubField>();
            for (int i = 0; i < count; i++)
            {
                if (subFields[i].Code.OneOf(codes))
                {
                    result.Add(subFields[i]);
                }
            }

            return result.ToArray();
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
            LocalList<SubField> result = new LocalList<SubField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    foreach (SubField subField in field.SubFields)
                    {
                        if (subField.Code.OneOf(codes))
                        {
                            result.Add(subField);
                        }
                    }
                }
            }

            return result.ToArray();
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
            LocalList<SubField> result = new LocalList<SubField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (field.Tag.SameString(tag))
                    {
                        foreach (SubField subField in field.SubFields)
                        {
                            if (subField.Code.SameChar(code))
                            {
                                result.Add(subField);
                            }
                        }
                    }
                }
            }

            return result.ToArray();
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
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (field.Tag.SameString(tag))
                    {
                        if (fieldOccurrence == 0)
                        {
                            foreach (SubField subField in field.SubFields)
                            {
                                if (subField.Code.SameChar(code))
                                {
                                    if (subOccurrence == 0)
                                    {
                                        return subField;
                                    }

                                    subOccurrence--;
                                }
                            }
                        }

                        fieldOccurrence--;
                    }
                }
            }

            return null;
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
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (field.Tag.SameString(tag))
                    {
                        foreach (SubField subField in field.SubFields)
                        {
                            if (subField.Code.SameChar(code))
                            {
                                if (occurrence == 0)
                                {
                                    return subField;
                                }

                                occurrence--;
                            }
                        }
                    }
                }
            }

            return null;
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
            LocalList<SubField> result = new LocalList<SubField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    foreach (SubField subField in field.SubFields)
                    {
                        if (subField.Code.SameChar(code))
                        {
                            result.Add(subField);
                        }
                    }
                }
            }

            return result.ToArray();
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
            LocalList<SubField> result = new LocalList<SubField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (fieldPredicate(field))
                    {
                        foreach (SubField subField in field.SubFields)
                        {
                            if (subPredicate(subField))
                            {
                                result.Add(subField);
                            }
                        }
                    }
                }
            }

            return result.ToArray();
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
            LocalList<SubField> result = new LocalList<SubField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (field.Tag.OneOf(tags))
                    {
                        foreach (SubField subField in field.SubFields)
                        {
                            if (subField.Code.OneOf(codes))
                            {
                                result.Add(subField);
                            }
                        }
                    }
                }
            }

            return result.ToArray();
        }

        // ==========================================================

        /// <summary>
        /// Filter subfields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] GetSubFieldRegex
            (
                [NotNull] this IEnumerable<SubField> subFields,
                [NotNull] string codeRegex
            )
        {
            LocalList<SubField> result = new LocalList<SubField>();
            foreach (SubField subField in subFields)
            {
                if (!ReferenceEquals(subField, null))
                {
                    if (Regex.IsMatch(codeRegex, subField.CodeString))
                    {
                        result.Add(subField);
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Filter subfields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] GetSubFieldRegex
            (
                [NotNull] this IEnumerable<SubField> subFields,
                [NotNull] char[] codes,
                [NotNull] string textRegex
            )
        {
            LocalList<SubField> result = new LocalList<SubField>();
            foreach (SubField subField in subFields)
            {
                if (!ReferenceEquals(subField, null))
                {
                    if (subField.Code.OneOf(codes)
                        && !string.IsNullOrEmpty(subField.Text))
                    {
                        if (Regex.IsMatch(subField.Text, textRegex))
                        {
                            result.Add(subField);
                        }
                    }
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// Filter subfields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SubField[] GetSubFieldRegex
            (
                [NotNull] this IEnumerable<RecordField> fields,
                [NotNull] string[] tags,
                [NotNull] char[] codes,
                [NotNull] string textRegex
            )
        {
            LocalList<SubField> result = new LocalList<SubField>();
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (field.Tag.OneOf(tags))
                    {
                        foreach (SubField subField in field.SubFields)
                        {
                            if (subField.Code.OneOf(codes)
                                && !string.IsNullOrEmpty(subField.Text))
                            {
                                if (Regex.IsMatch(subField.Text, textRegex))
                                {
                                    result.Add(subField);
                                }
                            }
                        }
                    }
                }
            }
            return result.ToArray();
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
            LocalList<string> result = new LocalList<string>();
            foreach (SubField subField in subFields)
            {
                if (!ReferenceEquals(subField, null))
                {
                    string text = subField.Text;
                    if (!string.IsNullOrEmpty(text))
                    {
                        result.Add(text);
                    }
                }
            }

            return result.ToArray();
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
            foreach (RecordField field in fields)
            {
                if (!ReferenceEquals(field, null))
                {
                    if (field.Tag.SameString(tag))
                    {
                        foreach (SubField subField in field.SubFields)
                        {
                            if (subField.Code.SameChar(code))
                            {
                                return subField.Text;
                            }
                        }
                    }
                }
            }

            return null;
        }

        // ==========================================================

        #endregion
    }
}
