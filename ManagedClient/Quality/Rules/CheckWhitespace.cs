// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CheckWhitespace.cs -- проверка употребления пробелов
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
    /// Проверка употребления пробелов в полях/подполях
    /// </summary>
    public sealed class CheckWhitespace
        : IrbisRule
    {
        #region Private members

        #endregion

        #region IrbisRule members

        public override string FieldSpec
        {
            get { return "!100,330,905,907,919,920,3005"; }
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
                CheckWhitespace(field);
            }

            return EndCheck();
        }

        #endregion
    }
}
