﻿/* Check908.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

#endregion

namespace ManagedClient.Quality.Rules
{
    /// <summary>
    /// Авторский знак
    /// </summary>
    public sealed class Check908
        : IrbisRule
    {
        #region Private members

        private void CheckField
            (
                RecordField field
            )
        {
            MustNotContainSubfields(field);
            string text = field.Text;
            if (string.IsNullOrEmpty(text))
            {
                AddDefect
                    (
                        field,
                        5,
                        "Неверный формат поля 908: Авторский знак"
                    );
            }
            else
            {
                char firstLetter = text[0];
                bool isGood = ((firstLetter >= 'A') && (firstLetter <= 'Z'))
                              || ((firstLetter >= 'А') && (firstLetter <= 'Я'));
                if (!isGood)
                {
                    AddDefect
                        (
                            field,
                            1,
                            "Неверный формат поля 908: Авторский знак"
                        );
                }
                else
                {
                    string regex = @"[А-Я]\s\d{2}";

                    if ((firstLetter >= 'A') && (firstLetter <= 'Z'))
                    {
                        regex = @"[A-Z]\d{2}";
                    }
                    if ((firstLetter == 'З') || (firstLetter == 'О')
                        || (firstLetter == 'Ч'))
                    {
                        regex = @"[ЗОЧ]-\d{2}";
                    }

                    if (!Regex.IsMatch(text, regex))
                    {
                        AddDefect
                            (
                                field,
                                1,
                                "Неверный формат поля 908: Авторский знак"
                            );
                    }
                }
            }
        }

        #endregion

        #region IrbisRule members

        public override string FieldSpec
        {
            get { return "908"; }
        }

        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            RecordField[] fields = GetFields();
            if (fields.Length > 1)
            {

                AddDefect
                    (
                        "908",
                        5,
                        "Повторяется поле 908: Авторский знак"
                    );
            }
            foreach (RecordField field in fields)
            {
                CheckField(field);
            }

            return EndCheck();
        }

        #endregion
    }
}
