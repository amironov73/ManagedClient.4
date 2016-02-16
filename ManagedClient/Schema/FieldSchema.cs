/* FieldSchema.cs
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
    public sealed class FieldSchema
        : SchemaItem
    {
        #region Properties

        public char Tag { get; set; }

        public bool Mandatory { get; set; }

        public bool Repeatable { get; set; }

        public SchemaValidator<FieldSchema> Validator { get; set; }

        #endregion

        #region Construciton

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public static FieldSchema LoadFromFile
            (
                string fileName
            )
        {
            return null;
        }


        #endregion
    }
}
