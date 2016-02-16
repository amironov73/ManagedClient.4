/* PftModeSwitch.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// <para>Переключение режима вывода полей с подполями.</para>
    /// </summary>
    [Serializable]
    public sealed class PftModeSwitch
        : PftAst
    {
        #region Properties

        public PftMode OutputMode { get; set; }

        public bool UpperMode { get; set; }

        #endregion

        #region Construction

        public PftModeSwitch()
        {
        }

        public PftModeSwitch(PftParser.ModeSwitchContext node)
            : base(node)
        {
            string text = Text.ToLowerInvariant();
            if (text.Length != 3)
            {
                throw new ArgumentException();
            }
            switch (text[1])
            {
                case 'p':
                    OutputMode = PftMode.ModeP;
                    break;
                case 'h':
                    OutputMode = PftMode.ModeH;
                    break;
                case 'd':
                    OutputMode = PftMode.ModeD;
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

        #region PftAst members

        public override void Execute
            (
                PftContext context
            )
        {
            context.OutputMode = OutputMode;
            context.UpperMode = UpperMode;
        }

        #endregion
    }
}