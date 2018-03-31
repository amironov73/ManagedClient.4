// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AfterQueryEventArgs.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

#endregion

namespace ManagedClient
{
    // Примеры ссылок на поля
    // v200
    // v200^a
    // ". - "v200
    // v300+| - |
    // v701[1-2]
    // v701^a*2.2
    // "Отсутствует"n700

    public sealed class FieldReference
    {
        #region Properties

        public string OriginalText
        {
            get
            {
                return _originalText;
            }
        }

        public string PreConditional { get; set; }

        public string PreRepeatable { get; set; }

        public bool PrePlus { get; set; }

        public char Dvn
        {
            get
            {
                return _dvn;
            }
            set
            {
#if PocketPC
                _dvn = char.ToUpper ( value );
#else
                _dvn = char.ToUpperInvariant(value);
#endif
            }
        }

        public string Field
        {
            get
            {
                return _field;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException();
                }
                _field = value;
            }
        }

        public string Embedded { get; set; }

        public char SubField { get; set; }

        public bool NumberPresent { get; set; }

        public int NumberFrom { get; set; }

        public int NumberTo { get; set; }

        public int? Offset { get; set; }

        public int? Length { get; set; }

        public string PostReapeatable { get; set; }

        public bool PostPlus { get; set; }

        public string PostConditional { get; set; }

        #endregion

        #region Construction

        public FieldReference()
        {
            Dvn = 'V';
        }

        public FieldReference(string originalText)
            : this()
        {
            _originalText = originalText;

            if (string.IsNullOrEmpty(originalText))
            {
                return;
            }

            Regex regex = _GetRegex();
            Match match = regex.Match(originalText);
            if (!match.Success)
            {
                throw new ArgumentException();
            }

            string text = match.Groups["precond"].Value;
            if (!string.IsNullOrEmpty(text))
            {
                PreConditional = _StripEnds(text);
            }
            text = match.Groups["prerep"].Value;
            if (!string.IsNullOrEmpty(text))
            {
                if (text.EndsWith("+"))
                {
                    PrePlus = true;
                    text = text.Substring
                        (
                            0,
                            text.Length - 1
                        );
                }
                PreRepeatable = _StripEnds(text);
            }
            text = match.Groups["field"].Value;
#if PocketPC
            Dvn = char.ToUpper(text[0]);
#else
            Dvn = char.ToUpperInvariant(text[0]);
#endif
            Field = text.Substring(1);
            text = match.Groups["embedded"].Value;
            if (!string.IsNullOrEmpty(text))
            {
                Embedded = text.Substring(1);
            }
            text = match.Groups["subfield"].Value;
            if (!string.IsNullOrEmpty(text))
            {
                SubField = text[1];
            }
            text = match.Groups["number"].Value;
            if (!string.IsNullOrEmpty(text))
            {
                NumberPresent = true;
                text = _StripEnds(text);
                Regex numRegex = new Regex(@"(?<from>\d+)(?:-(?<to>\d+))?");
                Match numMatch = numRegex.Match(text);
                if (!numMatch.Success)
                {
                    throw new ArgumentException();
                }
                NumberFrom = FastNumber.ParseInt32(numMatch.Groups["from"].Value);
                text = numMatch.Groups["to"].Value;
                NumberTo = string.IsNullOrEmpty(text)
                               ? NumberFrom
                               : FastNumber.ParseInt32(text);
                if (NumberFrom > NumberTo)
                {
                    throw new ArgumentException();
                }
            }
            text = match.Groups["offset"].Value;
            if (!string.IsNullOrEmpty(text))
            {
                Offset = FastNumber.ParseInt32(text.Substring(1));
            }
            text = match.Groups["length"].Value;
            if (!string.IsNullOrEmpty(text))
            {
                Length = FastNumber.ParseInt32(text.Substring(1));
            }
            text = match.Groups["occur"].Value;
            if (!string.IsNullOrEmpty(text))
            {
                NumberFrom = NumberTo = FastNumber.ParseInt32(text.Substring(1));
                NumberPresent = true;
            }
            text = match.Groups["postrep"].Value;
            if (!string.IsNullOrEmpty(text))
            {
                if (text.StartsWith("+"))
                {
                    PostPlus = true;
                    text = text.Substring(1);
                }
                PostReapeatable = _StripEnds(text);
            }
            text = match.Groups["postcond"].Value;
            if (!string.IsNullOrEmpty(text))
            {
                PostConditional = _StripEnds(text);
            }
        }

