// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Check10.cs -- ISBN и цена.
 */

#region Using directives

using System.Text.RegularExpressions;

#endregion

namespace ManagedClient.Quality.Rules
{
    /// <summary>
    /// ISBN и цена.
    /// </summary>
    public sealed class Check10
        : IrbisRule
    {
        #region Private members

        private void CheckField
            (
                RecordField field
            )
        {
            MustNotContainText(field);

            SubField isbn = field.GetFirstSubField('a');
            if (isbn != null)
            {
                if (Utilities.SafeContains(isbn.Text, "(", " ", ".", ";", "--"))
                {
                    AddDefect
                        (
                            field,
                            isbn,
                            1,
                            "Неверно введен ISBN в поле 10"
                        );
                }
            }

            SubField price = field.GetFirstSubField('d');
            if (price != null)
            {
                if (!Regex.IsMatch(price.Text, @"\d+\.\d{2}"))
                {
                    AddDefect
                        (
                            field,
                            price,
                            5,
                            "Неверный формат цены в поле 10"
                        );
                }
            }
        }

        #endregion

        #region IrbisRule members

        public override string FieldSpec
        {
            get { return "10"; }
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
                CheckField(field);
            }

            return EndCheck();
        }

        #endregion
    }
}
