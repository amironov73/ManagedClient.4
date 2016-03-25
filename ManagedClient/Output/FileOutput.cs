/* FileOutput.cs
 */

#region

using System;
using System.IO;
using System.Text;

#endregion

namespace ManagedClient.Output
{
    /// <summary>
    /// Файловый вывод
    /// </summary>
    public sealed class FileOutput
        : AbstractOutput
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public string FileName { get { return _fileName; } }

        #endregion

        #region Construction

        /// <summary>
        /// 
        /// </summary>
        public FileOutput()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public FileOutput
            (
                string fileName
            )
        {
            Open
                (
                    fileName
                );
        }

        #endregion

        #region Private members

        private string _fileName;

        private TextWriter _writer;

        #endregion

        #region Public methods

        public void Close()
        {
            Dispose();
        }

        public void Open
            (
                string fileName
            )
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }
            Close();
            _fileName = fileName;
            _writer = new StreamWriter
                (
                    fileName
                );
        }

        public void Open
            (
                string fileName, 
                Encoding encoding
            )
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }
            if (ReferenceEquals(encoding, null))
            {
                throw new ArgumentNullException("encoding");
            }
            Close();
            _fileName = fileName;
            _writer = new StreamWriter
                (
                    fileName,
                    false,
                    encoding
                );
        }

        #endregion

        #region AbstractOutput members

        public override bool HaveError { get; set; }

        public override AbstractOutput Clear()
        {
            // TODO: implement properly
            return this;
        }

        public override AbstractOutput Configure
            (
                string configuration
            )
        {
            // TODO: implement properly
            Open(configuration);
            return this;
        }

        public override AbstractOutput Write
            (
                string text
            )
        {
            if (!ReferenceEquals(_writer, null))
            {
                _writer.Write(text);
                _writer.Flush();
            }
            return this;
        }

        public override AbstractOutput WriteError
            (
                string text
            )
        {
            HaveError = true;
            if (!ReferenceEquals(_writer, null))
            {
                _writer.Write(text);
                _writer.Flush();
            }
            return this;
        }

        #endregion

        #region IDisposable members

        public override void Dispose()
        {
            if (_writer != null)
            {
                _writer.Dispose();
                _writer = null;
            }
            base.Dispose();
        }

        #endregion
    }
}
