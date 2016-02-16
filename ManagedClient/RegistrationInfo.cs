/* RegistrationInfo.cs
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
    /// Информация о (пере)регистрации читателя.
    /// </summary>
    [Serializable]
    public sealed class RegistrationInfo
    {
        #region Properties

        /// <summary>
        /// Дата. Подполе *
        /// </summary>
        public string DateString { get; set; }

        public DateTime Date { get { return DateString.ParseIrbisDate(); } }

        /// <summary>
        /// Место (пере)регистрации. Подполе C.
        /// </summary>
        public string Place { get; set; }

        #endregion

        #region Private members

        private static string FM
            (
                RecordField field,
                char code
            )
        {
            return field.GetSubFieldText(code, 0);
        }

        #endregion

        #region Public methods

        public static RegistrationInfo Parse(RecordField field)
        {
            RegistrationInfo result = new RegistrationInfo
            {
                DateString = field.Text,
                Place = FM(field,'c')
            };

            return result;
        }

        #endregion
    }
}
