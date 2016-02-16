/* PftSimpleFormat.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft.Ast
{
    /// <summary>
    /// Простой формат, т. е. состоящий из примитивных элементов.
    /// </summary>
    [Serializable]
    public sealed class PftSimpleFormat
        : PftAst
    {
        #region Properties

        #endregion

        #region Construction

        public PftSimpleFormat()
        {
        }

        public PftSimpleFormat
            (
                PftParser.SimpleFormatContext context
            )
        {
            DiscoverChildren(context);
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion
    }
}
