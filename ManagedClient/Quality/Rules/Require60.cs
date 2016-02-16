/* Require60.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Quality.Rules
{
    public sealed class Require60
        : IrbisRule
    {
        #region Private members

        private IrbisMenu _menu;

        #endregion

        #region IrbisRule members

        public override string FieldSpec
        {
            get { return "60"; }
        }

        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            if (IsBook())
            {
                RecordField[] fields = GetFields();

                if (fields.Length == 0)
                {
                    AddDefect
                        (
                            "60",
                            3,
                            "Отстутсвует поле 60: Раздел знаний"
                        );
                }

                _menu = CacheMenu("rzn.mnu", _menu);


            }

            return EndCheck();
        }

        #endregion
    }
}
