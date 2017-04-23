// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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
