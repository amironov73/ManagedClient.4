/* PftVariable.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Pft
{
    using Debugging;

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class PftVariable
    {
        #region Events

        /// <summary>
        /// Вызывается непосредственно перед считыванием значения.
        /// </summary>
        public event EventHandler<PftDebugEventArgs> BeforeReading;

        /// <summary>
        /// Вызывается непосредственно после модификации.
        /// </summary>
        public event EventHandler<PftDebugEventArgs> AfterModification;

        #endregion

        #region Properties

        // Имя переменной.
        public string Name { get; set; }

        /// <summary>
        /// Признак числовой переменной.
        /// </summary>
        public bool IsNumeric { get; set; }

        /// <summary>
        /// Числовое значение.
        /// </summary>
        public double NumericValue { get; set; }

        /// <summary>
        /// Строковое значение.
        /// </summary>
        public string StringValue { get; set; }

        #endregion

        #region Construction

        public PftVariable()
        {
        }

        public PftVariable
            (
                string name, 
                bool isNumeric
            )
        {
            Name = name;
            IsNumeric = isNumeric;
        }

        public PftVariable
            (
                string name, 
                double numericValue
            )
        {
            Name = name;
            IsNumeric = true;
            NumericValue = numericValue;
        }

        public PftVariable
            (
                string name, 
                string stringValue
            )
        {
            Name = name;
            StringValue = stringValue;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion
    }
}
