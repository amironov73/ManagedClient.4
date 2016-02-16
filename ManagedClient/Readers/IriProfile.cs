/* IriProfile.cs
 */

#region Using directives

using System;
using System.Collections.Generic;

using ManagedClient;

#endregion

namespace ManagedClient.Readers
{
    [Serializable]
    public sealed class IriProfile
    {
        #region Properties

        /// <summary>
        /// Подполе A
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Подполе B
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Подполе C
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Подполе D
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Подполе E
        /// </summary>
        public int Periodicity { get; set; }

        /// <summary>
        /// Подполе F
        /// </summary>
        public string LastServed { get; set; }

        /// <summary>
        /// Подполе I
        /// </summary>
        public string Database { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public static IriProfile ParseField
            (
                RecordField field
            )
        {
            IriProfile result = new IriProfile
            {
                Active = field.GetFirstSubFieldText('a') == "1",
                ID = field.GetFirstSubFieldText('b'),
                Title = field.GetFirstSubFieldText('c'),
                Query = field.GetFirstSubFieldText('d'),
                Periodicity = int.Parse(field.GetFirstSubFieldText('e')),
                LastServed = field.GetFirstSubFieldText('f'),
                Database = field.GetFirstSubFieldText('i')
            };

            return result;
        }

        public static IriProfile[] ParseRecord
            (
                IrbisRecord record
            )
        {
            List<IriProfile> result = new List<IriProfile>();
            foreach (RecordField field in record.Fields.GetField("140"))
            {
                IriProfile profile = ParseField(field);
                if (profile != null)
                {
                    result.Add(profile);
                }
            }
            return result.ToArray();
        }

        #endregion
    }
}
