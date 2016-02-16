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
    /// Группа элементов.
    /// </summary>
    [Serializable]
    public sealed class PftGroupStatement
        : PftAst
    {
        #region Properites

        /// <summary>
        /// Ссылка на поле, по которому идёт группировка.
        /// </summary>
        public PftFieldReference Field { get; set; }

        /// <summary>
        /// Тег, по которому идёт группировка
        /// (чисто для удобства, тег можно взять
        /// из поля <see cref="Field"/>).
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Текущий номер повторения (начинается с 0).
        /// </summary>
        public int GroupIndex { get; set; }

        /// <summary>
        /// Значения повторяющихся полей.
        /// </summary>
        public RecordField[] GroupItems { get; set; }

        /// <summary>
        /// Встретилась команда break
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

            // TODO: просматривать также ссылки на глобальные переменные

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

            // Помечаем все чувствительные элементы формата
            // как входящие в повторяющуюся группу.
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

            // Обратите внимание, что группа повторяется всегда
            // на один раз больше, чем есть повторений поля.

            // См. http://irbis.gpntb.ru/read.php?7,22730
            // В документации (Общее описание, Приложение 4) 
            // по этому поводу сказано следующее:
            // «Если в процессе текущего просмотра всей повторяющейся 
            // группы ничего не выводится (то есть в пределах группы больше 
            // не оказалось экземпляров повторяющегося поля), то процесс 
            // обработки повторяющейся группы завершается.» 
            // Здесь требуется уточнение. Выход из повторяющейся группы 
            // происходит, если при очередном проходе НИ ОДНА из ИСПОЛНЯЕМЫХ 
            // (в процессе данного прохода) конструкций выбора ПОЛЯ 
            // (именно поля, а не подполя, т.е если исполняется конструкция 
            // вида V100^A, то в расчет данного условия берется только V100) 
            // и НИ ОДИН из ИСПОЛНЯЕМЫХ форматных выходов (&uf) не возвращают 
            // НЕПУСТОЕ значение (можно сформулировать это условие иначе 
            // - …если ВСЕ ИСПОЛНЯЕМЫЕ конструкции выбора ПОЛЯ и форматные 
            // выходы возвращают ПУСТОТУ) 

            // Такая группа должна зациклиться в ИРБИС:
            // (if &uf("Av100#1")>v200 then ... else ... fi/) 
            // Вообще, непродуманное употребление форматного выхода для получения 
            // заданного повторения поля (&uf(‘AvMM#N’) чаще всего приводит 
            // к зацикливанию повторяющихся групп. 

            // Чтобы группа выполнилась столько раз, сколько повторений поля,
            // нужно использовать конструкцию:
            // (if p(v100^A) then … fi/)

            // В оригинальном форматере имеется программная защита от зацикливания 
            // повторяющихся групп, которая основана на жестком ограничении 
            // максимального количества проходов: в ИРБИС32 это 500, в ИРБИС64 – 5000 
            // (разумеется, эти величины условные и при необходимости их можно будет изменить)

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