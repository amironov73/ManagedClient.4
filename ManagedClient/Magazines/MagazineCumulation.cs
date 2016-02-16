/* MagazineCumulation.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Magazines
{
    /// <summary>
    /// Данные о кумуляции номеров. Поле 909.
    /// </summary>
    [Serializable]
    public sealed class MagazineCumulation
    {
        #region Properties

        /// <summary>
        /// Год. Подполе Q.
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// Том. Подполе F.
        /// </summary>
        public string Volume { get; set; }

        /// <summary>
        /// Место хранения. Подполе D.
        /// </summary>
        public string Place { get; set; }

        /// <summary>
        /// Кумулированные номера. Подполе H.
        /// </summary>
        public string Numbers { get; set; }

        /// <summary>
        /// Номер комплекта. Подполе K.
        /// </summary>
        public string Complect { get; set; }

        #endregion

        #region Construciton

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public static MagazineCumulation Parse
            (
                RecordField field
            )
        {
            MagazineCumulation result = new MagazineCumulation
            {
                Year = field.GetFirstSubFieldText('q'),
                Volume = field.GetFirstSubFieldText('f'),
                Place = field.GetFirstSubFieldText('d'),
                Numbers = field.GetFirstSubFieldText('h'),
                Complect = field.GetFirstSubFieldText('k')
            };

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
