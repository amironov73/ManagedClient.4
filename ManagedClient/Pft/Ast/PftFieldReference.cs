/* PftFieldReference.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Pft.Ast
{
    /// <summary>
    /// Ссылка на поле записи.
    /// </summary>
    [Serializable]
    public sealed class PftFieldReference
        : PftGroupMember
    {
        #region Properties

        public PftLeftHand LeftHand
        {
            get { return _leftHand; }
            set { _leftHand = ChangeChild(_leftHand, value); }
        }

        public PftRightHand RightHand
        {
            get { return _rightHand; }
            set { _rightHand = ChangeChild(_rightHand, value); }
        }

        #endregion

        #region Construction

        public PftFieldReference()
        {
        }

        public PftFieldReference
            (
                PftParser.FieldReferenceContext context
            )
        {
            LeftHand = (PftLeftHand) PftDispatcher.DispatchFormat(context.leftHand());
            RightHand = (PftRightHand) PftDispatcher.DispatchFormat(context.rightHand());
        }

        #endregion

        #region Private members

        private PftLeftHand _leftHand;

        private PftRightHand _rightHand;

        #endregion

        #region Public methods

        #endregion

        #region PftAst members

        #endregion

        #region PftGroupMember members

        #endregion
    }
}
