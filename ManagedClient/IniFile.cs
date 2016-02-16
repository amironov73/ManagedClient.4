/* IniFile.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#endregion

namespace ManagedClient
{
    [Serializable]
    public class IniFile
        : Dictionary<string, IniFile.Section>
    {
        public sealed class Section
            : Dictionary<string, string>
        {
            public string Name { get; set; }

            public Section
                (
                    string name
                )
                : base(StringComparer.CurrentCultureIgnoreCase)
            {
                Name = name;
            }

            public string Get
                (
                    string key,
                    string defaultValue
                )
            {
                string result;
                if (!TryGetValue(key, out result))
                {
                    result = defaultValue;
                }
                return result;
            }

            public string Get
                (
                    string key
                )
            {
                return Get(key, null);
            }

            public T Get<T>
                (
                    string key,
                    T defaultValue
                )
            {
                string text;
                if (!TryGetValue(key, out text))
                {
                    return defaultValue;
                }
                T result = (T)Convert.ChangeType(text, typeof(T));
                return result;
            }

            public T Get<T>
                (
                    string key
                )
            {
                return Get(key, default(T));
            }
        }

        public string FileName { get; set; }

        public IniFile()
            : base(StringComparer.CurrentCultureIgnoreCase)
        {
        }

        public string this[string sectionName, string parameterName]
        {
            get { return GetString(sectionName, parameterName, null); }
        }

        public Section GetSection(string name)
        {
            Section result;
            TryGetValue(name, out result);
            return result;
        }

        public string GetString
            (
                string sectionName,
                string key,
                string defaultValue
            )
        {
            Section section = GetSection(sectionName);
            return section == null
                       ? defaultValue
                       : section.Get(key, defaultValue);
        }

        public T Get<T>
            (
                string sectionName,
                string key,
                T defaultValue
            )
        {
            Section section = GetSection(sectionName);
            return section == null
                       ? defaultValue
                       : section.Get(key, defaultValue);
        }

        public static T ParseLines<T>
            (
                string[] lines
            )
            where T: IniFile, new()
        {
            T result = new T();

            Section section = new Section(string.Empty);
            result.Add(string.Empty, section);

            foreach (string line in lines)
            {
                if (line.StartsWith("["))
                {
                    string name = line.Trim(' ', '\t', '[', ']');
                    if (!string.IsNullOrEmpty(name))
                    {
                        if (name.StartsWith("@"))
                        {
                            name = name.Substring(1);
                            IniFile outer = ParseFile<T>
                                (
                                    Path.ChangeExtension(name, ".ini"),
                                    Encoding.Default
                                );
                            result.Merge(outer);
                        }
                        else
                        {
                            Section newSection = result.GetSection(name);
                            if (newSection == null)
                            {
                                newSection = new Section(name);
                                result.Add(name, newSection);
                                section = newSection;
                            }
                        }
                    }
                }
                else
                {
                    string line2 = line.TrimStart();
                    int position = line2.IndexOf('=');
                    if (position > 0)
                    {
                        string key = line2.Substring(0, position).TrimEnd();
                        string value = line2.Substring(position + 1).Trim();
                        section[key] = value;
                    }
                }
            }

            return result;
        }

        public static T ParseFile<T>
            (
                string fileName,
                Encoding encoding
            )
            where T: IniFile, new()
        {
            string[] lines = File.ReadAllLines
                (
                    fileName,
                    encoding
                );

            T result = ParseLines<T>(lines);
            result.FileName = fileName;

            return result;
        }

        public static T ParseText<T>
            (
                string text
            )
            where T: IniFile, new()
        {
            string[] lines = text.SplitLines();
            return ParseLines<T>(lines);
        }

        public IniFile Merge
            (
                IniFile other
            )
        {
            foreach (KeyValuePair<string, Section> pair in other)
            {
                string name = pair.Key;
                Section otherSection = pair.Value;
                Section thisSection;
                if (!TryGetValue(name, out thisSection))
                {
                    thisSection = new Section(name);
                }
                foreach (KeyValuePair<string, string> pair2 in otherSection)
                {
                    string key = pair2.Key;
                    string value = pair2.Value;
                    thisSection[key] = value;
                }

            }
            return this;
        }
    }
}
