// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FieldMapper.cs
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace ManagedClient.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public sealed class FieldMapper
    {
        #region Properties

        /// <summary>
        /// Target <see cref="Type"/>
        /// </summary>
        public Type TargetType { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldMapper 
            ( 
                [NotNull] Type targetType 
            )
        {
            TargetType = targetType;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Convert <see cref="IrbisRecord"/> to <see cref="object"/>
        /// </summary>
        [NotNull]
        public object ToObject
            (
                [NotNull] IrbisRecord record,
                [NotNull] object target
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
