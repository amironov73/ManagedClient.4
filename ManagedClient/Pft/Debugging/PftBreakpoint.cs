/* PftBreakpoint.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Pft.Debugging
{
    /// <summary>
    /// Точка останова (прерывания) PFT-скрипта.
    /// </summary>
    [Serializable]
    public sealed class PftBreakpoint
    {
        #region Properties

        /// <summary>
        /// Условие, при котором срабатывает точка останова.
        /// </summary>
        public string ConditionalExpression { get; set; }

        /// <summary>
        /// Выводится в консоль отладчика каждый раз при прохождении точки.
        /// </summary>
        public string TraceExpression { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion
    }
}
