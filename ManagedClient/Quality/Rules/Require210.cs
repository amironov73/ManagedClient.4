// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Require210.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Quality.Rules
{
    /// <summary>
    /// Выходные данные
    /// </summary>
    public sealed class Require210
        : IrbisRule
    {
        #region Private members

        private void CheckField
            (
                RecordField field
            )
        {
            MustNotContainText(field);

            SubField city = field.GetFirstSubField('a');
            SubField publisher = field.GetFirstSubField('c');
            SubField year = field.GetFirstSubField('d');

            if (city != null)
            {
                if (Utilities.SafeContains(city.Text, ",", ";"))
                {
                    AddDefect
                        (
                            field,
                            city,
                            1,
                            "Несколько городов в одном подполе 210^a"
                        );
                }

                if (Utilities.SafeContains(city.Text, "б."))
                {
                    AddDefect
                        (
                            field,
                            city,
                            1,
                            "Город Б. М. в подполе 210^a"
                        );
                }
                else if (Utilities.SafeContains(city.Text, "."))
                {
                    AddDefect
                        (
                            field,
                            city,
                            1,
                            "Город с сокращением в подполе 210^a"
                        );
                }
            }

            if (publisher != null)
            {
                if (Utilities.SafeContains(publisher.Text, ",", ";"))
                {
                    AddDefect
                        (
                            field,
                            publisher,
                            1,
                            "Несколько издательств в одном подполе 210^c"
                        );
                }

                if (Utilities.SafeContains(publisher.Text, "б."))
                {
                    AddDefect
                        (
                            field,
                            publisher,
                            1,
                            "Издательство Б. И. в подполе 210^c"
                        );
                }
                else if (Utilities.SafeContains(publisher.Text, "."))
                {
                    AddDefect
                        (
                            field,
                            publisher,
                            1,
                            "Издательство с сокращением в подполе 210^c"
                        );
                }
            }

            if (year != null)
            {
                if (Utilities.SafeContains(year.Text, "б."))
                {
                    AddDefect
                        (
                            field,
                            year,
                            1,
                            "Год издания Б. Г. в подполе 210^d"
                        );
                }
            }

            if (field.HaveNotSubField('a') && field.HaveSubField('4'))
            {
                AddDefect
                    (
                        field,
                        1,
                        "Город введен в подполе 200^4: Город на издании"
                    );
            }
        }

        #endregion

        #region IrbisRule members

        public override string FieldSpec
        {
            get { return "210"; }
        }

        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            RecordField[] fields = GetFields();
            if (fields.Length == 0)
            {
                AddDefect
                    (
                        "210",
                        10,
                        "Отсутствует поле 210: Выходные данные"
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
