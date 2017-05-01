// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SubFieldCollection.cs -- коллекция подполей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Коллекция подполей.
    /// Отличается тем, что принципиально не принимает
    /// значения <c>null</c>.
    /// </summary>
    [Serializable]
    [MoonSharpUserData]
    [ClassInterface(ClassInterfaceType.None)]
    [DebuggerDisplay("Count = {Count}")]
    public sealed class SubFieldCollection
        : Collection<SubField>,
        IHandmadeSerializable
    {
        #region Public methods

        /// <summary>
        /// Добавление в коллекцию нескольких подполей сразу
        /// </summary>
        public void AddRange
            (
                IEnumerable<SubField> subFields
            )
        {
            foreach (SubField subField in subFields)
            {
                Add(subField);
            }
        }

        /// <summary>
        /// Поиск с помощью предиката.
        /// </summary>
        public SubField Find
            (
                Predicate<SubField> predicate
            )
        {
            return this
                .FirstOrDefault
                (
                    subField => predicate(subField)
                );
        }

        /// <summary>
        /// Отбор с помощью предиката.
        /// </summary>
        public SubField[] FindAll
            (
                Predicate<SubField> predicate
            )
        {
            return this
                .Where(subField => predicate(subField))
                .ToArray();
        }

        #region Ручная сериализация

        /// <summary>
        /// Сохранение в поток
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WritePackedInt32(Count);
            foreach (SubField subField in this)
            {
                subField.SaveToStream(writer);
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
            Clear();
            int count = reader.ReadPackedInt32();
            for (int i = 0; i < count; i++)
            {
                SubField subField = new SubField();
                subField.ReadFromStream(reader);
                Add(subField);
            }
        }

        #endregion

        #endregion

        #region Collection<T> members

        protected override void InsertItem
            (
                int index,
                SubField item
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
                SubField item
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
