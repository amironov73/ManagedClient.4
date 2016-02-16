/* IrbisDate.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Строка с Ирбис-датой yyyyMMdd.
    /// </summary>
    [Serializable]
    public sealed class IrbisDate
    {
        #region Properties

        public string AsString { get; set; }

        public DateTime AsDate { get; set; }

        public bool Valid { get; set; }

        #endregion

        #region Construction

        public IrbisDate()
        {
        }

        public IrbisDate(string asString)
        {
            AsString = asString;
        }

        public IrbisDate(DateTime asDate)
        {
            AsDate = asDate;
        }

        #endregion

        #region Public methods

        public static implicit operator IrbisDate
            (
                string text
            )
        {
            return null;
        }

        public static implicit operator IrbisDate
            (
                DateTime date
            )
        {
            return null;
        }

        public static implicit operator string 
            ( 
                IrbisDate date 
            )
        {
            return null;
        }

        public static implicit operator DateTime
            (
                IrbisDate date
            )
        {
            return DateTime.MinValue;
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" /> 
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> 
        /// that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("AsString: {0}", AsString);
        }

        #endregion
    }
}
