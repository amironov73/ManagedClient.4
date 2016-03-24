/* IrbisDate.cs -- строка с ИРБИС-датой
 */

#region Using directives

using System;
using System.Globalization;

using JetBrains.Annotations;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Строка с ИРБИС-датой yyyyMMdd.
    /// </summary>
    [PublicAPI]
    [Serializable]
    public sealed class IrbisDate
    {
        #region Constants

        /// <summary>
        /// Формат конверсии по умолчанию.
        /// </summary>
        public const string DefaultFormat = "yyyyMMdd";

        #endregion

        #region Properties

        /// <summary>
        /// Формат конверсии.
        /// </summary>
        public static string ConversionFormat = DefaultFormat;

        /// <summary>
        /// В виде текста.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// В виде даты.
        /// </summary>
        public DateTime Date { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор
        /// </summary>
        public IrbisDate
            (
                [NotNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("text");
            }

            Text = text;
            Date = ConvertStringToDate(text);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public IrbisDate
            (
                DateTime date
            )
        {
            Date = date;
            Text = ConvertDateToString(date);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Преобразование даты в строку.
        /// </summary>
        [NotNull]
        public static string ConvertDateToString
            (
                DateTime date
            )
        {
            return date.ToString(ConversionFormat);
        }

        /// <summary>
        /// Преобразование строки в дату.
        /// </summary>
        public static DateTime ConvertStringToDate
            (
                [NotNull] string date
            )
        {
            if (string.IsNullOrEmpty(date))
            {
                throw new ArgumentNullException("date");
            }

            DateTime result = DateTime.ParseExact
                (
                    date,
                    ConversionFormat,
                    CultureInfo.CurrentCulture
                );

            return result;
        }

        /// <summary>
        /// Неявное преобразование
        /// </summary>
        [NotNull]
        public static implicit operator IrbisDate
            (
                [NotNull] string text
            )
        {
            return new IrbisDate(text);
        }

        /// <summary>
        /// Неявное преобразование
        /// </summary>
        [NotNull]
        public static implicit operator IrbisDate
            (
                DateTime date
            )
        {
            return new IrbisDate(date);
        }

        /// <summary>
        /// Неявное преобразование
        /// </summary>
        [NotNull]
        public static implicit operator string 
            ( 
                [NotNull] IrbisDate date 
            )
        {
            if (ReferenceEquals(date, null))
            {
                throw new ArgumentNullException("date");
            }

            return date.Text;
        }

        /// <summary>
        /// Неявное преобразование
        /// </summary>
        public static implicit operator DateTime
            (
                [NotNull] IrbisDate date
            )
        {
            if (ReferenceEquals(date, null))
            {
                throw new ArgumentNullException("date");
            }

            return date.Date;
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            return Text;
        }

        #endregion
    }
}
