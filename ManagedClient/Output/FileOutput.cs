/* FileOutput.cs -- файловый вывод
 */

#region

using System;
using System.IO;
using System.Text;

#endregion

namespace ManagedClient.Output
{
    /// <summary>
    /// Файловый вывод.
    /// </summary>
    public sealed class FileOutput
        : AbstractOutput
    {
        #region Properties

        /// <summary>
        /// Имя файла.
        /// </summary>
        public string FileName { get { return _fileName; } }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public FileOutput()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
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

        /// <summary>
        /// Закрытие файла.
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>
        /// Открытие файла.
        /// </summary>
        public void Open
            (
                string fileName,
                bool append
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
                    fileName,
                    append
                );
        }

        /// <summary>
        /// Открытие файла.
        /// </summary>
        public void Open
            (
                string fileName
            )
        {
            Open
                (
                    fileName, 
                    false
                );
        }

        /// <summary>
        /// Открытие файла.
        /// </summary>
        public void Open
            (
                string fileName,
                bool append,
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
                    append,
                    encoding
                );
        }

        /// <summary>
        /// Открытие файла.
        /// </summary>
        public void Open
            (
                string fileName, 
                Encoding encoding
            )
        {
            Open
                (
                    fileName,
                    false,
                    encoding
                );
        }

        #endregion

        #region AbstractOutput members

        /// <summary>
        /// Флаг: был ли вывод с помощью WriteError.
        /// </summary>
        /// <value><c>true</c> if [have error]; otherwise, <c>false</c>.</value>
        public override bool HaveError { get; set; }

        /// <summary>
        /// Очищает вывод, например, окно.
        /// Надо переопределить в потомке.
        /// </summary>
        /// <returns>AbstractOutput.</returns>
        public override AbstractOutput Clear()
        {
            // TODO: implement properly
            return this;
        }

        /// <summary>
        /// Конфигурирование объекта.
        /// Надо переопределить в потомке.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>AbstractOutput.</returns>
        public override AbstractOutput Configure
            (
                string configuration
            )
        {
            // TODO: implement properly
            Open(configuration);
            return this;
        }

        /// <summary>
        /// Метод, который нужно переопределить
        /// в потомке.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>Возвращает сам объект
        /// вывода.</returns>
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

        /// <summary>
        /// Выводит ошибку. Например, красным цветом.
        /// Надо переопределить в потомке.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>AbstractOutput.</returns>
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

        /// <summary>
        /// Disposes this instance.
        /// </summary>
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
