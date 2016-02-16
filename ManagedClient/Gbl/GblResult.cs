/* GblResult.cs - GBL result for one record.
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

namespace ManagedClient.Gbl
{
    /// <summary>
    /// Результат выполнения GBL для одной записи.
    /// </summary>
    [Serializable]
    public sealed class GblResult
    {
        #region Properties

        /// <summary>
        /// Общий признак успеха.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Имя базы данных
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// MFN записи
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Результат Autoin.gbl
        /// </summary>
        public string Autoin { get; set; }

        public string Update { get; set; }

        public string Status { get; set; }

        /// <summary>
        /// Код ошибки, если есть
        /// </summary>
        public string Error { get; set; }

        public string UpdUf { get; set; }

        /// <summary>
        /// Исходный текст (до парсинга)
        /// </summary>
        public string Text { get; set; }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        // Типичная строка с положительным результатом:
        // DBN=IBIS#MFN=2#AUTOIN=#UPDATE=0#STATUS=8#UPDUF=0#

        // Типичная строка с отрицательным результатом
        // DBN=IBIS#MFN=4#GBL_ERROR=-605

        public static GblResult Parse
            (
                string line
            )
        {
            if (string.IsNullOrEmpty(line))
            {
                return null;
            }

            GblResult result = new GblResult
            {
                Text = line,
                Success = true
            };
            string[] parts = line.Split('#');
            foreach (string part in parts)
            {
                string[] p = part.Split('=');
                if (p.Length > 0)
                {
#if PocketPC
                    string name = p[0].ToUpper();
#else
                    string name = p[0].ToUpperInvariant();
#endif
                    string value = string.Empty;
                    if (p.Length > 1)
                    {
                        value = p[1];
                    }
                    switch (name)
                    {
                        case "DBN":
                            result.Database = value;
                            break;
                        case "MFN":
                            result.Mfn = value.SafeParseInt32();
                            break;
                        case "AUTOIN":
                            result.Autoin = value;
                            break;
                        case "UPDATE":
                            result.Update = value;
                            break;
                        case "STATUS":
                            result.Status = value;
                            break;
                        case "UPDUF":
                            result.UpdUf = value;
                            break;
                        case "GBL_ERROR":
                            result.Error = value;
                            result.Success = false;
                            break;
                    }
                }
            }
            return result;
        }

        public static GblResult[] Parse
            (
                IEnumerable<string> lines
            )
        {
            List<GblResult> result = new List<GblResult>();
            foreach (string line in lines)
            {
                GblResult gblResult = Parse(line);
                if (gblResult != null)
                {
                    result.Add(gblResult);
                }
            }

            return result.ToArray();
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            return Text;
        }

        #endregion
    }
}
