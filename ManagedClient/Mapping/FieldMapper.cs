/* FieldMapper.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Mapping
{
    public sealed class FieldMapper
    {
        #region Properties

        public Type TargetType { get; private set; }

        #endregion

        #region Construction

        public FieldMapper 
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
                IrbisRecord record,
                object target
            )
        {
            if (ReferenceEquals(record, null))
            {
                throw new ArgumentNullException("record");
            }
            if (ReferenceEquals(target, null))
            {
                throw new ArgumentNullException("target");
            }
            return target;
        }

        #endregion

    }
}
