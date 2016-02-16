/* Check610.cs -- ключевые слова
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
    /// Ключевые слова.
    /// </summary>
    public sealed class Check610
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
            if (Utilities.SafeContains(text, "."))
            {
                AddDefect
                    (
                        field,
                        1,
                        "Ключевые слова с сокращениями"
                    );
            }
        }

        #endregion

        #region IrbisRule members

        public override string FieldSpec
        {
            get { return "610"; }
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
                        "610",
                        5,
                        "Отсутствуют ключевые слова: поле 610"
                    );
            }

            MustBeUniqueField
                (
                    fields
                );

            foreach (RecordField field in fields)
            {
                CheckField(field);
            }


            return EndCheck();
        }

        #endregion
    }
}
