/* CodesInfo.cs
 */

#region Using directives

using System;

using ManagedClient.Mapping;

#endregion

namespace ManagedClient.Fields
{
    /// <summary>
    /// Коды (поле 900).
    /// </summary>
    [Serializable]
    public sealed class CodesInfo
    {
        #region Properties

        /// <summary>
        /// Тип документа. Подполе t.
        /// </summary>
        [SubField('t')]
        public string DocumentType { get; set; }

        /// <summary>
        /// Вид документа. Подполе b.
        /// </summary>
        [SubField('b')]
        public string DocumentKind { get; set; }

        /// <summary>
        /// Характер документа. Подполе c.
        /// </summary>
        [SubField('c')]
        public string DocumentCharacter1 { get; set; }

        /// <summary>
        /// Характер документа. Подполе 2.
        /// </summary>
        [SubField('2')]
        public string DocumentCharacter2 { get; set; }

        /// <summary>
        /// Характер документа. Подполе 3.
        /// </summary>
        [SubField('3')]
        public string DocumentCharacter3 { get; set; }

        /// <summary>
        /// Характер документа. Подполе 4.
        /// </summary>
        [SubField('4')]
        public string DocumentCharacter4 { get; set; }

        /// <summary>
        /// Характер документа. Подполе 5.
        /// </summary>
        [SubField('5')]
        public string DocumentCharacter5 { get; set; }

        /// <summary>
        /// Характер документа. Подполе 6.
        /// </summary>
        [SubField('6')]
        public string DocumentCharacter6 { get; set; }

        /// <summary>
        /// Код целевого назначения. Подполе x.
        /// </summary>
        [SubField('7')]
        public string PurposeCode1 { get; set; }

        /// <summary>
        /// Код целевого назначения. Подполе y.
        /// </summary>
        [SubField('y')]
        public string PurposeCode2 { get; set; }

        /// <summary>
        /// Код целевого назначения. Подполе 9.
        /// </summary>
        [SubField('9')]
        public string PurposeCode3 { get; set; }

        /// <summary>
        /// Возрастные ограничения. Подполе z.
        /// </summary>
        [SubField('z')]
        public string AgeRestrictions { get; set; }

        #endregion

        #region Public methods

        public static CodesInfo Parse
            (
                RecordField field
            )
        {
            CodesInfo result = new CodesInfo
                {
                    DocumentType = field.GetSubFieldText ( 't', 0 ),
                    DocumentKind = field.GetSubFieldText ( 'b', 0 ),
                    DocumentCharacter1 = field.GetSubFieldText ( 'c', 0 ),
                    DocumentCharacter2 = field.GetSubFieldText ( '2', 0 ),
                    DocumentCharacter3 = field.GetSubFieldText ( '3', 0 ),
                    DocumentCharacter4 = field.GetSubFieldText ( '4', 0 ),
                    DocumentCharacter5 = field.GetSubFieldText ( '5', 0 ),
                    DocumentCharacter6 = field.GetSubFieldText ( '6', 0 ),
                    PurposeCode1 = field.GetSubFieldText ( 'x', 0 ),
                    PurposeCode2 = field.GetSubFieldText ( 'y', 0 ),
                    PurposeCode3 = field.GetSubFieldText ( '9', 0 ),
                    AgeRestrictions = field.GetSubFieldText ( 'z', 0 )
                };

            return result;
        }

        public RecordField ToField ()
        {
            RecordField result = new RecordField("900")
                .AddNonEmptySubField ( 't', DocumentType )
                .AddNonEmptySubField ( 'b', DocumentKind )
                .AddNonEmptySubField ( 'c', DocumentCharacter1 )
                .AddNonEmptySubField ( '2', DocumentCharacter2 )
                .AddNonEmptySubField ( '3', DocumentCharacter3 )
                .AddNonEmptySubField ( '4', DocumentCharacter4 )
                .AddNonEmptySubField ( '5', DocumentCharacter5 )
                .AddNonEmptySubField ( '6', DocumentCharacter6 )
                .AddNonEmptySubField ( 'x', PurposeCode1 )
                .AddNonEmptySubField ( 'y', PurposeCode2 )
                .AddNonEmptySubField ( '9', PurposeCode3 )
                .AddNonEmptySubField ( 'z', AgeRestrictions );
            return result;
        }

        #endregion
    }
}
