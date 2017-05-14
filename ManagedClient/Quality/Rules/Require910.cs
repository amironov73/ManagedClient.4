// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Require910.cs -- сведения об экземплярах.
 */

#region Using directives

using JetBrains.Annotations;

using ManagedClient.Fields;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Quality.Rules
{
    /// <summary>
    /// Сведения об экземплярах.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Require910
        : IrbisRule
    {
        #region Private members

        private IrbisMenu _statusMenu;
        private IrbisMenu _placeMenu;

        private void CheckField
            (
                [NotNull] RecordField field
            )
        {
            ExemplarInfo exemplar = ExemplarInfo.Parse(field);

            if (!CheckForMenu(_statusMenu, exemplar.Status))
            {
                AddDefect
                    (
                        field,
                        10,
                        "Статус экземпляра не из словаря"
                    );
            }
            if (!CheckForMenu(_placeMenu, exemplar.Place))
            {
                AddDefect
                    (
                        field,
                        10,
                        "Место хранения не из словаря"
                    );
            }
            if (string.IsNullOrEmpty(exemplar.Number))
            {
                AddDefect
                    (
                        field,
                        10,
                        "Не задан номер экземпляра"
                    );
            }
        }

        #endregion

        #region IrbisRule members

        /// <inheritdoc cref="IrbisRule.FieldSpec"/>
        public override string FieldSpec
        {
            get { return "910"; }
        }

        /// <inheritdoc cref="IrbisRule.CheckRecord"/>
        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            if (!IsBook())
            {
                goto DONE;
            }

            RecordField[] fields = GetFields();
            if (fields.Length == 0)
            {
                AddDefect
                    (
                        "910",
                        10,
                        "Нет сведений об экземплярах: поле 910"
                    );
            }

            _statusMenu = CacheMenu("ste.mnu", _statusMenu);
            _placeMenu = CacheMenu("mhr.mnu", _placeMenu);

            foreach (RecordField field in fields)
            {
                CheckField(field);
            }

            MustBeUniqueSubfield
                (
                    fields,
                    'b'
                );

            DONE: return EndCheck();
        }

        #endregion
    }
}
