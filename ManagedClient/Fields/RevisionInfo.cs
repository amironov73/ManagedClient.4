/* RevisionInfo.cs
 */

#region Using directives

using System;

using ManagedClient.Mapping;

#endregion

namespace ManagedClient.Fields
{
    /// <summary>
    /// Данные о редактировании записи (поле 907).
    /// </summary>
    [Serializable]
    public sealed class RevisionInfo
    {
        #region Properties

        /// <summary>
        /// Этап работы. Подполе c.
        /// </summary>
        [SubField('c')]
        public string Stage { get; set; }

        /// <summary>
        /// Дата. Подполе a.
        /// </summary>
        [SubField('a')]
        public string Date { get; set; }

        /// <summary>
        /// ФИО оператора. Подполе b.
        /// </summary>
        [SubField('b')]
        public string Name { get; set; }

        #endregion

        #region Public methods

        public static RevisionInfo Parse
            ( 
                RecordField field 
            )
        {
            RevisionInfo result = new RevisionInfo
                {
                    Date = field.GetSubFieldText ( 'a', 0 ),
                    Name = field.GetSubFieldText ( 'b', 0 ),
                    Stage = field.GetSubFieldText ( 'c', 0 )
                };

            return result;
        }

        public RecordField ToField ()
        {
            RecordField result = new RecordField ( "907" )
                .AddNonEmptySubField ( 'a', Date )
                .AddNonEmptySubField ( 'b', Name )
                .AddNonEmptySubField ( 'c', Stage );
            return result;
        }

        #endregion

        #region Object members

        public override string ToString ()
        {
            return string.Format 
                ( 
                    "Stage: {0}, Date: {1}, Name: {2}", 
                    Stage, 
                    Date, 
                    Name 
                );
        }

        #endregion
    }
}
