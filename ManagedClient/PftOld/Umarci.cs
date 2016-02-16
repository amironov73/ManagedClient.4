/* Umarci.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// <para>Форматный выход &amp;umarci.</para>
    /// <list type="bullet">
    /// <item><term>&amp;umarci('1N1#i#N2')</term><description> - выбирает N2-е 
    /// повторение подполя i поля N1.</description></item>
    /// <item><term>&amp;umarci('2N1#S')</term><description> - определяет количество
    /// вхождений строки S в поле N1; длина S &lt;= 10 симв.</description></item>
    /// <item><term>&amp;umarci('3N1#N2#R)</term><description> - из поля N1 выбирает
    /// информацию между (N2-1)-ым и N2-ым разделителями R, если N2&lt;1 и до N2, 
    /// если N2=1.</description></item>
    /// <item><term>&amp;umarci('0a')</term><description> - когда-то использовалась 
    /// для замены разделителей, но теперь замена происходит, если имя fst импорта
    /// содержит 'marc' как часть.</description></item>
    /// <item><term>&amp;umarci('4N1/N2')</term><description> - выдает содержимое 
    /// поля с меткой N2, встроенного в поле N1.</description></item>
    /// </list>
    /// </summary>
    public sealed class Umarci
    {
        #region Properties

        /// <summary>
        /// Контекст форматирования
        /// </summary>
        public PftContext Context { get; set; }

        public PftGroupStatement Group { get; set; }

        #endregion

        #region Construction

        public Umarci()
        {
        }

        public Umarci
            (
                PftContext context
            )
        {
            Context = context;
        }

        public Umarci
            (
                PftContext context, 
                PftGroupStatement grp
            )
        {
            Context = context;
            Group = grp;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public string Evaluate
            (
                string format
            )
        {
            if (string.IsNullOrEmpty(format))
            {
                return string.Empty;
            }

            char firstLetter = format[0];
            string trail = format.Substring(1);

            switch (firstLetter)
            {
                case '1':
                    break;
                case '2':
                    break;
                case '3':
                    break;
                case '4':
                    break;
                default:
                    throw new ArgumentException();
            }

            return string.Empty;
        }

        #endregion
    }
}
