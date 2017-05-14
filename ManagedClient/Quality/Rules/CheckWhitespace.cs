// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CheckWhitespace.cs -- проверка употребления пробелов
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Quality.Rules
{
    /// <summary>
    /// Проверка употребления пробелов в полях/подполях
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class CheckWhitespace
        : IrbisRule
    {
        #region Private members

        #endregion

        #region IrbisRule members

        /// <inheritdoc cref="IrbisRule.FieldSpec"/>
        public override string FieldSpec
        {
            get { return "!100,330,905,907,919,920,3005"; }
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
                CheckWhitespace(field);
            }

            return EndCheck();
        }

        #endregion
    }
}
