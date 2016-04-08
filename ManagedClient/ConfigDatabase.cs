/* ConfigDatabase.cs
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

#endregion

namespace ManagedClient
{
    public class ConfigDatabase
    {
        #region Nested classes

        class ConfigLine
        {
            #region Properties

            public NumberText From { get; set; }

            public NumberText To { get; set; }

            #endregion
        }

        #endregion

        #region Construction

        public ConfigDatabase
            (
                ManagedClient64 client
            )
        {
            _lines = new List<ConfigLine>();

            using (new IrbisContextSaver(client))
            {
                client.Database = "CONFIG";
                IrbisRecord record = client
                    .SearchReadOneRecord("RL=OBRAB");
                if (record != null)
                {
                    RecordField[] fields = record.Fields
                        .GetField("100");
                    _lines.AddRange(fields.Select(_ParseField));
                }
            }
        }

        #endregion

        #region Private members

        private readonly List<ConfigLine> _lines;

        private static ConfigLine _ParseField
            (
                RecordField field
            )
        {
            ConfigLine result = new ConfigLine
            {
                From = field.GetFirstSubFieldText('a'),
                To = field.GetFirstSubFieldText('b')
            };
            return result;
        }

        #endregion

        #region Public methods

        public bool BookInWork
            (
                string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            NumberText number = text;
            foreach (var line in _lines)
            {
                if ((line.From <= number)
                    && (line.To >= number))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
