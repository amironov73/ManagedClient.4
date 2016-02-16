/* ObjectDumper.cs
 */

#region Using directives

using System;
using System.IO;
using System.Text;

#endregion

namespace ManagedClient
{
    class ObjectDumper
    {
        #region Properties

        public TextWriter Writer { get { return _writer; } }

        #endregion

        #region Construction

        public ObjectDumper()
            : this (Console.Out)
        {
        }

        public ObjectDumper(TextWriter writer)
        {
            _writer = writer;
        }

        #endregion

        #region Private members

        private readonly TextWriter _writer;

        private void _DumpRow
            (
                byte[] array, 
                long position, 
                int offset, 
                int length
            )
        {
            StringBuilder visible = new StringBuilder();

            int curofs = offset - (offset%16);

            Writer.Write("{0:X8} ", position + curofs);

            while (curofs < offset)
            {
                Writer.Write( "   " );
                visible.Append(' ');
                curofs++;
            }

            for (int i = 0; i < length; i++, curofs++)
            {
                byte b = array[curofs];
                Writer.Write( "{0:X2} ", b );
                if ((b >= 32) && (b <= 127))
                {
                    char c = (char) b;
                    visible.Append(c);
                }
                else
                {
                    visible.Append('.');
                }
            }

            Writer.WriteLine(visible);
        }

        #endregion

        #region Public methods

        public ObjectDumper DumpBytes
            (
                byte[] array, 
                long position, 
                int offset, 
                int count
            )
        {
            if ((array == null) || (offset < 0) || (offset >= array.Length))
            {
                return this;
            }

            if ((offset + count) > array.Length)
            {
                count = array.Length - offset;
            }

            do
            {
                int rowLength = Math.Min(count, 16);
                _DumpRow(array,position,offset,rowLength);
                count -= rowLength;
                offset += rowLength;
            } while (count > 0);

            return this;
        }

        public ObjectDumper DumpStream
            (
                Stream stream, 
                long position, 
                int count
            )
        {
            if ((stream == null) || (count <= 0))
            {
                return this;
            }

            long savedPosition = stream.Position;

            try
            {
                byte[] buffer = new byte[count];
                int readed = stream.Read(buffer, 0, count);
                DumpBytes(buffer,position, 0,readed);
            }
            finally
            {
                stream.Position = savedPosition;
            }

            return this;
        }

        public ObjectDumper WriteLine()
        {
            Writer.WriteLine();
            return this;
        }

        public ObjectDumper WriteLine
            ( 
                string format, 
                params object[] args 
            )
        {
            Writer.WriteLine( format, args );
            return this;
        }

        #endregion
    }
}
