// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisAlphabetTable.cs -- ISISAC.TAB
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
    /// ISISAC.TAB
    /// </summary>
    [Serializable]
    public sealed class IrbisAlphabetTable
    {
        #region Properties

        #endregion

        #region Constructor

        public IrbisAlphabetTable
            (
                Encoding encoding, 
                byte[] table
            )
        {
            _encoding = encoding;
            _table = table;
        }

        public IrbisAlphabetTable
            (
                ManagedClient64 client,
                string fileName
            )
        {
            
        }

        public IrbisAlphabetTable
            (
                ManagedClient64 client
            )
            : this ( client, "ISISAC.TAB" )
        {
            
        }

        #endregion

        #region Private members

        private readonly Encoding _encoding;

        private readonly byte[] _table;

        #endregion

        #region Public methods

        public bool IsAlpha
            (
                char c
            )
        {
            return false;
        }

        #endregion
    }
}
