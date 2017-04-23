// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Require900.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Quality.Rules
{
    public sealed class Require900
        : IrbisRule
    {
        #region Private members

        #endregion

        #region IrbisRule members

        public override string FieldSpec
        {
            get { return "900"; }
        }

        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            return EndCheck();
        }

        #endregion
    }
}
