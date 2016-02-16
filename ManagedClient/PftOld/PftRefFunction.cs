/* PftRefFunction.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Функция REF позволяет извлечь данные из альтернативной 
    /// записи файла документов (той же самой БД).
    /// Первый аргумент - это числовое выражение, дающее MFN 
    /// альтернативной записи, которая должна быть выбрана,
    /// второй аргумент - это формат, который должен быть применен к этой записи.
    /// Если значение выражения не соответствует MFN ни одной 
    /// из записей базы данных, то функция REF возвратит пустую строку.
    /// Функция REF - очень мощное средство, поскольку позволяет 
    /// объединить данные, хранимые в различных записях базы данных, 
    /// в один выводимый документ.
    /// В большинстве случаев связывание записей непосредственно 
    /// через MFN может оказаться неудобным.
    /// Более удобным является использование возможности функции L . 
    /// Напомним, что функция L находит MFN, соответствующий термину доступа. 
    /// Поэтому можно использовать ее для преобразования символьной строки в MFN. 
    /// Для корректного использования функции L нужно установить однозначное 
    /// соответствие между символьной строкой и соответствующим ей MFN. 
    /// Инвертированный файл предоставляет возможность установить такое соответствие.
    /// Система не делает никаких предположений относительно природы связей, 
    /// существующих между записями. Она просто предоставляет механизм связывания 
    /// записей. При конкретном практическом применении пользователь сам определяет 
    /// смысл связей посредством использования языка форматирования и специального 
    /// проектирования базы данных.
    /// Например, если библиографическая запись описания статьи должна быть 
    /// связана с записью соответствующего номера журнала, то необходимо поле 
    /// для отражения природы этой связи (шифр номера журнала).
    /// </summary>
    [Serializable]
    public sealed class PftRefFunction
        : PftAst
    {
        #region Properties

        public PftArithExpression Argument1 { get; set; }

        public PftAst Argument2 { get; set; }

        #endregion

        #region Construction

        public PftRefFunction()
        {
        }

        public PftRefFunction(PftParser.RefFunctionContext node) 
            : base(node)
        {
            Argument1 = new PftArithExpression(node.arg1);
            Children.Add(Argument1);
            Argument2 = PftNonGrouped.DispatchContext(node.arg2);
            Children.Add(Argument2);
        }

        #endregion

        #region PftAst members

        public override void Execute
            (
                PftContext context
            )
        {
            Argument1.Execute(context);
            int mfn = (int) Argument1.Value;
            if (mfn > 0)
            {
                string format = context.Evaluate(Argument2);
                string text = context.Client.FormatRecord(format, mfn);
                context.Write(text);
            }
        }

        #endregion
    }
}
