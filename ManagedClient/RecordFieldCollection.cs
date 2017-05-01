// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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
        : Collection<RecordField>,
        IHandmadeSerializable
    {
        #region Public methods

        /// <summary>
        /// Добавление нескольких полей.
        /// </summary>
        public void AddRange
            (
                [NotNull] IEnumerable<RecordField> fields
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
        [CanBeNull]
        public RecordField Find
            (
                [NotNull] Predicate<RecordField> predicate
            )
        {
            return this
                .FirstOrDefault(field => predicate (field));
        }

        /// <summary>
        /// Поиск всех вхождений с помощью предиката.
        /// </summary>
        [NotNull]
        public RecordField[] FindAll
            (
                [NotNull] Predicate<RecordField> predicate
            )
        {
            return this
                .Where(field => predicate(field))
                .ToArray();
        }

        #region Ручная сериализация

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WritePackedInt32(Count);

            foreach (RecordField field in this)
            {
                field.SaveToStream(writer);
            }
        }

        /// <inheritdoc cref="IHandmadeSerializable.ReadFromStream"/>
        public void ReadFromStream
            (
                BinaryReader reader
            )
        {
            int count = reader.ReadPackedInt32();

            for (int i = 0; i < count; i++)
            {
                RecordField field = new RecordField();
                field.ReadFromStream(reader);
                Add(field);
            }
        }

        #endregion

        #endregion

        #region Collection<T> members

        /// <inheritdoc cref="Collection{T}.InsertItem"/>
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

        /// <inheritdoc cref="Collection{T}.SetItem"/>
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