        #endregion

        #region Private members

        private readonly string _originalText;

        private char _dvn;

        private string _field;

        private static string _SafeSubString
            (
                string text,
                int offset,
                int length
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            if ((offset + length) > text.Length)
            {
                length = text.Length - offset;
                if (length < -0)
                {
                    return string.Empty;
                }
            }
            return text.Substring
                (
                    offset,
                    length
                );
        }

        private static string _StripEnds(string text)
        {
            return text.Substring
                (
                    1,
                    text.Length - 2
                );
        }

        private static Regex _GetRegex()
        {
            const string text = @"^
(?<precond>""[^""]*?"")?
(?<prerep>\|[^|]*?\|[+]?)?
(?<field>[dvng]\d+)
(?<embedded>[@]\d+)?
(?<subfield>\^.)?
(?<number>\[(?:\d+|\d+-\d+)\])?
(?<offset>[*]\d+)?
(?<length>[.]\d+)?
(?<occur>[#]\d+)?
(?<postrep>[+]?\|[^|]*?\|)?
(?<postcond>""[^""]*?"")?
$";
            Regex result = new Regex
                (
                    text,
                    RegexOptions.IgnoreCase
                    | RegexOptions.IgnorePatternWhitespace
                    | RegexOptions.Singleline
                );

            return result;
        }

        #endregion

        #region Public methods

        public string[] GetAll
            (
            IEnumerable<RecordField> fields
            )
        {
            List<string> result = new List<string>();

            RecordField[] selectedFields = fields.GetField(Field);
            if (!string.IsNullOrEmpty(Embedded))
            {
                RecordField[] embeddedFields = selectedFields
                    .SelectMany(f => f.GetEmbeddedFields())
                    .GetField(Embedded);
                selectedFields = embeddedFields;
            }

            foreach (RecordField field in selectedFields)
            {
                if (SubField != '\0')
                {
                    string[] texts = (SubField == '*')
                                         ? new[] { field.Text }
                                         : field.GetSubField(SubField).GetSubFieldText();
                    result.AddRange(texts);
                }
                else
                {
                    result.Add(field.ToText());
                }
            }

            return result
                .Where(s => !string.IsNullOrEmpty(s))
                .ToArray();
        }

        public string[] LimitNumber
            (
            string[] source
            )
        {
            List<string> result = new List<string>();

            if (!NumberPresent)
            {
                result.AddRange(source);
            }
            else
            {
                int low = NumberFrom - 1;
                int high = NumberTo - 1;

                for (int i = 0; i < source.Length; i++)
                {
                    if ((i >= low)
                         && (i <= high))
                    {
                        result.Add(source[i]);
                    }
                }
            }

            return result
                .Where(s => !string.IsNullOrEmpty(s))
                .ToArray();
        }

        public string[] LimitLength
            (
            string[] source
            )
        {
            List<string> result = new List<string>();

            if (!Offset.HasValue
                 && !Length.HasValue)
            {
                result.AddRange(source);
            }
            else
            {
                int offset = 0;
                if (Offset.HasValue)
                {
                    offset = Offset.Value;
                }
                int length = int.MaxValue;
                if (Length.HasValue)
                {
                    length = Length.Value;
                }
                result.AddRange(source.Select(s =>
                                              _SafeSubString(s, offset, length)));
            }

            return result
                .Where(s => !string.IsNullOrEmpty(s))
                .ToArray();
        }

        public string[] Decorate
            (
            string[] source
            )
        {
            List<string> result = new List<string>();

            bool nonEmpty = source.Any(s => !string.IsNullOrEmpty(s));

            if (Dvn == 'V')
            {
                for (int i = 0; i < source.Length; i++)
                {
                    string text = source[i];

                    if ((i == 0)
                         && !string.IsNullOrEmpty(PreConditional))
                    {
                        text = PreConditional + text;
                    }

                    if (!string.IsNullOrEmpty(PreRepeatable))
                    {
                        if (!PrePlus
                             || (i != 0))
                        {
                            text = PreRepeatable + text;
                        }
                    }

                    if (!string.IsNullOrEmpty(PostReapeatable))
                    {
                        if (!PostPlus
                             || (i != (source.Length - 1)))
                        {
                            text = text + PostReapeatable;
                        }
                    }

                    if ((i == (source.Length - 1))
                         && !string.IsNullOrEmpty(PostConditional))
                    {
                        text = text + PostConditional;
                    }

                    result.Add(text);
                }
            }
            else if (Dvn == 'D')
            {
                string text = string.Empty;

                if (nonEmpty)
                {
                    if (!string.IsNullOrEmpty(PreConditional))
                    {
                        text = PreConditional + text;
                    }
                    if (!string.IsNullOrEmpty(PostConditional))
                    {
                        text = text + PostConditional;
                    }
                    result.Add(text);
                }
            }
            else if (Dvn == 'N')
            {
                string text = string.Empty;

                if (!nonEmpty)
                {
                    if (!string.IsNullOrEmpty(PreConditional))
                    {
                        text = PreConditional + text;
                    }
                    if (!string.IsNullOrEmpty(PostConditional))
                    {
                        text = text + PostConditional;
                    }
                    result.Add(text);
                }
            }
            else
            {
                throw new ApplicationException();
            }

            return result
                .Where(s => !string.IsNullOrEmpty(s))
                .ToArray();
        }

        public string[] Format
            (
            string[] source
            )
        {
            string[] result = LimitNumber(source);
            result = LimitLength(result);
            result = Decorate(result);
            return result;
        }

        public string FormatSingle
            (
            string[] source
            )
        {
            string[] result = Format(source);
            return string.Join
                (
                 string.Empty,
                 result
                );
        }

        public string[] Format
            (
            IEnumerable<RecordField> fields
            )
        {
            string[] source = GetAll(fields);
            string[] result = Format(source);
            return result;
        }

        public string FormatSingle
            (
            IEnumerable<RecordField> fields
            )
        {
            string[] source = GetAll(fields);
            string result = FormatSingle(source);
            return result;
        }

        public string[] Format
            (
            IrbisRecord record
            )
        {
            return Format(record.Fields);
        }

        public string FormatSingle
            (
            IrbisRecord record
            )
        {
            return string.Join
                (
                 string.Empty,
                 Format(record)
                );
        }

        public string ToText()
        {
            StringBuilder result = new StringBuilder();

            if (!string.IsNullOrEmpty(PreConditional))
            {
                result.AppendFormat
                    (
                        "\"{0}\"",
                        PreConditional
                    );
            }

            if (!string.IsNullOrEmpty(PreRepeatable))
            {
                result.AppendFormat
                    (
                        "|{0}|",
                        PreRepeatable
                    );
                if (PrePlus)
                {
                    result.Append("+");
                }
            }

            result.AppendFormat
                (
                    "{0}{1}",
                    char.ToLowerInvariant(Dvn),
                    Field
                );

            if (!string.IsNullOrEmpty(Embedded))
            {
                result.AppendFormat
                    (
                        "@{0}",
                        Embedded
                    );
            }

            if (SubField != '\0')
            {
                result.AppendFormat
                    (
                        "^{0}",
                        char.ToLowerInvariant(SubField)
                    );
            }

            if (NumberPresent)
            {
                if (NumberTo == NumberFrom)
                {
                    result.AppendFormat
                        (
                            "[{0}]",
                            NumberFrom
                        );
                }
                else
                {
                    result.AppendFormat
                        (
                            "[{0}-{1}]",
                            NumberFrom,
                            NumberTo
                        );
                }
            }

            if (Offset.HasValue)
            {
                result.AppendFormat
                    (
                        "*{0}",
                        Offset.Value
                    );
            }

            if (Length.HasValue)
            {
                result.AppendFormat
                    (
                        ".{0}",
                        Length.Value
                    );
            }

            if (!string.IsNullOrEmpty(PostReapeatable))
            {
                if (PostPlus)
                {
                    result.Append("+");
                }
                result.AppendFormat
                    (
                        "|{0}|",
                        PostReapeatable
                    );
            }

            if (!string.IsNullOrEmpty(PostConditional))
            {
                result.AppendFormat
                    (
                         "\"{0}\"",
                        PostConditional
                    );
            }

            return result.ToString();
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.IsNullOrEmpty(OriginalText)
                       ? ToText()
                       : OriginalText;
        }

        #endregion
    }
}
