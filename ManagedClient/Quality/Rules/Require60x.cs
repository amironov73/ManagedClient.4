﻿/* Require60x.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Quality.Rules
{
    public sealed class Require60x
        : IrbisRule
    {
        #region Private members

        #endregion

        #region IrbisRule members

        public override string FieldSpec
        {
            get { throw new NotImplementedException(); }
        }

        public override RuleReport CheckRecord(RuleContext context)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
