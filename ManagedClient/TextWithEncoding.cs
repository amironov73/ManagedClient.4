/* TextWithEncoding.cs
 */

#region Using directives

using System;
using System.Text;

#endregion

namespace ManagedClient
{
    [Serializable]
    public sealed class TextWithEncoding
        : IComparable<TextWithEncoding>
    {
        #region Properties

        public string Text { get; set; }

        public Encoding Encoding { get; set; }

        #endregion

        #region Construction

        public TextWithEncoding()
        {
        }

        public TextWithEncoding
            (
                string text
            )
        {
            Text = text;
            Encoding = Encoding.UTF8;
        }

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

        public int CompareTo
            (
                TextWithEncoding other
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

        private bool Equals(TextWithEncoding other)
        {
            return string.Equals(Text, other.Text);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is TextWithEncoding 
                && Equals((TextWithEncoding) obj);
        }

        public override int GetHashCode()
        {
            return (Text != null ? Text.GetHashCode() : 0);
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
