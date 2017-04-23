// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Require102.cs -- страна.
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
    /// Страна.
    /// </summary>
    public sealed class Require102
        : IrbisRule
    {
        #region Private members

        private IrbisMenu _menu;

        private void CheckField
            (
                RecordField field
            )
        {
            MustNotContainSubfields
                (
                    field
                );
            if (!CheckForMenu(_menu, field.Text))
            {
                AddDefect
                    (
                        field,
                        10,
                        "Поле 102 (страна) не из словаря"
                    );
            }
        }

        #endregion

        #region IrbisRule members

        public override string FieldSpec
        {
            get { return "102"; }
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
                        "102",
                        10,
                        "Не заполнено поле 102: Страна"
                    );
            }

            MustBeUniqueField
                (
                    fields
                );

            _menu = CacheMenu("str.mnu", _menu);
            foreach (RecordField field in fields)
            {
                CheckField(field);
            }

            return EndCheck();
        }

        #endregion
    }
}
