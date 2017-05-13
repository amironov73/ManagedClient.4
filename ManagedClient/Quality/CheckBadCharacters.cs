// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CheckBadCharacters.cs -- проверка на плохие символы
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Quality
{
    /// <summary>
    /// Проверка на плохие символы.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class CheckBadCharacters
        : IrbisRule
    {
        #region Private members

        #endregion

        #region IrbisRule members

        /// <inheritdoc cref="IrbisRule.FieldSpec"/>
        public override string FieldSpec
        {
            get { return "!3005"; }
        }

        /// <inheritdoc cref="IrbisRule.CheckRecord"/>
        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            RecordField[] fields = GetFields();
            foreach (RecordField field in fields)
            {
                MustNotContainBadCharacters(field);
                foreach (SubField subField in field.SubFields)
                {
                    MustNotContainBadCharacters
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
