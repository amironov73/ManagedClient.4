// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GblBuilder.cs
 */

#region Using directives

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Gbl
{
    /// <summary>
    /// <para>Инструмент для упрощённого построения заданий на
    /// глобальную корректировку.</para>
    /// <para>Пример построения и выполнения задания:</para>
    /// <code>
    /// GblFinal final = new GblBuilder(client)
    ///        .Add("3079", "'1'")
    ///        .Delete("3011")
    ///        .Execute(new[] {30, 32, 34});
    /// Console.WriteLine ("Processed {0} records", final.RecordsProcessed);
    /// foreach (GblResult result in final.Results)
    /// {
    ///     Console.WriteLine(result);
    /// }
    /// </code>
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class GblBuilder
    {
        #region Properties

        /// <summary>
        /// Client connection.
        /// </summary>
        [CanBeNull]
        public ManagedClient64 Client { get; set; }

        /// <summary>
        /// Database name.
        /// </summary>
        [CanBeNull]
        public string Database { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public GblBuilder()
        {
            _commands = new List<GblItem>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public GblBuilder
            (
                [NotNull] ManagedClient64 client
            )
            : this ()
        {
            Client = client;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public GblBuilder
            (
                [NotNull] ManagedClient64 client, 
                [NotNull] string database
            )
            : this ()
        {
            Client = client;
            Database = database;
        }

        #endregion

        #region Private members

        private const string Filler = "XXXXXXXXXXXXXXXXX";
        private const string All = "*";

        private readonly List<GblItem> _commands;

        #endregion

        #region Public methods

        /// <summary>
        /// Add arbitrary command.
        /// </summary>
        [NotNull]
        public GblBuilder AddCommand
            (
                [NotNull] string command,
                string parameter1,
                string parameter2,
                string format1,
                string format2
            )
        {
            GblItem item = new GblItem
            {
                Command = VerifyCommand(command),
                Parameter1 = parameter1,
                Parameter2 = parameter2,
                Format1 = format1,
                Format2 = format2
            };
            _commands.Add(item);

            return this;
        }

        /// <summary>
        /// Verify the command.
        /// </summary>
        [NotNull]
        public string VerifyCommand
            (
                [NotNull] string command
            )
        {
            if (string.IsNullOrEmpty(command))
            {
                throw new ArgumentException();
            }

            return command;
        }

        /// <summary>
        /// Verify the field.
        /// </summary>
        [NotNull]
        public string VerifyField
            (
                [NotNull] string field
            )
        {
            if (string.IsNullOrEmpty(field))
            {
                throw new ArgumentException();
            }
// ReSharper disable ObjectCreationAsStatement
            if (!ReferenceEquals(Client, null))
            {
                // Чисто для проверки, что ссылка на поле задана верно
                new FieldReference("v" + field);
            }
// ReSharper restore ObjectCreationAsStatement
            return field;
        }

        /// <summary>
        /// Verify the repeat index.
        /// </summary>
        [CanBeNull]
        public string VerifyRepeat
            (
                [CanBeNull] string repeat
            )
        {
            // TODO do something?

            return repeat;
        }

        /// <summary>
        /// Verify the value.
        /// </summary>
        [NotNull]
        public string VerifyValue
            (
                [NotNull] string value
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException();
            }

            return value;
        }

        /// <summary>
        /// ADD command.
        /// </summary>
        [NotNull]
        public GblBuilder Add
            (
                [NotNull] string field,
                [NotNull] string value
            )
        {
            return AddCommand
                (
                    GblCommand.Add,
                    VerifyField(field),
                    All,
                    VerifyValue(value),
                    Filler
                );
        }

        /// <summary>
        /// ADD command.
        /// </summary>
        [NotNull]
        public GblBuilder Add
            (
                [NotNull] string field,
                [NotNull] string repeat,
                [NotNull] string value
            )
        {
            return AddCommand
                (
                    GblCommand.Add,
                    VerifyField(field),
                    VerifyRepeat(repeat),
                    VerifyValue(value),
                    Filler
                );
        }

        /// <summary>
        /// CHA command.
        /// </summary>
        public GblBuilder Change
            (
                [NotNull] string field,
                [NotNull] string fromValue,
                [NotNull] string toValue
            )
        {
            return AddCommand
                (
                    GblCommand.Change,
                    VerifyField(field),
                    All,
                    VerifyValue(fromValue),
                    VerifyValue(toValue)
                );
        }

        /// <summary>
        /// CHA command.
        /// </summary>
        [NotNull]
        public GblBuilder Change
            (
                [NotNull] string field,
                [NotNull] string repeat,
                [NotNull] string fromValue,
                [NotNull] string toValue
            )
        {
            return AddCommand
                (
                    GblCommand.Change,
                    VerifyField(field),
                    VerifyRepeat(repeat),
                    VerifyValue(fromValue),
                    VerifyValue(toValue)
                );
        }

        /// <summary>
        /// DEL command.
        /// </summary>
        public GblBuilder Delete
            (
                [NotNull] string field,
                [NotNull] string repeat
            )
        {
            return AddCommand
                (
                    GblCommand.Delete,
                    VerifyField(field),
                    VerifyRepeat(repeat),
                    Filler,
                    Filler
                );
        }

        /// <summary>
        /// DEL command.
        /// </summary>
        [NotNull]
        public GblBuilder Delete
            (
                [NotNull] string field
            )
        {
            return AddCommand
                (
                    GblCommand.Delete,
                    VerifyField(field),
                    All,
                    Filler,
                    Filler
                );
        }

        /// <summary>
        /// FI command.
        /// </summary>
        [NotNull]
        public GblBuilder Fi()
        {
            return AddCommand
                (
                    GblCommand.Fi,
                    Filler,
                    Filler,
                    Filler,
                    Filler
                );
        }

        /// <summary>
        /// IF command.
        /// </summary>
        [NotNull]
        public GblBuilder If
            (
                [NotNull] string condition
            )
        {
            return AddCommand
                (
                    GblCommand.If,
                    VerifyValue(condition),
                    Filler,
                    Filler,
                    Filler
                );
        }

        /// <summary>
        /// REP command.
        /// </summary>
        [NotNull]
        public GblBuilder Replace
            (
                [NotNull] string field,
                [NotNull] string repeat,
                [NotNull] string toValue
            )
        {
            return AddCommand
                (
                    GblCommand.Replace,
                    VerifyField(field),
                    VerifyRepeat(repeat),
                    VerifyValue(toValue),
                    Filler
                );
        }

        /// <summary>
        /// REP command.
        /// </summary>
        [NotNull]
        public GblBuilder Replace
            (
                [NotNull] string field,
                [NotNull] string toValue
            )
        {
            return AddCommand
                (
                    GblCommand.Replace,
                    VerifyField(field),
                    All,
                    VerifyValue(toValue),
                    Filler
                );
        }

        /// <summary>
        /// Set client connection.
        /// </summary>
        [NotNull]
        public GblBuilder SetClient
            (
                [NotNull] ManagedClient64 client
            )
        {
            if (ReferenceEquals(client,null))
            {
                throw new ArgumentException();
            }

            Client = client;

            return this;
        }

        /// <summary>
        /// Set the database name.
        /// </summary>
        [NotNull]
        public GblBuilder SetDatabase
            (
                [NotNull] string database
            )
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentException();
            }

            Database = database;

            return this;
        }

        /// <summary>
        /// Convert the <see cref="GblBuilder"/>
        /// to the array of <see cref="GblItem"/>.
        /// </summary>
        [NotNull]
        public GblItem[] ToCommands()
        {
            return _commands.ToArray();
        }

        /// <summary>
        /// Execute the global correction on the entire database.
        /// </summary>
        [NotNull]
        public GblFinal Execute ()
        {
            return new GlobalCorrector
                (
                    Client,
                    Database
                )
                .ProcessWholeDatabase
                (
                    ToCommands()
                );
        }

        /// <summary>
        /// Execute the global correction on the found records.
        /// </summary>
        [NotNull]
        public GblFinal Execute
            (
                [NotNull] string searchExpression
            )
        {
            return new GlobalCorrector
                (
                    Client,
                    Database
                )
                .ProcessSearchResult
                (
                    searchExpression,
                    ToCommands()
                );
        }

        /// <summary>
        /// Execute the global correction on the interval
        /// of records.
        /// </summary>
        [NotNull]
        public GblFinal Execute
            (
                int fromMfn,
                int toMfn
            )
        {
            return new GlobalCorrector
                (
                    Client,
                    Database
                )
                .ProcessInterval
                (
                    fromMfn,
                    toMfn,
                    ToCommands()
                );
        }

        /// <summary>
        /// Execute the global correction on specified recordset.
        /// </summary>
        [NotNull]
        public GblFinal Execute
            (
                [NotNull] IEnumerable<int> recordset
            )
        {
            return new GlobalCorrector
                (
                    Client,
                    Database
                )
                .ProcessRecordset
                (
                    recordset,
                    ToCommands()
                );
        }
        
        #endregion
    }
}
