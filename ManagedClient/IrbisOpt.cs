/* IrbisOpt.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Файл OPT.
    /// </summary>
    [Serializable]
    public sealed class IrbisOpt
    {        
        #region Properties

        public int Id
        {
            get
            {
                return OptFormatInfo.fieldId;
            }
        }

        #endregion

        #region Construction

        public IrbisOpt()
        {
            OptFormatInfo = new FormatInfo();
        }


        #endregion

        #region Private members

        private bool IsEqual(String str1, String str2, int count)
        {
            int index = str2.IndexOf('+');
            if (index == -1)
                return str1 == str2;

            if (index < count)
            {
                bool result;
                if (index != 0)
                    result = str1.Substring(0, index) == str2.Substring(0, index);
                else
                    result = true;

                if (!result)
                    return false;

                while (index < count && index < str1.Length && index < str2.Length)
                {
                    if (str2[index] != '+')
                    {
                        if (str1[index] != str2[index])
                            return false;
                    }
                    index++;
                }

                return (index < count && (index == str2.Length || index >= str1.Length));
            }
            else
                return false;
        }

        #endregion

        #region Public methods

        public string SelectOptFile(IrbisRecord record)
        {
            String selector = record.FM(Id.ToString());

            int index = -1;
            if (!String.IsNullOrEmpty(selector))
            {
                index = 0;
                foreach (FormatItems formatItem in OptFormatInfo.formatItems)
                {
                    if (IsEqual(selector, formatItem.docType, OptFormatInfo.docTypeMaxLen))                    
                        return formatItem.pftFilename;                        
                    
                    index++;
                }
            }
            return String.Empty;
        }

        #endregion

        #region Object members

        public FormatInfo OptFormatInfo;

        #endregion
    }

    public struct FormatItems
    {
        public String docType;
        public String pftFilename;
    };

    public class FormatInfo
    {
        public int fieldId;
        public int docTypeMaxLen;
        public FormatItems[] formatItems;
    };
}
