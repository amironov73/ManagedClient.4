﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Require700.cs
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
    /// Индивидуальные авторы.
    /// </summary>
    public sealed class Require700
        : IrbisRule
    {
        #region Private members

        #endregion

        #region IrbisRule members

        public override string FieldSpec
        {
            get { return "70[01]"; }
        }

        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            if (IsPazk())
            {
                RecordField[] fields = Record.Fields
                    .GetFieldBySpec("7[01][012]");
                if (fields.Length == 0)
                {
                    AddDefect
                        (
                            "700",
                            10,
                            "Отсутствуют сведения об авторах"
                        );
                }
                else
                {
                    if (Record.HaveField("700") && Record.HaveField("710"))
                    {
                        AddDefect
                            (
                                "700",
                                10,
                                "Одновременно присутствуют поля 700 и 710"
                            );
                    }
                }
            }

            return EndCheck();
        }

        #endregion
    }
}
