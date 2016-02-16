/* PftFieldOrGlobal.cs
 */

#region Using directives

using System;

using Antlr4.Runtime.Tree;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// ������ �� ���� ��� �� ���������� ����������.
    /// ����������� �����, ��. ��������.
    /// </summary>
    [Serializable]
    public abstract class PftFieldOrGlobal
        : PftGroupItem
    {
        #region Properties

        #endregion

        #region Construction

        internal PftFieldOrGlobal()
        {
        }

        internal PftFieldOrGlobal(IParseTree node)
            : base(node)
        {
        }

        #endregion
    }
}