// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SubFieldMapper.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Mapping
{
    public sealed class SubFieldMapper
    {
        #region Properties

        public Type TargetType { get; private set; }

        #endregion

        #region Construction

        public SubFieldMapper 
            ( 
                Type targetType 
            )
        {
            TargetType = targetType;
        }

        #endregion

        #region Public methods

        public object ToObject
            (
                RecordField field,
                object target
            )
        {
            if ( ReferenceEquals ( field, null ) )
            {
                throw new ArgumentNullException("field");
            }
            if ( ReferenceEquals ( target, null ) )
            {
                throw new ArgumentNullException("target");
            }
            return target;
        }

        #endregion
    }
}
