/* RecordHandler.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Processing
{
    public delegate ProcessingResult RecordHandler
        (
            ProcessingContext context
        );
}
