// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Check908.cs
 */

#region Using directives

using System.Text.RegularExpressions;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Quality.Rules
{
    /// <summary>
    /// Авторский знак
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Check908
        : IrbisRule
    {
        #region Private members

        private void CheckField
            (
                [NotNull] RecordField field
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
                bool isGood = firstLetter >= 'A' && firstLetter <= 'Z'
                              || firstLetter >= 'А' && firstLetter <= 'Я';
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

                    if (firstLetter >= 'A' && firstLetter <= 'Z')
                    {
                        regex = @"[A-Z]\d{2}";
                    }
                    if (firstLetter == 'З' || firstLetter == 'О'
                        || firstLetter == 'Ч')
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

        /// <inheritdoc cref="IrbisRule.FieldSpec"/>
        public override string FieldSpec
        {
            get { return "908"; }
        }

        /// <inheritdoc cref="IrbisRule.CheckRecord"/>
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
