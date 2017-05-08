// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisMorphologyProvider.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using ManagedClient.Query;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Morphology
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IrbisMorphologyProvider
        : MorphologyProvider
    {
        #region Properties

        /// <summary>
        /// Client connection.
        /// </summary>
        [CanBeNull]
        public ManagedClient64 Client { get; set; }

        /// <summary>
        /// Search prefix.
        /// </summary>
        [CanBeNull]
        public string Prefix
        {
            get { return _prefix; }
            set { _prefix = value; }
        }

        /// <summary>
        /// Database name.
        /// </summary>
        [CanBeNull]
        public string Database
        {
            get { return _database; }
            set { _database = value; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisMorphologyProvider()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisMorphologyProvider
            (
                [NotNull] ManagedClient64 client
            )
        {
            if (ReferenceEquals(client, null))
            {
                throw new ArgumentNullException("client");
            }

            Client = client;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisMorphologyProvider
            (
                [NotNull] string prefix, 
                [NotNull] string database
            )
        {
            if (string.IsNullOrEmpty(prefix))
            {
                throw new ArgumentNullException("prefix");
            }
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException("database");
            }

            _prefix = prefix;
            _database = database;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisMorphologyProvider
            (
                [NotNull] string prefix, 
                [NotNull] string database, 
                [NotNull] ManagedClient64 client
            )
        {
            if (string.IsNullOrEmpty(prefix))
            {
                throw new ArgumentNullException("prefix");
            }
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException("database");
            }
            if (ReferenceEquals(client, null))
            {
                throw new ArgumentNullException("client");
            }

            _prefix = prefix;
            _database = database;
            Client = client;
        }

        #endregion

        #region Private members

        private string _prefix = "K=";

        private string _database = "MORPH";

        private QAst _MakeAst
            (
                [NotNull] string[] array,
                [NotNull] IEnumerable<string> tags
            )
        {
            if (array.Length == 1)
            {
                return new QAstEntry(tags)
                {
                    Expression = Prefix + array[0],
                    Quoted = true
                };
            }
            QAstPlusOperator result = new QAstPlusOperator
            {
                LeftOperand = new QAstEntry
                {
                    Expression = Prefix + array[0],
                    Quoted = true,
                },
                RightOperand = _MakeAst
                    (
                        array.Skip(1).ToArray(),
                        tags
                    )
            };
            result.Children.Add(result.LeftOperand);
            result.Children.Add(result.RightOperand);

            return result;
        }

        private bool _QueryWalker
            (
                [NotNull] QAst ast
            )
        {
            string prefix = Prefix.ThrowIfNull("Prefix");

            for (int i = 0; i < ast.Children.Count; i++)
            {
                QAst child = ast.Children[i];
                QAstEntry entry = child as QAstEntry;
                if (entry != null)
                {
                    if (entry.Expression.StartsWith(prefix)
                        && entry.Ending == EndingKind.NoTrim)
                    {
                        string word = string.IsNullOrEmpty(prefix)
                            ? entry.Expression
                            : entry.Expression.Substring(prefix.Length);
                        MorphologyEntry[] entries = FindWord(word);
                        string[] flatten = Flatten(word, entries);
                        if (flatten.Length > 1)
                        {
                            QAstParen paren = new QAstParen();
                            QAst newAst = _MakeAst(flatten, entry.Tags);
                            paren.Children.Add(newAst);
                            ast.Children[i] = paren;

                            return false;
                        }
                    }
                }
            }

            return true;
        }

        #endregion

        #region MorphologyProvider members

        /// <inheritdoc cref="MorphologyProvider.FindWord"/>
        public override MorphologyEntry[] FindWord
            (
                string word
            )
        {
            ManagedClient64 client = Client.ThrowIfNull("Client");
            string database = Database.ThrowIfNull("Database");


            client.PushDatabase(database);
            try
            {
                IrbisRecord[] records = client.SearchRead
                    (
                        "\"K={0}\"",
                        word
                    );
                MorphologyEntry[] result = records
                    .Select(r => MorphologyEntry.Parse(r))
                    .ToArray();

                return result;
            }
            finally
            {
                client.PopDatabase();
            }
        }

        /// <inheritdoc cref="MorphologyProvider.RewriteQuery"/>
        public override string RewriteQuery
            (
                string queryExpression
            )
        {
            QueryManager manager = new QueryManager();
            QAst ast = manager.ParseQuery(queryExpression);
            ast.Walk(_QueryWalker);

            string result = manager.SerializeAst(ast);

            return result;
        }

        #endregion
    }
}
