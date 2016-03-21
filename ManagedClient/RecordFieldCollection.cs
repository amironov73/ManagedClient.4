/* RecordFieldCollection.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Коллекция полей записи.
    /// Отличается тем, что принципиально
    /// не принимает значения <c>null</c>.
    /// </summary>
    [Serializable]
    [MoonSharpUserData]
    [ClassInterface(ClassInterfaceType.None)]
    public sealed class RecordFieldCollection
        : Collection<RecordField>
    {
        #region Public methods

        public void AddRange
            (
                IEnumerable<RecordField> fields
            )
        {
            foreach (RecordField field in fields)
            {
                Add(field);
            }
        }

        public RecordField Find
            (
                Predicate<RecordField> predicate
            )
        {
            return this
                .FirstOrDefault(field => predicate (field));
        }

        public RecordField[] FindAll
            (
                Predicate<RecordField> predicate
            )
        {
            return this
                .Where(field => predicate(field))
                .ToArray();
        }

        #endregion

        #region Collection<T> members

        protected override void InsertItem
            (
                int index, 
                RecordField item
            )
        {
            if (ReferenceEquals(item, null))
            {
                throw new ArgumentNullException("item");
            }

            base.InsertItem(index, item);
        }

        protected override void SetItem
            (
                int index, 
                RecordField item
            )
        {
            if (ReferenceEquals(item, null))
            {
                throw new ArgumentNullException("item");
            }

            base.SetItem(index, item);
        }

        #endregion
    }
}
