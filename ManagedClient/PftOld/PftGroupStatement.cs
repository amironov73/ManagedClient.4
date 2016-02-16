/* PftGroupStatement.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// ������ ���������.
    /// </summary>
    [Serializable]
    public sealed class PftGroupStatement
        : PftAst
    {
        #region Properites

        /// <summary>
        /// ������ �� ����, �� �������� ��� �����������.
        /// </summary>
        public PftFieldReference Field { get; set; }

        /// <summary>
        /// ���, �� �������� ��� �����������
        /// (����� ��� ��������, ��� ����� �����
        /// �� ���� <see cref="Field"/>).
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// ������� ����� ���������� (���������� � 0).
        /// </summary>
        public int GroupIndex { get; set; }

        /// <summary>
        /// �������� ������������� �����.
        /// </summary>
        public RecordField[] GroupItems { get; set; }

        /// <summary>
        /// ����������� ������� break
        /// </summary>
        public bool BreakEncountered { get; set; }

        #endregion

        #region Construction

        public PftGroupStatement()
        {
        }

        public PftGroupStatement(PftParser.GroupStatementContext node)
            : base(node)
        {
            PftNonGrouped subTree = new PftNonGrouped(node.nonGrouped());
            Children.Add(subTree);

            // TODO: ������������� ����� ������ �� ���������� ����������

            List<PftFieldReference> refs = subTree
                .GetDescendants<PftFieldReference>();
            if (refs.Count != 0)
            {
                Field = refs[0];
                Tag = Field.Field.Field;
                foreach (PftFieldReference fld in refs)
                {
                    if (fld.Field.Field == Tag)
                    {
                        fld.Group = this;
                    }
                }
            }

            // �������� ��� �������������� �������� �������
            // ��� �������� � ������������� ������.
            subTree
                .GetDescendants<PftGroupItem>()
                .ForEach( item => item.Group = this );
        }

        #endregion

        #region Private members

        #endregion

        #region PftAst members

        public override void Execute
            (
                PftContext context
            )
        {
            GroupItems = context.Record.Fields.GetField(Tag);
            string embedded = Field.Field.Embedded;
            if (!string.IsNullOrEmpty(embedded))
            {
                GroupItems = GroupItems.SelectMany(f => f.GetEmbeddedFields())
                    .GetField(embedded);
            }

            GroupIndex = 0;

            bool needMore;

            // �������� ��������, ��� ������ ����������� ������
            // �� ���� ��� ������, ��� ���� ���������� ����.

            // ��. http://irbis.gpntb.ru/read.php?7,22730
            // � ������������ (����� ��������, ���������� 4) 
            // �� ����� ������ ������� ���������:
            // ����� � �������� �������� ��������� ���� ������������� 
            // ������ ������ �� ��������� (�� ���� � �������� ������ ������ 
            // �� ��������� ����������� �������������� ����), �� ������� 
            // ��������� ������������� ������ �����������.� 
            // ����� ��������� ���������. ����� �� ������������� ������ 
            // ����������, ���� ��� ��������� ������� �� ���� �� ����������� 
            // (� �������� ������� �������) ����������� ������ ���� 
            // (������ ����, � �� �������, �.� ���� ����������� ����������� 
            // ���� V100^A, �� � ������ ������� ������� ������� ������ V100) 
            // � �� ���� �� ����������� ��������� ������� (&uf) �� ���������� 
            // �������� �������� (����� �������������� ��� ������� ����� 
            // - ����� ��� ����������� ����������� ������ ���� � ��������� 
            // ������ ���������� �������) 

            // ����� ������ ������ ����������� � �����:
            // (if &uf("Av100#1")>v200 then ... else ... fi/) 
            // ������, ������������� ������������ ���������� ������ ��� ��������� 
            // ��������� ���������� ���� (&uf(�AvMM#N�) ���� ����� �������� 
            // � ������������ ������������� �����. 

            // ����� ������ ����������� ������� ���, ������� ���������� ����,
            // ����� ������������ �����������:
            // (if p(v100^A) then � fi/)

            // � ������������ ��������� ������� ����������� ������ �� ������������ 
            // ������������� �����, ������� �������� �� ������� ����������� 
            // ������������� ���������� ��������: � �����32 ��� 500, � �����64 � 5000 
            // (����������, ��� �������� �������� � ��� ������������� �� ����� ����� ��������)

            BreakEncountered = false;

            do
            {
                foreach (PftAst child in Children)
                {
                    child.Execute(context);
                    if (BreakEncountered)
                    {
                        break;
                    }
                }

                GroupIndex++;
                needMore = (GroupIndex < GroupItems.Length);
            } while (needMore);

        }

        #endregion
    }
}