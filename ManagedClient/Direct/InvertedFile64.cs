﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* InvertedFile64.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Direct
{
    /// <summary>
    /// Inverted file for IRBIS64.
    /// </summary>
    public class InvertedFile64
        : IDisposable
    {
        #region Constants

        /// <summary>
        /// Длина записи N01/L01.
        /// </summary>
        public const int NodeLength = 2048;

        #endregion

        #region Properties

        /// <summary>
        /// File name.
        /// </summary>
        public string FileName { get { return _fileName; } }

        /// <summary>
        /// IFP file.
        /// </summary>
        public Stream Ifp { get { return _ifp; } }

        /// <summary>
        /// L01 file.
        /// </summary>
        public Stream L01 { get { return _l01; } }

        /// <summary>
        /// N01 file.
        /// </summary>
        public Stream N01 { get { return _n01; } }

        #endregion

        #region Construction

        public InvertedFile64(string fileName)
        {
            _encoding = new UTF8Encoding(false, true);

            _fileName = fileName;

            _ifp = _OpenStream(fileName);
            _l01 = _OpenStream(Path.ChangeExtension(fileName, ".l01"));
            _n01 = _OpenStream(Path.ChangeExtension(fileName, ".n01"));
        }

        #endregion

        #region Private members

        private readonly string _fileName;

        private Stream _ifp;

        private Stream _l01;

        private Stream _n01;

        private readonly Encoding _encoding;

        private static Stream _OpenStream(string fileName)
        {
            return new FileStream
                (
                    fileName,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite
                );
        }

        private long _NodeOffset(int nodeNumber)
        {
            long result = unchecked((((long)nodeNumber) - 1) * NodeLength);
            return result;
        }

        private NodeRecord _ReadNode
            (
                bool isLeaf,
                Stream stream,
                long offset
            )
        {
            stream.Position = offset;

            NodeRecord result = new NodeRecord(isLeaf)
                {
                    _stream = stream,
                    Leader =
                        {
                            Number = stream.ReadInt32Network(),
                            Previous = stream.ReadInt32Network(),
                            Next = stream.ReadInt32Network(),
                            TermCount = stream.ReadInt16Network(),
                            FreeOffset = stream.ReadInt16Network()
                        }
                };

            for (int i = 0; i < result.Leader.TermCount; i++)
            {
                NodeItem item = new NodeItem
                    {
                        Length = stream.ReadInt16Network(),
                        KeyOffset = stream.ReadInt16Network(),
                        LowOffset = stream.ReadInt32Network(),
                        HighOffset = stream.ReadInt32Network()
                    };
                result.Items.Add(item);
            }

            foreach (NodeItem item in result.Items)
            {
                stream.Position = offset + item.KeyOffset;
                byte[] buffer = stream.ReadBytes(item.Length);
                string text = _encoding.GetString(buffer);
                item.Text = text;
            }

            return result;
        }

        #endregion

        #region Public methods

        public NodeRecord ReadNode(int number)
        {
            return _ReadNode(false, _n01, _NodeOffset(number));
        }

        public NodeRecord ReadLeaf(int number)
        {
            number = Math.Abs(number);
            return _ReadNode(true, _l01, _NodeOffset(number));
        }

        public NodeRecord ReadNext(NodeRecord record)
        {
            int number = record.Leader.Next;

            if (number < 0)
            {
                return null;
            }

            return _ReadNode(record.IsLeaf, record._stream, _NodeOffset(number));
        }

        public NodeRecord ReadPrevious(NodeRecord record)
        {
            int number = record.Leader.Previous;
            if (number < 0)
            {
                return null;
            }

            return _ReadNode(record.IsLeaf, record._stream, _NodeOffset(number));
        }

        public IfpRecord ReadIfpRecord(long offset)
        {
            IfpRecord result = IfpRecord.Read(Ifp, offset);
            return result;
        }

        public TermLink[] SearchExact(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return new TermLink[0];
            }

            try
            {

                key = key.ToUpperInvariant();

                NodeRecord firstNode = ReadNode(1);
                NodeRecord rootNode = ReadNode(firstNode.Leader.Number);
                NodeRecord currentNode = rootNode;

                NodeItem goodItem = null;
                while (true)
                {
                    bool found = false;
                    bool beyond = false;

                    foreach (NodeItem item in currentNode.Items)
                    {
                        int compareResult = string.CompareOrdinal(item.Text, key);
                        if (compareResult > 0)
                        {
                            beyond = true;
                            break;
                        }

                        goodItem = item;
                        found = true;

                        if ((compareResult == 0)
                            && currentNode.IsLeaf)
                        {
                            goto FOUND;
                        }

                    }
                    if (goodItem == null)
                    {
                        break;
                    }
                    if (found)
                    {
                        if (beyond || (currentNode.Leader.Next == -1))
                        {
                            currentNode = goodItem.RefersToLeaf
                                ? ReadLeaf(goodItem.LowOffset)
                                : ReadNode(goodItem.LowOffset);
                        }
                        else
                        {
                            currentNode = ReadNext(currentNode);
                        }
                    }
                    else
                    {
                        currentNode = goodItem.RefersToLeaf
                            ? ReadLeaf(goodItem.LowOffset)
                            : ReadNode(goodItem.LowOffset);
                    }
                    //Console.WriteLine(currentNode);
                }

                FOUND:
                if (goodItem != null)
                {
                    IfpRecord ifp = ReadIfpRecord(goodItem.FullOffset);
                    return ifp.Links.ToArray();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return new TermLink[0];
        }

        public TermLink[] SearchStart(string key)
        {
            List<TermLink> result = new List<TermLink>();

            if (string.IsNullOrEmpty(key))
            {
                return new TermLink[0];
            }

            key = key.ToUpperInvariant();

            NodeRecord firstNode = ReadNode(1);
            NodeRecord rootNode = ReadNode(firstNode.Leader.Number);
            NodeRecord currentNode = rootNode;

            NodeItem goodItem = null;
            while (true)
            {
                bool found = false;
                bool beyond = false;

                foreach (NodeItem item in currentNode.Items)
                {
                    int compareResult = string.CompareOrdinal(item.Text, key);
                    if (compareResult > 0)
                    {
                        beyond = true;
                        break;
                    }

                    goodItem = item;
                    found = true;
                }
                if (goodItem == null)
                {
                    break;
                }
                if (found)
                {
                    if (beyond || (currentNode.Leader.Next == -1))
                    {
                        if (goodItem.RefersToLeaf)
                        {
                            goto FOUND;
                        }
                        currentNode = ReadNode(goodItem.LowOffset);
                    }
                    else
                    {
                        currentNode = ReadNext(currentNode);
                    }
                }
                else
                {
                    if (goodItem.RefersToLeaf)
                    {
                        goto FOUND;
                    }
                    currentNode = ReadNode(goodItem.LowOffset);
                }
                //Console.WriteLine(currentNode);
            }

        FOUND:
            if (goodItem != null)
            {
                currentNode = ReadLeaf(goodItem.LowOffset);
                while (true)
                {
                    foreach (NodeItem item in currentNode.Items)
                    {
                        int compareResult = string.CompareOrdinal(item.Text, key);
                        if (compareResult >= 0)
                        {
                            bool starts = item.Text.StartsWith(key);
                            if ((compareResult > 0) && !starts)
                            {
                                goto DONE;
                            }
                            if (starts)
                            {
                                IfpRecord ifp = ReadIfpRecord(item.FullOffset);
                                result.AddRange(ifp.Links);
                            }
                        }
                    }
                    if (currentNode.Leader.Next > 0)
                    {
                        currentNode = ReadNext(currentNode);
                    }
                }

            }

        DONE:
            return result
                .Distinct()
                .ToArray();
        }

        public int[] SearchSimple(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return new int[0];
            }

            TermLink[] result = new TermLink[0];

            if (key.EndsWith("$"))
            {
                key = key.Substring(0, key.Length - 1);
                if (!string.IsNullOrEmpty(key))
                {
                    result = SearchStart(key);
                }
            }
            else
            {
                result = SearchExact(key);
            }

            return result
                .Select(link => link.Mfn)
                .Distinct()
                .ToArray();
        }

        #endregion

        #region IDisposable members

        public void Dispose()
        {
            if (_ifp != null)
            {
                _ifp.Dispose();
                _ifp = null;
            }
            if (_l01 != null)
            {
                _l01.Dispose();
                _l01 = null;
            }
            if (_n01 != null)
            {
                _n01.Dispose();
                _n01 = null;
            }
        }

        #endregion
    }
}
