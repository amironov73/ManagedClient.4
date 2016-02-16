/* IrbisLine.cs -- one line in worksheet
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
    /// One line in worksheet
    /// </summary>
    [Serializable]
    public sealed class IrbisLine
    {
        #region Properties

        /// <summary>
        /// Числовая метка поля.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Наименование поля.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Повторяемость поля.
        /// </summary>
        public bool Repeatable { get; set; }

        /// <summary>
        /// Индекс контекстной помощи.
        /// </summary>
        public string Help { get; set; }

        /// <summary>
        /// Режим ввода.
        /// </summary>
        public IrbisInputMode InputMode { get; set; }

        /// <summary>
        /// Дополнительная информация для расширенных
        /// средств ввода.
        /// </summary>
        public string InputInfo { get; set; }

        /// <summary>
        /// ФЛК.
        /// </summary>
        public string FormalVerification { get; set; }

        /// <summary>
        /// Подсказка - текст помощи (инструкции),
        /// сопровождающий ввод в поле.
        /// </summary>
        public string Hint { get; set; }

        /// <summary>
        /// Знчение по умолчанию при создании 
        /// новой записи.
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Используется при определенных режимах ввода.
        /// </summary>
        public string Reserved { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="IrbisLine"/> class.
        /// </summary>
        public IrbisLine()
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="IrbisLine"/> class.
        /// </summary>
        /// <param name="tag">The tag.</param>
        public IrbisLine
            (
                string tag
            )
        {
            Tag = tag;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IrbisLine"/> class.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="title">The title.</param>
        public IrbisLine
            (
                string tag, 
                string title
            )
        {
            Tag = tag;
            Title = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IrbisLine"/> class.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="title">The title.</param>
        /// <param name="repeatable">if set to <c>true</c> [repeatable].</param>
        public IrbisLine
            (
                string tag, 
                string title, 
                bool repeatable
            )
        {
            Tag = tag;
            Title = title;
            Repeatable = repeatable;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public void ParseLines
            (
                string[] lines
            )
        {
            Tag = lines[0];
            Title = lines[1].Trim();
            Repeatable = Convert.ToBoolean(lines[2]);
            Help = lines[3].Trim();
            InputMode = (IrbisInputMode) int.Parse(lines[4]);
            InputInfo = lines[5].Trim();
            FormalVerification = lines[6].Trim();
            Hint = lines[7].Trim();
            DefaultValue = lines[8].Trim();
            Reserved = lines[9].Trim();
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format
                (
                    "{0}: {1} [{2}][{3}]", 
                    Tag,
                    Title,
                    Repeatable,
                    InputMode
                );
        }

        #endregion
    }
}
