/* PftSlashOperator.cs
 */

#region Using directives

using System;
using System.IO;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// ������� ������������� ����������������.
    /// ������� �� ����� ������, ���� ������� ������ �� ���� ������.
    /// �������� � ���������� ����������� ������ � ������ ��������� ������.
    /// ������ ������ ������������� ������� /, ���� � �������� ������������� 
    /// �����������, �� ����� ��� �� �����, ��� � ���� ������� /, 
    /// �.�. ������� / ������� �� ������� ������ �����.
    /// </summary>
    [Serializable]
    public sealed class PftSlashOperator
        : PftAst
    {
        #region Construction

        public PftSlashOperator()
        {
        }

        public PftSlashOperator(PftParser.SlashOperatorContext node)
            : base(node)
        {
        }

        #endregion

        #region PftAst members

        public override bool Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (!context.Output.HaveEmptyLine())
            {
                context.WriteLine(this);
            }

            OnAfterExecution(context);

            return false;
        }

        public override void Write
            (
                StreamWriter writer
            )
        {
            // ��������� ���������
            writer.Write(" / ");
        }

        #endregion
    }
}