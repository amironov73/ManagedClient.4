// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RecordHandler.cs
 */

namespace ManagedClient.Processing
{
    /// <summary>
    /// Record handler delegate.
    /// </summary>
    /// <param name="context">Processing context.</param>
    /// <returns>Result of the record processing.</returns>
    public delegate ProcessingResult RecordHandler
        (
            ProcessingContext context
        );
}
