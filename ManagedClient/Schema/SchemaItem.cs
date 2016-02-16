/* SchemaItem.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Schema
{
    public abstract class SchemaItem
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public virtual bool Validate()
        {
            return true;
        }

        #endregion
    }
}
