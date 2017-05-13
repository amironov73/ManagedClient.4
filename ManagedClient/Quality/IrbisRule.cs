// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisRule.cs -- абстрактный базовый класс для правил.
 */

#region Using directives

using System;
using System.Linq;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Quality
{
    /// <summary>
    /// Абстрактный базовый класс для правил.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class IrbisRule
    {
        #region Properties

        ///// <summary>
        ///// Заголовок правила.
        ///// </summary>
        //public abstract string Title { get; }

        /// <summary>
        /// Затрагиваемые поля.
        /// </summary>
        [CanBeNull]
        public abstract string FieldSpec { get; }

        /// <summary>
        /// Client connection.
        /// </summary>
        [NotNull]
        public ManagedClient64 Client { get { return _context.Client; } }

        /// <summary>
        /// Текущий контекст.
        /// </summary>
        [NotNull]
        public RuleContext Context { get { return _context; } }

        /// <summary>
        /// Текущая проверяемая запись.
        /// </summary>
        [NotNull]
        public IrbisRecord Record { get { return _context.Record; } }

        /// <summary>
        /// Накопленный отчёт.
        /// </summary>
        [NotNull]
        public RuleReport Report { get { return _report; } }

        /// <summary>
        /// Рабочий лист.
        /// </summary>
        [CanBeNull]
        public string Worksheet
        {
            get { return Record.FM("920"); }
        }

        #endregion

        #region Private members

        /// <summary>
        /// Context.
        /// </summary>
        [CLSCompliant(false)]
        // ReSharper disable InconsistentNaming
        protected RuleContext _context;
        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Report.
        /// </summary>
        [CLSCompliant(false)]
        // ReSharper disable InconsistentNaming
        protected RuleReport _report;
        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Add the detected defect.
        /// </summary>
        protected void AddDefect
            (
                [NotNull] string tag,
                int damage,
                [NotNull] string format,
                params object[] args
            )
        {
            FieldDefect defect = new FieldDefect
            {
                Field = tag,
                Damage = damage,
                Message = string.Format(format, args)
            };
            Report.Defects.Add(defect);
        }

        /// <summary>
        /// Add the detected defect.
        /// </summary>
        protected void AddDefect
            (
                [NotNull] RecordField field,
                int damage,
                [NotNull] string format,
                params object[] args
            )
        {
            FieldDefect defect = new FieldDefect
            {
                Field = field.Tag,
                FieldRepeat = field.Repeat,
                Value = field.ToText(),
                Damage = damage,
                Message = string.Format(format, args)
            };
            Report.Defects.Add(defect);
        }

        /// <summary>
        /// Add the detected defect.
        /// </summary>
        protected void AddDefect
            (
                [NotNull] RecordField field,
                [NotNull] SubField subfield,
                int damage,
                [NotNull] string format,
                params object[] args
            )
        {
            FieldDefect defect = new FieldDefect
            {
                Field = field.Tag,
                FieldRepeat = field.Repeat,
                Subfield = subfield.Code.ToString(),
                Value = subfield.Text,
                Damage = damage,
                Message = string.Format(format, args)
            };
            Report.Defects.Add(defect);
        }

        /// <summary>
        /// Begin the record checking.
        /// </summary>
        protected void BeginCheck
            (
                [NotNull] RuleContext context
            )
        {
            _context = context;
            _report = new RuleReport();
        }

        /// <summary>
        /// Cache the menu for later using.
        /// </summary>
        [NotNull]
        protected IrbisMenu CacheMenu
            (
                [NotNull] string name,
                [CanBeNull] IrbisMenu menu
            )
        {
            menu = menu ?? IrbisMenu.Read(Client, name);

            return menu;
        }

        /// <summary>
        /// Check the value against the menu.
        /// </summary>
        protected bool CheckForMenu
            (
                [CanBeNull] IrbisMenu menu,
                [CanBeNull] string value
            )
        {
            if (ReferenceEquals(menu, null))
            {
                return true;
            }
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }

            IrbisMenu.Entry entry = menu.GetEntrySensitive(value);

            return entry != null;
        }

        /// <summary>
        /// Get text from specified position of the string.
        /// </summary>
        [NotNull]
        protected static string GetTextAtPosition
            (
                [NotNull] string text,
                int position
            )
        {
            int length = text.Length;
            int start = Math.Max(0, position - 1);
            int stop = Math.Min(length - 1, position + 2);
            while (start >= 0 && text[start] == ' ')
            {
                start--;
            }
            while (start >= 0 && text[start] != ' ')
            {
                start--;
            }
            start = Math.Max(0, start);
            while (stop < length && text[stop] == ' ')
            {
                stop++;
            }
            while (stop < length && text[stop] != ' ')
            {
                stop++;
            }
            stop = Math.Min(length - 1, stop);

            return text.Substring
            (
                start,
                stop - start + 1
            )
            .Trim();
        }

        /// <summary>
        /// Show double whitespace in the text.
        /// </summary>
        [NotNull]
        protected static string ShowDoubleWhiteSpace
            (
                [NotNull] string text
            )
        {
            int position = text.IndexOf
                (
                    "  ",
                    StringComparison.Ordinal
                );

            return GetTextAtPosition
                (
                    text,
                    position
                );
        }

        /// <summary>
        /// Check whether the subfield is whitespace.
        /// </summary>
        protected void CheckWhitespace
            (
                [NotNull] RecordField field,
                [NotNull] SubField subfield
            )
        {
            string text = subfield.Text;

            if (string.IsNullOrEmpty(text))
            {
                AddDefect
                    (
                        field,
                        subfield,
                        1,
                        "Пустое подполе {0}^{1}",
                        field.Tag,
                        subfield.Code
                    );

                return;
            }

            if (text.StartsWith(" "))
            {
                AddDefect
                    (
                        field,
                        subfield,
                        1,
                        "Подполе {0}^{1} начинается с пробела",
                        field.Tag,
                        subfield.Code
                    );
            }

            if (text.EndsWith(" "))
            {
                AddDefect
                    (
                        field,
                        subfield,
                        1,
                        "Подполе {0}^{1} оканчивается пробелом",
                        field.Tag,
                        subfield.Code
                    );
            }

            if (text.Contains("  "))
            {
                AddDefect
                    (
                        field,
                        subfield,
                        1,
                        "Подполе {0}^{1} содержит двойной пробел: {2}",
                        field.Tag,
                        subfield.Code,
                        ShowDoubleWhiteSpace(text)
                    );
            }
        }

        /// <summary>
        /// Check whether the field is whitespace.
        /// </summary>
        protected void CheckWhitespace
            (
                [NotNull] RecordField field
            )
        {
            string text = field.Text;
            if (!string.IsNullOrEmpty(text))
            {
                if (text.StartsWith(" "))
                {
                    AddDefect
                        (
                            field,
                            1,
                            "Поле {0} начинается с пробела",
                            field.Tag
                        );
                }
                if (text.EndsWith(" "))
                {
                    AddDefect
                        (
                            field,
                            1,
                            "Поле {0} оканчивается пробелом",
                            field.Tag
                        );
                }
                if (text.Contains("  "))
                {
                    AddDefect
                        (
                            field,
                            1,
                            "Поле {0} содержит двойной пробел: {1}",
                            field.Tag,
                            ShowDoubleWhiteSpace(text)
                        );
                }
            }

            foreach (SubField subfield in field.SubFields)
            {
                CheckWhitespace
                    (
                        field,
                        subfield
                    );
            }
        }

        /// <summary>
        /// End the record checking.
        /// </summary>
        protected RuleReport EndCheck()
        {
            Report.Damage = Report.Defects
                .Sum(defect => defect.Damage);

            return Report;
        }

        /// <summary>
        /// Whether the working list is ASP?
        /// </summary>
        protected bool IsAsp()
        {
            return Worksheet.SameString("ASP");
        }

        /// <summary>
        /// Whether the working list means book?
        /// </summary>
        /// <returns></returns>
        protected bool IsBook()
        {
            string worksheet = Worksheet;
            return worksheet.SameString("PAZK")
                   || worksheet.SameString("SPEC")
                   || worksheet.SameString("PVK");
        }

        /// <summary>
        /// Whether the working list is PAZK?
        /// </summary>
        protected bool IsPazk()
        {
            return Worksheet.SameString("PAZK");
        }

        /// <summary>
        /// Whether the working list is SPEC.
        /// </summary>
        protected bool IsSpec()
        {
            return Worksheet.SameString("SPEC");
        }

        /// <summary>
        /// Get fields from the record by the specification.
        /// </summary>
        protected RecordField[] GetFields()
        {
            return Record.Fields
                .GetFieldBySpec(FieldSpec);
        }

        /// <summary>
        /// Asserts that the field must not contains subfields.
        /// </summary>
        protected void MustNotContainSubfields
            (
                [NotNull] RecordField field
            )
        {
            if (field.SubFields.Count != 0)
            {
                AddDefect
                    (
                        field,
                        20,
                        "Поле {0} содержит подполя",
                        field.Tag
                    );
            }
        }

        /// <summary>
        /// Asserts that the field must not contains text.
        /// </summary>
        protected void MustNotContainText
            (
                [NotNull] RecordField field
            )
        {
            if (!string.IsNullOrEmpty(field.Text))
            {
                AddDefect
                    (
                        field,
                        20,
                        "Поле {0} должно состоять только из подполей",
                        field.Tag
                    );
            }
        }

        /// <summary>
        /// Asserts that the field must not contain repeatable subfields.
        /// </summary>
        protected void MustNotRepeatSubfields
            (
                [NotNull] RecordField field
            )
        {
            var grouped = field.SubFields
                .GroupBy(sf => sf.CodeString.ToLowerInvariant());
            foreach (var grp in grouped)
            {
                if (grp.Count() != 1)
                {
                    AddDefect
                        (
                            field,
                            20,
                            "Подполе {0}^{1} повторяется",
                            field.Tag,
                            grp.Key
                        );
                }
            }
        }

        /// <summary>
        /// Asserts that the specified fields must have unique values.
        /// </summary>
        protected void MustBeUniqueField
            (
                [NotNull] RecordField[] fields
            )
        {
            var grouped = fields.GroupBy
                (
                    f => f.Text
                        .ThrowIfNull("field.Text")
                        .ToLowerInvariant()
                );
            foreach (var grp in grouped)
            {
                if (grp.Count() != 1)
                {
                    AddDefect
                        (
                            grp.First(),
                            20,
                            "Поле {0} содержит повторяющееся значение {1}",
                            grp.First().Tag,
                            grp.Key
                        );
                }
            }
        }

        /// <summary>
        /// Subfield of the field mus be non-empty.
        /// </summary>
        protected void MustBeNonEmptySubfield
            (
                [NotNull] RecordField field,
                char code
            )
        {
            var selected = field.SubFields
                .GetSubField(new[] { code })
                .Where(sf => string.IsNullOrEmpty(sf.Text));
            foreach (SubField subField in selected)
            {
                AddDefect
                    (
                        field,
                        subField,
                        5,
                        "Подполе {0}^{1} пустое",
                        field.Tag,
                        subField.Code
                    );
            }
        }

        /// <summary>
        /// Subfields of the fields must be unique.
        /// </summary>
        protected void MustBeUniqueSubfield
            (
                [NotNull] RecordField[] fields,
                char code
            )
        {
            var grouped = fields
                .SelectMany(f => f.SubFields)
                .GetSubField(new[] { code })
                .GroupBy(sf => sf.Text.ToLowerInvariant());
            foreach (var grp in grouped)
            {
                if (grp.Count() != 1)
                {
                    AddDefect
                        (
                            fields[0],
                            grp.First(),
                            5,
                            "Подполе {0}^{1} содержит"
                            + " неуникальное значение {2}",
                            fields[0].Tag,
                            grp.First().Code,
                            grp.Key
                        );
                }
            }
        }

        /// <summary>
        /// Subfields of the field must be unique.
        /// </summary>
        protected void MustBeUniqueSubfield
            (
                [NotNull] RecordField[] fields,
                params char[] codes
            )
        {
            foreach (char code in codes)
            {
                MustBeUniqueSubfield
                    (
                        fields,
                        code
                    );
            }
        }

        /// <summary>
        /// The field must not contain whitespace.
        /// </summary>
        /// <param name="field"></param>
        protected void MustNotContainWhitespace
            (
                [NotNull] RecordField field
            )
        {
            string text = field.Text;
            if (!string.IsNullOrEmpty(text)
                && text.ContainsWhitespace())
            {
                AddDefect
                    (
                        field,
                        3,
                        "Поле {0} содержит пробельные символы",
                        field.Tag
                    );
            }
        }

        /// <summary>
        /// The subfield must not contain whitespace.
        /// </summary>
        protected void MustNotContainWhitespace
            (
                [NotNull] RecordField field,
                [NotNull] SubField subField
            )
        {
            string text = subField.Text;
            if (!string.IsNullOrEmpty(text)
                && text.ContainsWhitespace())
            {
                AddDefect
                    (
                        field,
                        subField,
                        3,
                        "Подполе {0}^{1} содержит пробельные символы",
                        field.Tag,
                        subField.Code
                    );
            }
        }

        /// <summary>
        /// Subfields of the field must not contain bad characters.
        /// </summary>
        protected void MustNotContainWhitespace
            (
                [NotNull] RecordField field,
                params char[] codes
            )
        {
            foreach (char code in codes)
            {
                SubField[] subFields = field.GetSubField(code);
                foreach (SubField subField in subFields)
                {
                    MustNotContainWhitespace
                        (
                            field,
                            subField
                        );
                }
            }
        }

        /// <summary>
        /// The field must not contain bad characters.
        /// </summary>
        protected void MustNotContainBadCharacters
            (
                [NotNull] RecordField field
            )
        {
            string text = field.Text;
            if (!string.IsNullOrEmpty(text))
            {
                int position = RuleUtility.BadCharacterPosition(text);
                if (position >= 0)
                {
                    AddDefect
                        (
                            field,
                            3,
                            "Поле {0} содержит запрещённые символы: {1}",
                            GetTextAtPosition(text, position)
                        );
                }
            }
        }

        /// <summary>
        /// The subfield must not contain bad characters.
        /// </summary>
        protected void MustNotContainBadCharacters
            (
                [NotNull] RecordField field,
                [NotNull] SubField subField
            )
        {
            string text = subField.Text;
            if (!string.IsNullOrEmpty(text))
            {
                int position = RuleUtility.BadCharacterPosition(text);
                if (position >= 0)
                {
                    AddDefect
                        (
                            field,
                            subField,
                            3,
                            "Подполе {0}^{1} содержит"
                            + "запрещённые символы: {2}",
                            field.Tag,
                            subField.Code,
                            GetTextAtPosition(text, position)
                        );
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Проверка записи.
        /// </summary>
        [NotNull]
        public abstract RuleReport CheckRecord
            (
                [NotNull] RuleContext context
            );

        #endregion
    }
}
