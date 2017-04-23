// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MorphologyEngine.cs
 */

#region Using directives

// No usings

#endregion

namespace ManagedClient.Morphology
{
    public sealed class MorphologyEngine
    {
        #region Properties

        public ManagedClient64 Client
        {
            get { return _client; }
        }

        public MorphologyProvider Provider
        {
            get { return _provider; }
        }

        #endregion

        #region Construction

        public MorphologyEngine
            (
                ManagedClient64 client
            )
        {
            _client = client;
            _provider = new IrbisMorphologyProvider
                (
                    client
                );
        }

        public MorphologyEngine
            (
                ManagedClient64 client,
                MorphologyProvider provider
            )
        {
            _client = client;
            _provider = provider;
        }

        public MorphologyEngine
            (
                ManagedClient64 client,
                string prefix,
                string database
            )
        {
            _client = client;
            _provider = new IrbisMorphologyProvider
                (
                    prefix,
                    database,
                    client
                );
        }

        public MorphologyEngine
            (
                MorphologyProvider provider
            )
        {
            _provider = provider;
        }

        #endregion

        #region Private members

        private readonly MorphologyProvider _provider;

        private readonly ManagedClient64 _client;

        #endregion

        #region Public methods

        public string RewriteQuery
            (
                string queryText
            )
        {
            return Provider.RewriteQuery(queryText);
        }

        public int[] Search
            (
                string format,
                params object[] args
            )
        {
            string original = string.Format(format, args);
            string rewritten = RewriteQuery(original);
            return _client.Search(rewritten);
        }

        public IrbisRecord[] SearchRead
            (
                string format,
                params object[] args
            )
        {
            string original = string.Format(format, args);
            string rewritten = RewriteQuery(original);
            return _client.SearchRead(rewritten);
        }

        public IrbisRecord SearchReadOneRecord
            (
                string format,
                params object[] args
            )
        {
            string original = string.Format(format, args);
            string rewritten = RewriteQuery(original);
            return _client.SearchReadOneRecord(rewritten);
        }

        public string[] SearchFormat
            (
                string expression,
                string format
            )
        {
            string rewritten = RewriteQuery(expression);
            return _client.SearchFormat(rewritten, format);
        }

        #endregion
    }
}
