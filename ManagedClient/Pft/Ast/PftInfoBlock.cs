/* PftInfoBlock.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Pft.Ast
{
    public class PftInfoBlock
        : PftAst
    {
        #region Properties

        public string Author { get; set; }

        public string Title { get; set; }

        public string Version { get; set; }

        public PftOutputFormat Format { get; set; }

        #endregion

        #region Construction

        public PftInfoBlock()
        {
        }

        public PftInfoBlock
            (
                PftParser.PftInfoBlockContext context
            )
        {
            //context.authorInfo()
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion
    }
}
