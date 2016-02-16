/* ProcessingResult.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Processing
{
    [Serializable]
    public sealed class ProcessingResult
    {
        #region Properties

        public bool Cancel { get; set; }

        public string Result { get; set; }

        #endregion
    }
}
