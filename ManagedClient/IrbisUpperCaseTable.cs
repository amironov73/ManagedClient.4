/* IrbisUpperCaseTable.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient
{
    [Serializable]
    public sealed class IrbisUpperCaseTable
    {
        #region Properties
        #endregion

        #region Constructor

        public IrbisUpperCaseTable
            (
                Encoding encoding, 
                byte[] firstTable, 
                byte[] secondTable
            )
        {
            _encoding = encoding;
            _firstTable = firstTable;
            _secondTable = secondTable;
        }

        public IrbisUpperCaseTable
            (
                ManagedClient64 client,
                string fileName
            )
        {
            
        }

        public IrbisUpperCaseTable
            (
                ManagedClient64 client
            )
            : this ( client, "ISISUC.TAB" )
        {
            
        }

        #endregion

        #region Private members

        private readonly Encoding _encoding;

        private readonly byte[] _firstTable;

        private readonly byte[] _secondTable;

        #endregion

        #region Public methods

        public char ToUpper
            (
                char c
            )
        {
            return c;
        }

        public string ToUpper
            (
                string s
            )
        {
            return s;
        }

        #endregion
    }
}
