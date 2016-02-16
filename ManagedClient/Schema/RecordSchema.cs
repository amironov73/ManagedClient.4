/* RecordSchema.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Schema
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class RecordSchema
        : SchemaItem
    {
        #region Properties

        public SchemaValidator<RecordSchema> Validator { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public static RecordSchema LoadFromFile
            (
                string fileName
            )
        {
            return null;
        }

        #endregion
    }
}
