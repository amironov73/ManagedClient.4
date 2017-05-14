// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Require101.cs -- язык основного текста.
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Quality.Rules
{
    /// <summary>
    /// Язык основного текста.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Require101
        : IrbisRule
    {
        #region Private members

        private IrbisMenu _menu;

        private void CheckField
            (
                [NotNull] RecordField field
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
                        "Поле 101 (язык основного текста) не из словаря"
                    );
            }
        }

        #endregion

        #region IrbisRule members

        /// <inheritdoc cref="IrbisRule.FieldSpec"/>
        public override string FieldSpec { get { return "101"; } }

        /// <inheritdoc cref="IrbisRule.CheckRecord"/>
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
                        "101",
                        10,
                        "Не заполнено поле 101: Язык основного текста"
                    );
            }

            MustBeUniqueField
                (
                    fields
                );

            _menu = CacheMenu("jz.mnu", _menu);
            foreach (RecordField field in fields)
            {
                CheckField(field);
            }

            return EndCheck();
        }

        #endregion
    }
}
