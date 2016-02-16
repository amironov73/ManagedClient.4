/* OrganizationInfo.cs -- обёртка для ORG.MNU
 */

#region Using directives

using System.IO;
using System.Text;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Обёртка для ORG.MNU
    /// </summary>
    public sealed class OrganizationInfo
    {
        #region Properties
        
        /// <summary>
        /// Код страны.
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Наименование организации.
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// Обозначение валюты.
        /// </summary>
        public string CurrencySymbol { get; set; }

        /// <summary>
        /// Единица измерения объема издания.
        /// </summary>
        public string VolumeSymbol { get; set; }

        /// <summary>
        /// Страницы для журналов/газет.
        /// </summary>
        public string PagesSymbol { get; set; }

        /// <summary>
        /// Национальный язык.
        /// </summary>
        public string NationalLanguage { get; set; }

        /// <summary>
        /// Проверка фонда.
        /// </summary>
        public string FundCheck { get; set; }

        /// <summary>
        /// Формировать словарь «Технология»?
        /// </summary>
        public string Technology { get; set; }

        /// <summary>
        /// Автоматически формировать авторский знак?
        /// </summary>
        public string AuthorSign { get; set; }

        /// <summary>
        /// Сигла библиотеки.
        /// </summary>
        public string Sigla { get; set; }

        #endregion

        #region Construction

        public OrganizationInfo()
        {
            CountryCode = "RU";
            Organization = "Организация";
            CurrencySymbol = " р.";
            VolumeSymbol = "с";
            PagesSymbol = "стр.";
            NationalLanguage = "sibir";
            FundCheck = "0";
            Technology = "0";
            AuthorSign = "0";
            Sigla = "00000000";
        }

        #endregion

        #region Public methods

        public void ParseLines
            (
                string[] lines
            )
        {
            int len = lines.Length - 1;
            for (int i = 0; i < len; i+=2)
            {
                string code = lines[i];
                if (code.StartsWith("*"))
                {
                    break;
                }
                string value = lines[i + 1];
                switch (code)
                {
                    case "1":
                        CountryCode = value;
                        break;
                    case "2":
                        Organization = value;
                        break;
                    case "3":
                        CurrencySymbol = value;
                        break;
                    case "4":
                        VolumeSymbol = value;
                        break;
                    case "5":
                        PagesSymbol = value;
                        break;
                    case "6":
                        NationalLanguage = value;
                        break;
                    case "7":
                        FundCheck = value;
                        break;
                    case "8":
                        Technology = value;
                        break;
                    case "9":
                        AuthorSign = value;
                        break;
                    case "S":
                        Sigla = value;
                        break;
                }
            }
        }

        public void ParseText
            (
                string text
            )
        {
            ParseLines(text.SplitLines());
        }

        public void ParseFile
            (
                string fileName
            )
        {
            string[] lines = File.ReadAllLines
                (
                    fileName,
                    Encoding.Default
                );
            ParseLines(lines);
        }

        public void LoadFromServer
            (
                ManagedClient64 client
            )
        {
            string text = client.ReadTextFile
                (
                    IrbisPath.MasterFile,
                    "org.mnu"
                );
            if (!string.IsNullOrEmpty(text))
            {
                ParseText(text);
            }
        }

        #endregion
    }
}
