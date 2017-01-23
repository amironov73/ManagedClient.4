/* FstItem.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Fst
{
    /// <summary>
    /// Строчка из FST-файла.
    /// </summary>
    [Serializable]
    public sealed class FstItem
    {
        #region Properties

        /// <summary>
        /// Номер строки в FST-файле
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        /// Тег поля
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Метод
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Строка с форматом
        /// </summary>
        public string Format { get; set; }

        ///// <summary>
        ///// Распарсенный формат
        ///// </summary>
        //public PftProgram Program { get; set; }

        #endregion

        #region Construction

        public FstItem()
        {
        }

        public FstItem
            (
                ManagedClient64 client,
                string format
            )
            : this ()
        {
            Format = format;
            //PftFormatter formatter = new PftFormatter
            //{
            //    Context = {Client = client}
            //};
            //formatter.ParseInput(format);
            //Program = formatter.Program;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public string[] Execute
            (
                //PftContext context
            )
        {
            return new string[0];

            //string text = context.Evaluate
            //    (
            //        Program
            //    );
            //if (string.IsNullOrEmpty(text))
            //{
            //    return new string[0];
            //}
            //string[] lines = text.SplitLines();
            //string[] result =
            //    (
            //        from line in lines
            //        where !string.IsNullOrEmpty(line)
            //        select line.Trim()
            //    )
            //    .ToArray();
            //return result;
        }

        //public bool Execute
        //    (
        //        PftContext context,
        //        IrbisRecord record
        //    )
        //{
        //    bool result = false;
        //    string[] lines = Execute(context);
        //    foreach (string line in lines)
        //    {
        //        // TODO реализовать разные методы построения поля из результата
        //        RecordField field = RecordField.Parse
        //            (
        //                Tag,
        //                line
        //            );
        //        record.Fields.Add(field);
        //        result = true;
        //    }
        //    return result;
        //}

        #endregion
    }
}
