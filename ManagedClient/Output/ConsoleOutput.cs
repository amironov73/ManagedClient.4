/* ConsoleOutput.cs -- консольный вывод.
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Output
{
    /// <summary>
    /// Консольный вывод.
    /// </summary>
    public sealed class ConsoleOutput
        : AbstractOutput
    {
        #region AbstractOutput members

        public override bool HaveError { get; set; }

        public override AbstractOutput Clear()
        {
            HaveError = false;
            Console.Clear();
            return this;
        }

        public override AbstractOutput Configure
            (
                string configuration
            )
        {
            // TODO: implement properly
            return this;
        }

        public override AbstractOutput Write
            (
                string text
            )
        {
            System.Console.Write(text);
            return this;
        }

        public override AbstractOutput WriteError
            (
                string text
            )
        {
            HaveError = true;
            System.Console.Error.Write(text);
            return this;
        }

        #endregion
    }
}
