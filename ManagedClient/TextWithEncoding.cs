/* TextWithEncoding.cs
 */

#region Using directives

using System;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Текст с заданной кодировкой.
    /// </summary>
    [Serializable]
    public sealed class TextWithEncoding
        : IComparable<TextWithEncoding>
    {
        #region Properties

        /// <summary>
        /// Собственно текст.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Кодировка.
        /// </summary>
        public Encoding Encoding { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// Не заданы ни текст, ни кодировка.
        /// </summary>
        public TextWithEncoding()
        {
        }

        /// <summary>
        /// Текст с кодировкой UTF8.
        /// </summary>
        public TextWithEncoding
            (
                string text
            )
        {
            Text = text;
            Encoding = Encoding.UTF8;
        }

        /// <summary>
        /// Текст с кодировкой ANSI либо UTF8.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ansi"></param>
        public TextWithEncoding
            (
                string text,
                bool ansi
            )
        {
            Text = text;
            Encoding = ansi
                ? Encoding.Default
                : Encoding.UTF8;
        }

        /// <summary>
        /// Текст с явно заданной кодировкой.
        /// </summary>
        public TextWithEncoding
            (
                string text,
                Encoding encoding
            )
        {
            Text = text;
            Encoding = encoding;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Конверсия в байтовое представление.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            if (string.IsNullOrEmpty(Text))
            {
                return new byte[0];
            }
            Encoding encoding = Encoding 
                ?? Encoding.Default;
            return encoding.GetBytes(Text);
        }

        /// <summary>
        /// Неявное преобразование текста
        /// в текст с кодировкой.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static implicit operator TextWithEncoding
            (
                string text
            )
        {
            return new TextWithEncoding
                (
                    text
                );
        }

        #endregion

        #region Comparison

        /// <inheritdoc cref="IComparable{T}.CompareTo"/>
        public int CompareTo
            (
                [CanBeNull] TextWithEncoding other
            )
        {
            if (ReferenceEquals(other, null))
            {
                return 1;
            }

            return string.Compare
                (
                    Text,
                    other.Text,
                    StringComparison.CurrentCulture
                );
        }

        /// <summary>
        /// Оператор сравнения двух текстов.
        /// </summary>
        public static bool operator ==
            (
                TextWithEncoding left,
                TextWithEncoding right
            )
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }
            if (ReferenceEquals(right, null))
            {
                return false;
            }
            return left.Text == right.Text;
        }

        /// <summary>
        /// Оператор сравнения двух текстов.
        /// </summary>
        public static bool operator !=
            (
                TextWithEncoding left,
                TextWithEncoding right
            )
        {
            if (ReferenceEquals(left, null))
            {
                return !ReferenceEquals(right, null);
            }
            if (ReferenceEquals(right, null))
            {
                return true;
            }
            return left.Text != right.Text;
        }

        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        private bool Equals
            (
                TextWithEncoding other
            )
        {
            return string.Equals(Text, other.Text);
        }

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals
            (
                object obj
            )
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is TextWithEncoding 
                && Equals((TextWithEncoding) obj);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            return Text != null ? Text.GetHashCode() : 0;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return Text;
        }

        #endregion
    }
}
