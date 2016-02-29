/* CheckBadCharacters.cs -- проверка на плохие символы
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Quality
{
    /// <summary>
    /// Проверка на плохие символы.
    /// </summary>
    public sealed class CheckBadCharacters
        : IrbisRule
    {
        #region Private members

        #endregion

        #region IrbisRule members

        public override string FieldSpec
        {
            get { return "!3005"; }
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
                MustNotContainBadCharacters
                    (
                        field
                    );
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
