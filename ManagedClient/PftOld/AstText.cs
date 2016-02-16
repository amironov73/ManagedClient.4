/* AstText.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Текст с информацией о его происхождении
    /// (какой элемент формата его сгенерировал).
    /// </summary>
    [Serializable]
    public sealed class AstText
    {
        #region Properties

        public PftAst Node { get; set; }

        public string Text { get; set; }

        public int LineNumber { get { return Node.LineNumber; } }

        //public int LinePosition { get { return Node.??? } }

        #endregion

        #region Construction

        public AstText()
        {
        }

        public AstText
            (
                PftAst node, 
                string text
            )
        {
            if (ReferenceEquals(node, null))
            {
                throw new ArgumentNullException("node");
            }
            if (ReferenceEquals(text, null))
            {
                throw new ArgumentNullException("text");
            }

            Node = node;
            Text = text;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion
    }
}
