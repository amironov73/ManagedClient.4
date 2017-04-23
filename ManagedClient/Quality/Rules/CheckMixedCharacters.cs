﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CheckMixedCharacters.cs -- проверка на смешение символов.
 */

#region Using directives

using System.Collections.Generic;
using System.Text.RegularExpressions;

#endregion

namespace ManagedClient.Quality.Rules
{
    /// <summary>
    /// Проверка на смешение символов.
    /// </summary>
    public sealed class CheckMixedCharacters
        : IrbisRule
    {
        #region Private members

        private readonly Regex _mixRegex = new Regex(@"\w+");

        private List<string> CheckText
            (
                string text
            )
        {
            List<string> result = new List<string>();

            if (!string.IsNullOrEmpty(text))
            {
                Match match = _mixRegex.Match(text);
                while (match.Success)
                {
                    CharacterClass classes = CharacterClassifier
                        .DetectCharacterClasses
                        (
                            match.Value
                        );
                    if (CharacterClassifier.IsBothCyrillicAndLatin(classes))
                    {
                        result.Add(match.Value);
                    }
                }
            }

            return result;
        }

        private static string FormatDefect
            (
                List<string> list
            )
        {
            string word = list.Count == 1
                ? "слове"
                : "словах";
            return string.Format
                (
                    "Смешение кириллицы и латиницы в {0}: {1}",
                    word,
                    string.Join(", ", list)
                );
        }

        private void CheckField
            (
                RecordField field
            )
        {
            List<string> result = CheckText(field.Text);
            if (result.Count != 0)
            {
                AddDefect
                    (
                        field,
                        15,
                        FormatDefect(result)
                    );
            }
        }

        private void CheckSubField
            (
                RecordField field,
                SubField subField
            )
        {
            List<string> result = CheckText(subField.Text);
            if (result.Count != 0)
            {
                AddDefect
                    (
                        field,
                        subField,
                        15,
                        FormatDefect(result)
                    );
            }
        }

        #endregion

        #region IrbisRule members

        public override string FieldSpec
        {
            get { return "*"; }
        }

        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            RecordField[] fields = GetFields();
            foreach (RecordField field in fields)
            {
                CheckField
                    (
                        field
                    );
                foreach (SubField subField in field.SubFields)
                {
                    CheckSubField
                        (
                            field,
                            subField
                        );
                }

            }

            return EndCheck();
        }

        #endregion
    }
}
