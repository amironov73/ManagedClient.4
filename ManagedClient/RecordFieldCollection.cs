/* RecordFieldCollection.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

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

        /// <summary>
        /// Добавление нескольких полей.
        /// </summary>
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

        /// <summary>
        /// Поиск первого вхождения с помощью предиката.
        /// </summary>
        public RecordField Find
            (
                Predicate<RecordField> predicate
            )
        {
            return this
                .FirstOrDefault(field => predicate (field));
        }

        /// <summary>
        /// Поиск всех вхождений с помощью предиката.
        /// </summary>
        public RecordField[] FindAll
            (
                Predicate<RecordField> predicate
            )
        {
            return this
                .Where(field => predicate(field))
                .ToArray();
        }

        #region Ручная сериализация

        /// <summary>
        /// Сохранение в поток.
        /// </summary>
        public void SaveToStream
            (
                [NotNull] BinaryWriter writer
            )
        {
            writer.WritePackedInt32(Count);

            foreach (RecordField field in this)
            {
                field.SaveToStream(writer);
            }
        }

        /// <summary>
        /// Считывание из потока.
        /// </summary>
        public void ReadFromStream
            (
                [NotNull] BinaryReader reader
            )
        {
            int count = reader.ReadPackedInt32();

            for (int i = 0; i < count; i++)
            {
                RecordField field = RecordField.ReadFromStream(reader);
                Add(field);
            }
        }

        #endregion

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
