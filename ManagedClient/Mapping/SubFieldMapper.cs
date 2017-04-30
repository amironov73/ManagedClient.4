// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SubFieldMapper.cs
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
    public sealed class SubFieldMapper
    {
        #region Properties

        /// <summary>
        /// Target <see cref="Type"/>.
        /// </summary>
        [NotNull]
        public Type TargetType { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SubFieldMapper 
            ( 
                [NotNull] Type targetType 
            )
        {
            TargetType = targetType;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Convert <see cref="RecordField"/> to <see cref="object"/>.
        /// </summary>
        [NotNull]
        public object ToObject
            (
                [NotNull] RecordField field,
                [NotNull] object target
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
