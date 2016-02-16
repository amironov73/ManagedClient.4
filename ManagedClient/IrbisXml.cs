/* IrbisXml.cs -- MarcXml import/export
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// MarcXml import/export
    /// </summary>
    public sealed class IrbisXml
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public string ExportRecords
            (
                IEnumerable<IrbisRecord> records
            )
        {
            StringWriter result = new StringWriter();
            XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "\t",
                    NewLineHandling = NewLineHandling.Entitize,
                    NewLineChars = Environment.NewLine
                };
            using (XmlWriter writer = XmlWriter.Create(result, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("RECORDS");
                ExportRecords
                    (
                        writer,
                        records
                    );
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            return result.ToString();
        }

        public void ExportRecords
            (
                XmlWriter writer,
                IEnumerable<IrbisRecord> records
            )
        {
            foreach (IrbisRecord record in records)
            {
                ExportRecord
                    (
                        writer,
                        record
                    );
            }
        }

        public void ExportRecord
            (
                XmlWriter writer, 
                IrbisRecord record
            )
        {
            writer.WriteStartElement("record");
            ExportMarker
                (
                    writer,
                    record
                );
            foreach (RecordField field in record.Fields)
            {
                ExportField
                    (
                        writer,
                        field
                    );
            }
            writer.WriteEndElement();
        }

        public void ExportMarker
            (
                XmlWriter writer,
                IrbisRecord record
            )
        {
            writer.WriteRaw
                (
                    "<mrk>"
                    + "<m_0_4 len=\"5\">#####</m_0_4>"
                    + "<m_5_5 len=\"1\">n</m_5_5>"
                    + "<m_6_6 len=\"1\">a</m_6_6>"
                    + "<m_7_7 len=\"1\">m</m_7_7>"
                    + "<m_8_8 len=\"1\">#</m_8_8>"
                    + "<m_9_9 len=\"1\">#</m_9_9>"
                    + "<m_10_10 len=\"1\">2</m_10_10>"
                    + "<m_11_11 len=\"1\">2</m_11_11>"
                    + "<m_12_16 len=\"5\">#####</m_12_16>"
                    + "<m_17_17 len=\"1\">#</m_17_17>"
                    + "<m_18_18 len=\"1\">#</m_18_18>"
                    + "<m_19_19 len=\"1\">#</m_19_19>"
                    + "<m_20_23 len=\"4\">450 </m_20_23>"
                    + "</mrk>"
                );
        }

        public void ExportField
            (
                XmlWriter writer, 
                RecordField field
            )
        {
            string localName = "FIELD." + field.Tag.ToUpper();
            writer.WriteStartElement(localName);
            ExportText(writer, field);
            foreach (SubField subField in field.SubFields)
            {
                ExportSubField
                    (
                        writer,
                        subField
                    );
            }
            writer.WriteEndElement();
        }

        public void ExportText
            (
                XmlWriter writer, 
                RecordField field
            )
        {
            if (!string.IsNullOrEmpty(field.Text))
            {
                writer.WriteValue(field.Text);
            }
        }

        public void ExportSubField
            (
                XmlWriter writer, 
                SubField subField
            )
        {
            string localName = "SUBFIELD." + char.ToUpper(subField.Code);
            writer.WriteElementString(localName, subField.Text);
        }

        #endregion

        #region Object members

        #endregion
    }
}
