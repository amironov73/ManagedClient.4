/* PftModeSwitch.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft.Ast
{
    /// <summary>
    /// Переключение режима вывода полей с подполями.
    /// </summary>
    [Serializable]
    public sealed class PftModeSwitch
        : PftAst
    {
        #region Properties

        public PftFieldOutputMode OutputMode { get; set; }

        public bool UpperMode { get; set; }

        #endregion

        #region Construction

        public PftModeSwitch()
        {
        }

        public PftModeSwitch
            (
                PftParser.ModeSwitchContext node
            )
        {
            string text = node.GetText().ToLowerInvariant();
            if (text.Length != 3)
            {
                throw new ArgumentException("mode");
            }
            switch (text[1])
            {
                case 'p':
                    OutputMode = PftFieldOutputMode.ModeP;
                    break;
                case 'h':
                    OutputMode = PftFieldOutputMode.ModeH;
                    break;
                case 'd':
                    OutputMode = PftFieldOutputMode.ModeD;
                    break;
                default:
                    throw new ArgumentException();
            }
            switch (text[2])
            {
                case 'u':
                    UpperMode = true;
                    break;
                case 'l':
                    UpperMode = false;
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region PftAst members

        public override void Execute
            (
                PftContext context
            )
        {
            context.FieldOutputMode = OutputMode;
            context.UpperMode = UpperMode;
        }

        #endregion
    }
}
