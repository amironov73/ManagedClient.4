// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GlobalCorrector.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Gbl
{
    /// <summary>
    /// Обёртка для облегчения выполнения глобальной корректировки
    /// порциями (например, по 100 записей за раз).
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class GlobalCorrector
    {
        #region Events

        /// <summary>
        /// Вызывается после обработки очередной порции записей
        /// и в конце общей обработки.
        /// </summary>
        public event EventHandler<GblEventArgs> PortionProcessed;

        #endregion

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

        /// <summary>
        /// Размер порции. По умолчанию 100 шт.
        /// </summary>
        public int ChunkSize
        {
            get { return _chunkSize; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _chunkSize = value;
            }
        }

        /// <summary>
        /// Размер порции, устанавливаемый для вновь создаваемых
        /// экземпляров <see cref="GlobalCorrector"/>.
        /// </summary>
        public static int DefaultChunkSize
        {
            get { return _defaultChunkSize; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _defaultChunkSize = value;
            }
        }

        /// <summary>
        /// Актуализировать ли словарь. По умолчанию <c>true</c>.
        /// </summary>
        public bool UpdateIf { get; set; }

        /// <summary>
        /// Выполнять ли autoin.gbl.
        /// </summary>
        public bool Autoin { get; set; }

        /// <summary>
        /// Выполнять ли формально-логический контроль.
        /// </summary>
        public bool Flc { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public GlobalCorrector()
        {
            ChunkSize = DefaultChunkSize;
            UpdateIf = true;
            Autoin = false;
            Flc = false;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public GlobalCorrector
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
        public GlobalCorrector
            (
                int chunkSize
            )
            : this ()
        {
            ChunkSize = chunkSize;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public GlobalCorrector
            (
                [NotNull] string database
            )
            : this ()
        {
            Database = database;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public GlobalCorrector
            (
                [NotNull] ManagedClient64 client, 
                [NotNull] string database
            )
            : this()
        {
            Client = client;
            Database = database;
        }

        #endregion

        #region Private members

        private static int _defaultChunkSize = 100;

        private int _chunkSize;

        private void _VerifyPreconditions()
        {
            if (ReferenceEquals(Client, null))
            {
                throw new ArgumentException();
            }
            if (ReferenceEquals(Database, null))
            {
                Database = Client.Database;
            }
        }

        [NotNull]
        private GblFinal _CreateTotal()
        {
            GblFinal result = new GblFinal
            {
                Results = new List<GblResult>(),
                TimeStarted =  DateTime.Now
            };

            return result;
        }

        private bool _Update
            (
                [NotNull] GblFinal final,
                [CanBeNull] IEnumerable<GblResult> results
            )
        {
            final.TimeElapsed = DateTime.Now - final.TimeStarted;

            if (!ReferenceEquals(results, null))
            {
                foreach (GblResult result in results)
                {
                    final.RecordsProcessed++;
                    if (result.Success)
                    {
                        final.RecordsSucceeded++;
                    }
                    else
                    {
                        final.RecordsFailed++;
                    }
                    final.Results.Add(result);
                }
            }

            EventHandler<GblEventArgs> handler = PortionProcessed;
            if (!ReferenceEquals(handler, null))
            {
                GblEventArgs args = new GblEventArgs
                {
                    RecordsFailed = final.RecordsFailed,
                    RecordsProcessed = final.RecordsProcessed,
                    RecordsSucceeded = final.RecordsSucceeded,
                    TimeStarted = final.TimeStarted,
                    TimeElapsed = final.TimeElapsed
                };
                handler(this, args);
                if (args.Cancel)
                {
                    final.Canceled = true;

                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Обработать базу данных в целом.
        /// </summary>
        [NotNull]
        public GblFinal ProcessWholeDatabase
            (
                [NotNull] GblItem[] gbl
            )
        {
            _VerifyPreconditions();
            int maxMfn = Client.ThrowIfNull("Client")
                .GetMaxMfn() - 1;

            return ProcessInterval
                (
                    1,
                    maxMfn,
                    gbl
                );
        }

        /// <summary>
        /// Обработать результат поиска.
        /// </summary>
        [NotNull]
        public GblFinal ProcessSearchResult
            (
                [NotNull] string searchExpression,
                [NotNull] GblItem[] gbl
            )
        {
            _VerifyPreconditions();
            int[] mfns = Client.ThrowIfNull("Client")
                .Search(searchExpression);

            return ProcessRecordset
                (
                    mfns, 
                    gbl
                );
        }

        /// <summary>
        /// Обработать интервал записей.
        /// </summary>
        [NotNull]
        public GblFinal ProcessInterval
            (
                int fromMfn,
                int toMfn,
                [NotNull] GblItem[] gbl
            )
        {
            if (fromMfn <= 0
                || fromMfn > toMfn)
            {
                throw new ArgumentOutOfRangeException();
            }

            _VerifyPreconditions();

            int maxMfn = Client.ThrowIfNull("Client")
                .GetMaxMfn() - 1;
            toMfn = Math.Min(maxMfn, toMfn);

            GblFinal finalResult = _CreateTotal();
            finalResult.RecordsSupposed = toMfn - fromMfn + 1;

            int startMfn = fromMfn;

            while (startMfn <= toMfn)
            {
                int amount = Math.Min
                    (
                        toMfn - startMfn + 1, 
                        ChunkSize
                    );
                int endMfn = startMfn + amount - 1;

                try
                {
                    GblResult[] intermediateResult = Client
                        .ThrowIfNull("Client")
                        .GlobalAdjustment
                        (
                            null,
                            0,
                            0,
                            startMfn,
                            endMfn,
                            null,
                            UpdateIf,
                            Flc,
                            Autoin,
                            gbl
                        );
                    if (_Update
                        (
                            finalResult,
                            intermediateResult
                        ))
                    {
                        break; // canceled
                    }
                }
                catch (Exception ex)
                {
                    finalResult.Exception = ex;
                    break;
                }

                startMfn = endMfn + 1;
            }

            _Update(finalResult, null);

            return finalResult;
        }

        /// <summary>
        /// Обработать явно (вручную) заданное множество записей.
        /// </summary>
        [NotNull]
        public GblFinal ProcessRecordset
            (
                [NotNull] IEnumerable<int> recordset,
                [NotNull] GblItem[] gbl
            )
        {
            _VerifyPreconditions();
            List<int> list = recordset.ToList();
            GblFinal finalResult = _CreateTotal();
            finalResult.RecordsSupposed = list.Count;

            while (list.Count > 0)
            {
                int[] portion = list.Take(ChunkSize).ToArray();
                list = list.Skip(ChunkSize).ToList();
                try
                {
                    GblResult[] intermediateResult = Client
                        .ThrowIfNull("Client")
                        .GlobalAdjustment
                        (
                            null,
                            0,
                            0,
                            0,
                            0,
                            portion,
                            UpdateIf,
                            Flc,
                            Autoin,
                            gbl
                        );
                    if (_Update
                        (
                            finalResult,
                            intermediateResult
                        ))
                    {
                        break; // canceled
                    }
                }
                catch (Exception ex)
                {
                    finalResult.Exception = ex;
                    break;
                }
            }
            
            _Update(finalResult, null);

            return finalResult;
        }

        #endregion
    }
}
