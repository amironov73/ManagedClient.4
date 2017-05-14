// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Require200.cs
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Quality.Rules
{
    /// <summary>
    /// Область заглавия.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Require200
        : IrbisRule
    {
        #region Private members

        #endregion

        #region IrbisRule members

        /// <inheritdoc cref="IrbisRule.FieldSpec"/>
        public override string FieldSpec
        {
            get { return "200"; }
        }

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
                        "200",
                        10,
                        "Не заполнено поле 200: Заглавие"
                    );
            }
            else if (fields.Length != 1)
            {
                AddDefect
                    (
                        "200",
                        10,
                        "Повторяется поле 200: Заглавие"
                    );
            }
            else
            {
                RecordField field = fields[0];
                if (IsSpec())
                {
                    if (field.HaveNotSubField('v'))
                    {
                        AddDefect
                            (
                                field,
                                10,
                                "Отсутствует подполе 200^v: "
                                + "Обозначение и номер тома"
                            );
                    }
                }
                else
                {
                    if (field.HaveSubField('v'))
                    {
                        AddDefect
                            (
                                field,
                                10,
                                "Присутствует подполе 200^v: "
                                + "Обозначение и номер тома"                                
                            );
                    }
                    if (field.HaveNotSubField('a'))
                    {
                        AddDefect
                            (
                                field,
                                10,
                                "Отсутстсвует подполе 200^a: Заглавие"
                            );
                    }
                }
            }

            return EndCheck();
        }

        #endregion
    }
}
