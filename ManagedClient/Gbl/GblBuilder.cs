﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GblBuilder.cs
 */

#region Using directives

using System;
using System.Collections.Generic;

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
    public sealed class GblBuilder
    {
        #region Properties

        public ManagedClient64 Client { get; set; }

        public string Database { get; set; }

        #endregion

        #region Construction

        public GblBuilder()
        {
            _commands = new List<GblItem>();
        }

        public GblBuilder
            (
                ManagedClient64 client
            )
            : this ()
        {
            Client = client;
        }

        public GblBuilder
            (
                ManagedClient64 client, 
                string database
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

        public GblBuilder AddCommand
            (
                string command,
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

        public string VerifyCommand
            (
                string command
            )
        {
            if (string.IsNullOrEmpty(command))
            {
                throw new ArgumentException();
            }
            return command;
        }

        public string VerifyField
            (
                string field
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

        public string VerifyRepeat
            (
                string repeat
            )
        {
            return repeat;
        }

        public string VerifyValue
            (
                string value
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException();
            }

            return value;
        }

        public GblBuilder Add
            (
                string field,
                string value
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

        public GblBuilder Add
            (
                string field,
                string repeat,
                string value
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

        public GblBuilder Change
            (
                string field,
                string fromValue,
                string toValue
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

        public GblBuilder Change
            (
                string field,
                string repeat,
                string fromValue,
                string toValue
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

        public GblBuilder Delete
            (
                string field,
                string repeat
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

        public GblBuilder Delete
            (
                string field
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

        public GblBuilder If
            (
                string condition
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

        public GblBuilder Replace
            (
                string field,
                string repeat,
                string toValue
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

        public GblBuilder Replace
            (
                string field,
                string toValue
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

        public GblBuilder SetClient
            (
                ManagedClient64 client
            )
        {
            if (ReferenceEquals(client,null))
            {
                throw new ArgumentException();
            }
            Client = client;
            return this;
        }

        public GblBuilder SetDatabase
            (
                string database
            )
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentException();
            }
            Database = database;
            return this;
        }

        public GblItem[] ToCommands()
        {
            return _commands.ToArray();
        }

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

        public GblFinal Execute
            (
                string searchExpression
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

        public GblFinal Execute
            (
                IEnumerable<int> recordset
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
