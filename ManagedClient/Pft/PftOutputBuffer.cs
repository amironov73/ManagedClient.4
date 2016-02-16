/* PftOutputBuffer.cs
 */

#region Using directives

using System.IO;
using System.Text;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// �������� ������ ����������.
    /// </summary>
    public sealed class PftOutputBuffer
    {
        #region Properties

        /// <summary>
        /// ������������ �����. ����� ���� <c>null</c>.
        /// </summary>
        public PftOutputBuffer Parent { get { return _parent; } }

        /// <summary>
        /// �������� (�������) �����.
        /// </summary>
        public TextWriter Normal { get { return _normal; } }

        /// <summary>
        /// ����� ��������������.
        /// </summary>
        public TextWriter Warning { get { return _warning; } }

        /// <summary>
        /// ����� ������.
        /// </summary>
        public TextWriter Error { get { return _error; } }

        /// <summary>
        /// ����������� ����� ��������� ������.
        /// </summary>
        public string Text { get { return Normal.ToString(); } }

        /// <summary>
        /// ����������� ����� ������ ��������������.
        /// </summary>
        public string WarningText { get { return Warning.ToString(); } }

        /// <summary>
        /// ����������� ����� ������ ������.
        /// </summary>
        public string ErrorText { get { return Error.ToString(); } }

        /// <summary>
        /// �������� �� ����� � �������� ������?
        /// </summary>
        public bool HaveText { get { return _HaveText(_normal); } }

        /// <summary>
        /// ���� �� ��������������?
        /// </summary>
        public bool HaveWarning { get { return _HaveText(_warning); } }

        /// <summary>
        /// ���� �� ������?
        /// </summary>
        public bool HaveError { get { return _HaveText(_error); } }

        #endregion

        #region Construction

        public PftOutputBuffer()
            : this (null)
        {
        }

        public PftOutputBuffer
            (
               PftOutputBuffer parent
            )
        {
            _parent = parent;
            _normal = new StringWriter();
            _warning = new StringWriter();
            _error = new StringWriter();
        }

        #endregion

        #region Private members

        private readonly PftOutputBuffer _parent;

        private readonly StringWriter _normal;
        private readonly StringWriter _warning;
        private readonly StringWriter _error;

        private static bool _HaveText
            (
               StringWriter writer
            )
        {
            return (writer.GetStringBuilder().Length != 0);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// ������� ��������� ������.
        /// </summary>
        /// <returns></returns>
        public PftOutputBuffer ClearText()
        {
            _normal.GetStringBuilder().Length = 0;
            return this;
        }

        /// <summary>
        /// �������� ������ ��������������.
        /// </summary>
        /// <returns></returns>
        public PftOutputBuffer ClearWarning()
        {
            _warning.GetStringBuilder().Length = 0;
            return this;
        }

        /// <summary>
        /// ������� ������ ������.
        /// </summary>
        /// <returns></returns>
        public PftOutputBuffer ClearError()
        {
            _error.GetStringBuilder().Length = 0;
            return this;
        }

        /// <summary>
        /// ��������� ������� � ������ ������.
        /// </summary>
        /// <returns></returns>
        public PftOutputBuffer Push()
        {
            PftOutputBuffer result = new PftOutputBuffer(this);
            return result;
        }

        /// <summary>
        /// ������� � ������� ������ � ������������
        /// � ����� ������, ������������ � �����
        /// ���������� ������.
        /// </summary>
        /// <returns></returns>
        public string Pop()
        {
            if (!ReferenceEquals(Parent, null))
            {
                string warningText = WarningText;
                if (!string.IsNullOrEmpty(warningText))
                {
                    Parent.Warning.Write(warningText);
                }

                string errorText = ErrorText;
                if (!string.IsNullOrEmpty(errorText))
                {
                    Parent.Error.Write(errorText);
                }
            }

            return ToString();
        }

        public PftOutputBuffer Write
            (
                string format,
                params object[] arg
            )
        {
            if (!string.IsNullOrEmpty(format))
            {
                Normal.Write(format, arg);
            }
            return this;
        }

        public PftOutputBuffer Write
            (
                string value
            )
        {
            if (!string.IsNullOrEmpty(value))
            {
                Normal.Write(value);
            }
            return this;
        }

        public PftOutputBuffer WriteLine
            (
                string format,
                params object[] arg
            )
        {
            if (!string.IsNullOrEmpty(format))
            {
                Normal.WriteLine(format, arg);
            }
            return this;
        }

        public PftOutputBuffer WriteLine
            (
               string value
            )
        {
            if (!string.IsNullOrEmpty(value))
            {
                Normal.WriteLine(value);
            }
            return this;
        }

        public PftOutputBuffer WriteLine()
        {
            Normal.WriteLine();
            return this;
        }

        /// <summary>
        /// �������� (������������) ������� ������� �� �����������.
        /// </summary>
        /// <returns></returns>
        public int GetCaretPosition()
        {
            StringBuilder builder = _normal.GetStringBuilder();
            int pos;
            for (pos = builder.Length - 1; pos >= 0; pos--)
            {
                if (builder[pos] == '\n')
                    break;
            }
            return (builder.Length - pos);
        }

        /// <summary>
        /// ������� ��������� ������ � ������, ���� ��� ������.
        /// </summary>
        public void RemoveEmptyLine()
        {
            StringBuilder builder = _normal.GetStringBuilder();
            int pos;
            for (pos = builder.Length - 1; pos >= 0; pos-- )
            {
                if (!char.IsWhiteSpace(builder[pos]))
                {
                    break;
                }
                builder.Length = pos;
            }
        }

        /// <summary>
        /// ������ �� ��������� ������ � �������� ������?
        /// </summary>
        /// <returns></returns>
        public bool HaveEmptyLine()
        {
            StringBuilder builder = _normal.GetStringBuilder();
            bool result = true;
            int pos;
            for (pos = builder.Length - 1; pos >= 0; pos--)
            {
                char c = builder[pos];
                if (c == '\n')
                {
                    break;
                }
                if (!char.IsWhiteSpace(c))
                {
                    result = false;
                    break;
                }
            }
            return result;
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
            return Normal.ToString();
        }

        #endregion
    }
}