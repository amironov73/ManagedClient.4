/* RecordExtractor.cs -- извлечение списка записей из ИРБИС.
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using ManagedClient.Output;
using ManagedClient.Ranges;

#endregion

namespace ManagedClient.SubBasing
{

    /// <summary>
    /// Извлечение списка записей из ИРБИС.
    /// </summary>
    public sealed class RecordExtractor
    {
        #region Properties

        /// <summary>
        /// Клиент для подключения к ИРБИС64.
        /// </summary>
        public ManagedClient64 Client { get { return _client; } }

        /// <summary>
        /// Отладочная печать.
        /// </summary>
        public AbstractOutput Output { get { return _output; } }

        /// <summary>
        /// Надо ли считывать записи.
        /// </summary>
        public bool ReadRecords { get; set; }

        /// <summary>
        /// Можно ли считывать удаленные записи.
        /// </summary>
        public bool AllowDeleted { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public RecordExtractor
            (
                ManagedClient64 client
            )
            : this(client, null)
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public RecordExtractor
            (
                ManagedClient64 client,
                AbstractOutput output
            )
        {
            if (ReferenceEquals(client, null))
            {
                throw new ArgumentNullException("client");
            }
            if (ReferenceEquals(output, null))
            {
                output = AbstractOutput.Null;
            }

            ReadRecords = false;

            _client = client;
            _output = output;
        }

        #endregion

        #region Private members

        private readonly ManagedClient64 _client;

        private readonly AbstractOutput _output;

        private int _maxMfn;

        private void _AddRecord
            (
                Dictionary<int,IrbisRecord> list,
                int mfn
            )
        {
            if ((mfn > 0)
                && !list.ContainsKey(mfn))
            {
                IrbisRecord record =
                    ReadRecords 
                        ? Client.ReadRecord(mfn)
                        : new IrbisRecord { Mfn = mfn };
                if (!record.Deleted
                    || AllowDeleted)
                {
                    list.Add
                        (
                            mfn,
                            record
                        );
                }
            }
        }

        // ReSharper disable ParameterTypeCanBeEnumerable.Local
        private void _SearchByMfn
            (
                Dictionary<int,IrbisRecord> result,
                NumberRangeCollection ranges
            )
        {
            foreach (NumberText number in ranges)
            {
                int mfn = int.Parse(number.ToString());
                if ((mfn > 0) && (mfn < _maxMfn))
                {
                    _AddRecord
                        (
                            result,
                            mfn
                        );
                }
            }
        }
        // ReSharper restore ParameterTypeCanBeEnumerable.Local

        // ReSharper disable ParameterTypeCanBeEnumerable.Local
        private void _SearchBySequential
            (
                Dictionary<int, IrbisRecord> result,
                string prefix,
                NumberRangeCollection ranges
            )
        {
            foreach (NumberText number in ranges)
            {
                if (ReadRecords)
                {
                    IrbisRecord record = Client.SearchReadOneRecord
                        (
                            "\"{0}{1}\"",
                            prefix,
                            number
                        );
                    if ((record != null)
                        && !result.ContainsKey(record.Mfn))
                    {
                        result.Add
                            (
                                record.Mfn,
                                record
                            );
                    }
                }
                else
                {
                    int[] found = Client.Search
                        (
                            "\"{0}{1}\"",
                            prefix,
                            number
                        );
                    if ((found != null)
                        &&(found.Length != 0))
                    {
                        _AddRecord
                            (
                                result,
                                found[0]
                            );
                    }
                }
            }
        }
        // ReSharper restore ParameterTypeCanBeEnumerable.Local

        private void _SearchByQuery
            (
                Dictionary<int, IrbisRecord> result,
                string statement
            )
        {
            if (ReadRecords)
            {
                IrbisRecord[] records = Client.SearchRead
                    (
                        statement
                    );
                foreach (IrbisRecord record in records)
                {
                    if (!result.ContainsKey(record.Mfn))
                    {
                        result.Add
                            (
                                record.Mfn,
                                record
                            );
                    }
                }
            }
            else
            {
                int[] found = Client.Search
                    (
                        statement
                    );
                foreach (int mfn in found)
                {
                    _AddRecord
                        (
                            result,
                            mfn
                        );
                }
            }
        }

        private void _SearchByDeep
            (
                Dictionary<int, IrbisRecord> result,
                string prefix,
                string statement
            )
        {
            int[] found = Client.SequentialSearch
                (
                    prefix,
                    statement
                );
            foreach (int mfn in found)
            {
                _AddRecord
                    (
                        result,
                        mfn
                    );
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Поиск записей согласно спецификации.
        /// </summary>
        public List<IrbisRecord> SearchRecords
            (
                SelectionQuery query
            )
        {
            if (ReferenceEquals(query, null))
            {
                throw new ArgumentNullException("query");
            }
            if (!query.IsValid())
            {
                throw new ArgumentOutOfRangeException("query");
            }
            
            NumberRangeCollection ranges = null;

            if ((query.SelectionType == SelectionType.Mfn)
                || (query.SelectionType == SelectionType.Sequential))
            {
                ranges = NumberRangeCollection.Parse(query.Statement);
            }

            Dictionary<int,IrbisRecord> result 
                = new Dictionary<int, IrbisRecord>();

            bool databaseChanged = false;
            if (!string.IsNullOrEmpty(query.Database))
            {
                Client.PushDatabase(query.Database);
                databaseChanged = true;
            }

            _maxMfn = Client.GetMaxMfn();

            switch (query.SelectionType)
            {
                case SelectionType.Mfn:
                    _SearchByMfn
                        (
                            result,
                            ranges
                        );
                    break;
                case SelectionType.Sequential:
                    _SearchBySequential
                        (
                            result,
                            query.Prefix,
                            ranges
                        );
                    break;
                case SelectionType.Search:
                    _SearchByQuery
                        (
                            result,
                            query.Statement
                        );
                    break;
                case SelectionType.Deep:
                    _SearchByDeep
                        (
                            result,
                            query.Prefix,
                            query.Statement
                        );
                    break;
            }

            if (databaseChanged)
            {
                Client.PopDatabase();
            }

            Output.WriteLine
                (
                    "Total found={0}",
                    result.Count
                );

            return result.Values.ToList();
        }

        #endregion
    }
}
