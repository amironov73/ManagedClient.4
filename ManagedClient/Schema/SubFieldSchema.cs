/* SubFieldSchema.cs
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
    public sealed class SubFieldSchema
        : SchemaItem
    {
        #region Properties

        public char Code { get; set; }

        public bool Mandatory { get; set; }

        public bool Repeatable { get; set; }

        public SchemaValidator<SubFieldSchema> Validator { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public static SubFieldSchema LoadFromFile
            (
                string fileName
            )
        {
            return null;
        }

        #endregion

        #region SchemaItem members

        public override bool Validate()
        {
            if (Validator == null)
            {
                return base.Validate();
            }
            return true;
        }

        #endregion
    }
}
