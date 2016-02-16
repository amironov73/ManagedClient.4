/* PftFieldPresense.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Тест на наличие/отсутствие поля/подполя.
    /// </summary>
    [Serializable]
    public sealed class PftFieldPresence
        : PftCondition
    {
        #region Properties

        /// <summary>
        /// Должно присутствовать (иначе должно отсутствовать).
        /// </summary>
        public bool MustPresent { get; set; }

        /// <summary>
        /// Ссылка на поле.
        /// </summary>
        public PftFieldReference Field { get; set; }

        #endregion

        #region Construction

        public PftFieldPresence()
        {
        }

        public PftFieldPresence(PftParser.ConditionFieldContext node)
            : base(node)
        {
            PftParser.FieldPresenseContext context = node.fieldPresense();
            if (context.P() != null)
            {
                MustPresent = true;
            }
            Field = new PftFieldReference(context.fieldReference());
            Children.Add(Field);
        }

        #endregion

        #region PftCondition members

        public override bool Evaluate
            (
                PftContext context
            )
        {
            string formatted = Field.Field.FormatSingle(context.Record);
            bool result = string.IsNullOrEmpty(formatted) != MustPresent;
            return result;
        }

        #endregion
    }
}