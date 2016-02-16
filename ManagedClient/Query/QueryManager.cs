/* QueryManager.cs
 */

#region Using directives

using System;
using System.Collections.Generic;

using Antlr4.Runtime;

#endregion

namespace ManagedClient.Query
{
    /// <summary>
    /// Сервисы переписывания и верификации запросов.
    /// </summary>
    public sealed class QueryManager
    {
        #region Properties

        public List<string> QueryHistory { get { return _queryHistory; } }

        #endregion

        #region Construction

        public QueryManager()
        {
            _queryHistory = new List<string>();
        }

        #endregion

        #region Private members

        private readonly List<string> _queryHistory;

        #endregion

        #region Public methods

        public QAst ParseQuery
            (
                string queryText
            )
        {
            if (string.IsNullOrEmpty(queryText))
            {
                throw new ArgumentNullException("queryText");
            }

            AntlrInputStream stream = new AntlrInputStream(queryText);
            IrbisQueryLexer lexer = new IrbisQueryLexer(stream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            IrbisQueryParser parser = new IrbisQueryParser(tokens);
            IrbisQueryParser.ProgramContext tree = parser.program();
            Console.WriteLine(tree.ToStringTree(parser));
            QAst result = new QAstProgram(tree);
            return result;
        }

        public string SerializeAst
            (
                QAst root
            )
        {
            if (ReferenceEquals(root, null))
            {
                throw new ArgumentNullException("root");
            }

            return root.ToString();
        }

        #endregion
    }
}
