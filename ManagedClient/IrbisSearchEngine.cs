#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedClient;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Параметры и операции, связанные с поисковыми сценариями
    /// </summary>
    [Serializable]
    public sealed class IrbisSearchEngine
    {
        #region Nested classes

        public class SearchTerm
        {
            public String Key;
            public String Name;
            public bool IsPresent;
            public bool Trunc;
            public List<PostingsParams> Postings;
            public String[] Fields;

            public SearchTerm()
            {
            }

            public SearchTerm(String termKey)
            {
                int index = termKey.IndexOf('=') + 1;
                if (termKey.EndsWith("$"))
                {
                    Trunc = true;
                    termKey = termKey.Remove(termKey.Length - 1);
                }
                else
                    Trunc = false;

                Key = termKey.Substring(0, index);
                Name = termKey.Substring(index).ToUpper();
                IsPresent = true;
            }
        }

        public struct PostingsParams
        {
            public String Text;
            public List<int> Mfn;
        }

        public struct SearchScenario
        {
            public String ItemName;
            public String ItemPref;
            public DictionType ItemDictionType;
            public String ItemMenu;
            public String ItemF8For;
            public String ItemModByDic;
            public bool ItemTranc;
            public String ItemHint;
            public String ItemModByDicAuto;
            public LogicType ItemLogic;
            public String ItemAdv;
            public String ItemPft;
        }

        public enum DictionType { None, Explanation, Special }

        public enum LogicType { Or, OrAnd, OrAndNot, OrAndNotAndField, OrAndNotAndPhrase }

        public struct SearchQualifier
        {
            public String QualifName;
            public String QualifValue;
        }

        #endregion

        #region Properties

        public int MinLKWLight { get; set; }

        #endregion

        #region Construction

        public IrbisSearchEngine(ManagedClient64 client)
        {
            IrbisSearchEngine.client = client;

            IniFile iniFile = IniFile.ParseText<IniFile>(client.Configuration);
            IniFile.Section SearchSection = iniFile.GetSection("SEARCH");

            int itemCount = (SearchSection == null)
                ? 0
                : SearchSection.Get("ItemNumb", 0);

            SearchScenarios = new SearchScenario[itemCount];
            if (SearchSection != null)
            {
                for (int index = 0; index < SearchScenarios.Length; index++)
                {
                    SearchScenario searchScenario;
                    searchScenario.ItemName = SearchSection.Get("ItemName" + index);
                    searchScenario.ItemPref = SearchSection.Get("ItemPref" + index);
                    searchScenario.ItemDictionType = (DictionType)SearchSection.Get("ItemDictionType" + index, 0);
                    searchScenario.ItemMenu = SearchSection.Get("ItemMenu" + index);
                    searchScenario.ItemF8For = SearchSection.Get("ItemF8For" + index);
                    searchScenario.ItemModByDic = SearchSection.Get("ItemModByDic" + index);
                    try
                    {
                        searchScenario.ItemTranc = SearchSection.Get<int>("ItemTranc" + index, 0) != 0;
                    }
                    catch
                    {
                        searchScenario.ItemTranc = true;
                    }
                    searchScenario.ItemHint = SearchSection.Get("ItemHint" + index);
                    searchScenario.ItemModByDicAuto = SearchSection.Get("ItemModByDicAuto" + index);
                    try
                    {
                        searchScenario.ItemLogic = (LogicType)SearchSection.Get<int>("ItemLogic" + index, 0);
                    }
                    catch
                    {
                        searchScenario.ItemLogic = LogicType.Or;
                    }
                    searchScenario.ItemAdv = SearchSection.Get("ItemAdv" + index);
                    searchScenario.ItemPft = SearchSection.Get("ItemPft" + index);

                    SearchScenarios[index] = searchScenario;
                }


                SearchQualifiers = new SearchQualifier[SearchSection.Get("CvalifNumb", 0)];
                for (int index = 0; index < SearchQualifiers.Length; index++)
                {
                    SearchQualifier searchQualifier;
                    searchQualifier.QualifName = SearchSection.Get("CvalifName" + index);
                    searchQualifier.QualifValue = SearchSection.Get("CvalifValue" + index);
                    SearchQualifiers[index] = searchQualifier;
                }

                MinLKWLight = SearchSection.Get<int>("MinLKWLight");
            }
        }

        #endregion

        #region Private members

        static ManagedClient64 client;
        private string searchRequest;
        private int pos;


        List<SearchTerm> ParseOrExpression()
        {
            List<SearchTerm> op1 = ParseAndExpression();
            List<SearchTerm> op2;
            while (pos != searchRequest.Length && searchRequest[pos] == ' ') pos++;
            while (pos != searchRequest.Length && searchRequest[pos] == '+')
            {
                pos++;
                op2 = ParseAndExpression();
                op1.AddRange(op2);
            }
            return op1;
        }

        List<SearchTerm> ParseAndExpression()
        {
            List<SearchTerm> op1 = ParseAndGExpression();
            List<SearchTerm> op2;
            while (pos != searchRequest.Length && searchRequest[pos] == ' ') pos++;
            while (pos != searchRequest.Length && (searchRequest[pos] == '*' || searchRequest[pos] == '^'))
            {
                switch (searchRequest[pos])
                {
                    case '*':
                        pos++;
                        op2 = ParseAndGExpression();
                        op1.AddRange(op2);
                        break;
                    case '^':
                        pos++;
                        op2 = ParseAndGExpression();
                        for (int i = 0; i < op2.Count; i++)
                            op2[i].IsPresent = !op2[i].IsPresent;
                        op1.AddRange(op2);
                        break;
                }
            }
            return op1;
        }

        List<SearchTerm> ParseAndGExpression()
        {
            List<SearchTerm> op1 = ParseAndFExpression();
            List<SearchTerm> op2;
            while (pos != searchRequest.Length && searchRequest[pos] == ' ') pos++;
            while (pos != searchRequest.Length && searchRequest[pos] == 'G')
            {
                pos++;
                if (pos == searchRequest.Length || searchRequest[pos] != ' ')
                    throw new RequestParsingException();
                op2 = ParseAndFExpression();
                op1.AddRange(op2);
            }
            return op1;
        }

        List<SearchTerm> ParseAndFExpression()
        {
            List<SearchTerm> op1 = ParseAnd_Expression();
            List<SearchTerm> op2;
            while (pos != searchRequest.Length && searchRequest[pos] == ' ') pos++;
            while (pos != searchRequest.Length && searchRequest[pos] == 'F')
            {
                pos++;
                if (pos == searchRequest.Length || searchRequest[pos] != ' ')
                    throw new RequestParsingException();
                op2 = ParseAnd_Expression();
                op1.AddRange(op2);
            }
            return op1;
        }

        List<SearchTerm> ParseAnd_Expression()
        {
            List<SearchTerm> op1 = ParseTerm();
            List<SearchTerm> op2;
            while (pos != searchRequest.Length && searchRequest[pos] == ' ') pos++;
            while (pos != searchRequest.Length && searchRequest[pos] == '.')
            {
                pos++;
                if (pos == searchRequest.Length || searchRequest[pos] != ' ')
                    throw new RequestParsingException();
                op2 = ParseTerm();
                op1.AddRange(op2);
            }
            return op1;
        }

        List<SearchTerm> ParseTerm()
        {
            while (searchRequest[pos] == ' ') pos++;
            switch (searchRequest[pos])
            {
                case '\"':
                    return ParseSimpleRequest();
                case '(':
                    pos++;
                    List<SearchTerm> searchTerms = ParseOrExpression();
                    if (pos >= searchRequest.Length || searchRequest[pos] != ')')
                        throw new RequestParsingException();
                    pos++;
                    while (pos != searchRequest.Length && searchRequest[pos] == ' ') pos++;
                    return searchTerms;
                default:
                    throw new RequestParsingException();
            }
        }

        List<SearchTerm> ParseSimpleRequest()
        {
            while (++pos < searchRequest.Length && searchRequest[pos] == ' ') ;
            int pos0 = pos;
            int length = 0;
            while (pos < searchRequest.Length && searchRequest[pos] != '\"')
            {
                length++;
                pos++;
            }

            if (pos == searchRequest.Length)
                throw new RequestParsingException();

            String termKey = searchRequest.Substring(pos0, length);
            List<SearchTerm> searchTerms = new List<SearchTerm>();
            SearchTerm searchTerm = new SearchTerm(termKey);

            pos++;
            while (pos < searchRequest.Length && searchRequest[pos] == ' ') pos++;
            if (pos < searchRequest.Length - 1 && searchRequest[pos] == '/' && searchRequest[pos + 1] == '(')
            {
                pos++;
                pos0 = ++pos;
                length = 0;

                while (pos < searchRequest.Length && searchRequest[pos] != ')')
                {
                    length++;
                    pos++;
                }

                if (pos >= searchRequest.Length)
                    throw new RequestParsingException();

                searchTerm.Fields = searchRequest.Substring(pos0, length).Split(',');

                while (++pos != searchRequest.Length && searchRequest[pos] == ' ') ;
            }
            else
                searchTerm.Fields = new string[0];

            searchTerms.Add(searchTerm);
            return searchTerms;
        }

        #endregion

        #region Public methods

        public void ParseRequest(string Request)
        {
            pos = 0;
            searchRequest = Request;

            try
            {
                searchTerms = ParseOrExpression();
            }
            catch (RequestParsingException)
            {
                searchTerms = new List<SearchTerm>();
                throw new RequestParsingException();
            }

            foreach (SearchTerm term in searchTerms)
            {
                if (term.IsPresent)
                {
                    SearchTermInfo[] searchTermInfo = client.GetSearchTerms(term.Key + term.Name, 0);

                    term.Postings = new List<PostingsParams>();
                    foreach (SearchTermInfo term2 in searchTermInfo)
                        if (term.Trunc && term2.Text.StartsWith(term.Key + term.Name) || term2.Text == term.Key + term.Name)
                        {
                            PostingsParams Posting;
                            Posting.Text = term2.Text.Substring(term.Key.Length);

                            SearchPostingInfo[] searchPostingInfo = client.GetSearchPostings(term2.Text, 0, /*i + */1);
                            Posting.Mfn = new List<int>();

                            foreach (SearchPostingInfo posting in searchPostingInfo)
                                if (!Posting.Mfn.Contains(posting.Mfn) && (term.Fields.Length == 0 || term.Fields.Contains(posting.Tag)))
                                    Posting.Mfn.Add(posting.Mfn);

                            term.Postings.Add(Posting);
                        }
                }
                term.Fields = null;
            }
        }

        public string MarkEntries(String str, String word)
        {
            int index = 0;
            StringBuilder sb = new StringBuilder();
            int index0 = index;
            while (index < str.Length)
            {
                index = str.IndexOf(word, index0, StringComparison.CurrentCultureIgnoreCase);
                if (index < 8 || str.Substring(index - 8, 8) != "{select}")
                {
                    if (index == -1)
                        break;
                    sb.Append(str.Substring(index0, index - index0)).Append("{select}").Append(str.Substring(index, word.Length)).Append("{/select}");
                }
                else
                    sb.Append(str.Substring(index0, index - index0)).Append(str.Substring(index, word.Length));

                index += word.Length;
                index0 = index;
            }

            sb.Append(str.Substring(index0, str.Length - index0));

            return sb.ToString();
        }

        #endregion

        #region Object members

        public List<SearchTerm> searchTerms;
        public SearchScenario[] SearchScenarios;
        public SearchQualifier[] SearchQualifiers;

        #endregion
    }

    class RequestParsingException : ApplicationException
    {
        public RequestParsingException()
            : base()
        {
        }

        public RequestParsingException(string message)
            : base(message)
        {
        }

        public RequestParsingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}