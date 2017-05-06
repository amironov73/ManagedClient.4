// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisUpperCaseTable.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient
{
    //
    // Системная таблица ISISUCW.TAB для перевода
    // текста в верхний регистр.
    // Таблица реализована в виде текстового файла.
    // Местонахождение и имя файла по умолчанию:
    // <IRBIS_SERVER_ROOT>\ISISUCW.TAB.
    // Местонахождение и имя файла определяется значением
    // параметра UCTABPATH в конфигурационном файле
    // ИРБИС и может быть изменено.
    //
    // Стандартное содержимое
    //
    // 000 001 002 003 004 005 006 007 008 009 010 011 012 013 014 015 016 017 018 019 020 021 022 023 024 025 026 027 028 028 030 031
    // 032 033 034 035 036 037 038 039 040 041 042 043 044 045 046 047 048 049 050 051 052 053 054 055 056 057 058 059 060 061 062 063
    // 064 065 066 067 068 069 070 071 072 073 074 075 076 077 078 079 080 081 082 083 084 085 086 087 088 089 090 091 092 093 094 095
    // 096 065 066 067 068 069 070 071 072 073 074 075 076 077 078 079 080 081 082 083 084 085 086 087 088 089 090 123 124 125 126 127
    // 128 129 130 131 132 133 134 135 136 137 138 139 140 141 142 143 144 145 146 147 148 149 150 151 152 153 154 155 156 157 158 159
    // 160 161 161 163 164 165 166 167 168 169 170 171 172 173 174 175 176 177 178 178 165 181 182 183 168 185 170 187 163 189 189 175
    // 192 193 194 195 196 197 198 199 200 201 202 203 204 205 206 207 208 209 210 211 212 213 214 215 216 217 218 219 220 221 222 223
    // 192 193 194 195 196 197 198 199 200 201 202 203 204 205 206 207 208 209 210 211 212 213 214 215 216 217 218 219 220 221 222 223
    //

    /// <summary>
    /// Table for uppercase character conversion.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    public sealed class IrbisUpperCaseTable
    {
        #region Constants

        /// <summary>
        /// Имя файла таблицы по умолчанию.
        /// </summary>
        public const string FileName = "ISISUCW.TAB";

        #endregion

        #region Properties

        /// <summary>
        /// Собственно таблица.
        /// </summary>
        public Dictionary<char, char> Mapping
        {
            get { return _mapping; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisUpperCaseTable()
        {
            _encoding = IrbisEncoding.Ansi;
            _table = new byte[]
            {
                000, 001, 002, 003, 004, 005, 006, 007,
                008, 009, 010, 011, 012, 013, 014, 015,
                016, 017, 018, 019, 020, 021, 022, 023,
                024, 025, 026, 027, 028, 028, 030, 031,
                032, 033, 034, 035, 036, 037, 038, 039,
                040, 041, 042, 043, 044, 045, 046, 047,
                048, 049, 050, 051, 052, 053, 054, 055,
                056, 057, 058, 059, 060, 061, 062, 063,
                064, 065, 066, 067, 068, 069, 070, 071,
                072, 073, 074, 075, 076, 077, 078, 079,
                080, 081, 082, 083, 084, 085, 086, 087,
                088, 089, 090, 091, 092, 093, 094, 095,
                096, 065, 066, 067, 068, 069, 070, 071,
                072, 073, 074, 075, 076, 077, 078, 079,
                080, 081, 082, 083, 084, 085, 086, 087,
                088, 089, 090, 123, 124, 125, 126, 127,
                128, 129, 130, 131, 132, 133, 134, 135,
                136, 137, 138, 139, 140, 141, 142, 143,
                144, 145, 146, 147, 148, 149, 150, 151,
                152, 153, 154, 155, 156, 157, 158, 159,
                160, 161, 161, 163, 164, 165, 166, 167,
                168, 169, 170, 171, 172, 173, 174, 175,
                176, 177, 178, 178, 165, 181, 182, 183,
                168, 185, 170, 187, 163, 189, 189, 175,
                192, 193, 194, 195, 196, 197, 198, 199,
                200, 201, 202, 203, 204, 205, 206, 207,
                208, 209, 210, 211, 212, 213, 214, 215,
                216, 217, 218, 219, 220, 221, 222, 223,
                192, 193, 194, 195, 196, 197, 198, 199,
                200, 201, 202, 203, 204, 205, 206, 207,
                208, 209, 210, 211, 212, 213, 214, 215,
                216, 217, 218, 219, 220, 221, 222, 223
            };
            _BuildMapping();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisUpperCaseTable
            (
                [NotNull] Encoding encoding,
                [NotNull] byte[] table
            )
        {

            if (!encoding.IsSingleByte)
            {
                throw new IrbisException
                    (
                        "Must be single-byte encoding"
                    );
            }

            if (table.Length != 256)
            {
                throw new IrbisException
                    (
                        "Must be 256 bytes in table"
                    );
            }

            _encoding = encoding;
            _table = table;
            _BuildMapping();
        }

        #endregion

        #region Private members

        private static object _lock = new object();

        private static IrbisUpperCaseTable _instance;

        private Encoding _encoding;

        private byte[] _table;

        private Dictionary<char, char> _mapping;

        private void _BuildMapping()
        {
            byte[] bytes = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                bytes[i] = (byte)i;
            }
            char[] fromMapping = _encoding.GetChars(bytes);
            char[] toMapping = _encoding.GetChars(_table);
            _mapping = new Dictionary<char, char>(256);
            for (int i = 0; i < 256; i++)
            {
                _mapping[fromMapping[i]] = toMapping[i];
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get global instance of the
        /// <see cref="IrbisUpperCaseTable"/>.
        /// </summary>
        [NotNull]
        public static IrbisUpperCaseTable GetInstance
            (
                [NotNull] ManagedClient64 connection
            )
        {
            if (ReferenceEquals(_instance, null))
            {
                lock (_lock)
                {
                    if (ReferenceEquals(_instance, null))
                    {
                        _instance = FromServer
                            (
                                connection,
                                FileName
                            );
                    }
                }
            }

            return _instance;
        }

        /// <summary>
        /// Load the table from specified server file.
        /// </summary>
        [NotNull]
        public static IrbisUpperCaseTable FromServer
            (
                [NotNull] ManagedClient64 connection,
                [NotNull] string fileName
            )
        {
            string text = connection.ReadTextFile
                (
                    IrbisPath.System,
                    fileName
                );

            if (string.IsNullOrEmpty(text))
            {
                throw new IrbisException
                    (
                        "No file " + fileName
                    );
            }

            IrbisUpperCaseTable result = ParseText
                (
                    IrbisEncoding.Ansi,
                    text
                );

            return result;
        }

        /// <summary>
        /// Парсим локальный файл.
        /// </summary>
        [NotNull]
        public static IrbisUpperCaseTable ParseLocalFile
            (
                [NotNull] string fileName
            )
        {
            using (StreamReader reader
                = new StreamReader
                    (
                        File.OpenRead(fileName),
                        IrbisEncoding.Ansi
                    ))
            {
                string text = reader.ReadToEnd();

                return ParseText
                    (
                        IrbisEncoding.Ansi,
                        text
                    );
            }
        }

        /// <summary>
        /// Parse text and build upper-case table.
        /// </summary>
        [NotNull]
        public static IrbisUpperCaseTable ParseText
            (
                [NotNull] Encoding encoding,
                [NotNull] string text
            )
        {
            List<byte> table = new List<byte>(256);

            MatchCollection matches = Regex.Matches(text, @"\d+");
            foreach (Match match in matches)
            {
                byte b = byte.Parse(match.Value);
                table.Add(b);
            }

            IrbisUpperCaseTable result = new IrbisUpperCaseTable
                (
                    encoding,
                    table.ToArray()
                );

            return result;
        }

        /// <summary>
        /// Free global instance of
        /// <see cref="IrbisUpperCaseTable"/>.
        /// </summary>
        public static void ResetInstance()
        {
            lock (_lock)
            {
                _instance = null;
            }
        }

        /// <summary>
        /// Формируем исходный код с определением таблицы.
        /// </summary>
        public void ToSourceCode
            (
                [NotNull] TextWriter writer
            )
        {
            int count = 0;

            writer.WriteLine("new byte [] {");
            foreach (byte c in _table)
            {
                if (count == 0)
                {
                    writer.Write("   ");
                }

                writer.Write(" ");
                writer.Write(c.ToString("000"));
                writer.Write(",");

                count++;
                if (count > 10)
                {
                    count = 0;
                    writer.WriteLine();
                }
            }
            writer.WriteLine();
            writer.WriteLine("};");
        }

        /// <summary>
        /// Converts specified character to uppercase.
        /// </summary>
        public char ToUpper
            (
                char c
            )
        {
            char result;
            if (!Mapping.TryGetValue(c, out result))
            {
                result = c;
            }

            return result;
        }

        /// <summary>
        /// Converts specified string to uppercase.
        /// </summary>
        [NotNull]
        public string ToUpper
            (
                [NotNull] string text
            )
        {
            StringBuilder result = new StringBuilder(text.Length);
            foreach (char c1 in text)
            {
                char c2;
                if (!Mapping.TryGetValue(c1, out c2))
                {
                    c2 = c1;
                }
                result.Append(c2);
            }

            return result.ToString();
        }

        /// <summary>
        /// Записываемся в файл.
        /// </summary>
        public void WriteLocalFile
            (
                [NotNull] string fileName
            )
        {
            using (StreamWriter writer = new StreamWriter
                (
                    File.Create(fileName),
                    IrbisEncoding.Ansi
                ))
            {
                WriteTable(writer);
            }
        }

        /// <summary>
        /// Записываемся в поток.
        /// </summary>
        public void WriteTable
            (
                [NotNull] TextWriter writer
            )
        {
            int count = 0;

            foreach (byte b in _table)
            {
                if (count != 0)
                {
                    writer.Write(" ");
                }
                writer.Write
                (
                    "{0:000}",
                    b
                );
                count++;
                if (count == 32)
                {
                    writer.WriteLine();
                    count = 0;
                }
            }
        }

        #endregion
    }
}
