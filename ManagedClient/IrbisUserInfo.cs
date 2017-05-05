// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisUserInfo.cs -- информация о зарегистрированном пользователе
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Информация о зарегистрированном пользователе системы
    /// (по данным client_m.mnu).
    /// </summary>
    [Serializable]
    [XmlRoot("user")]
    [MoonSharpUserData]
    public sealed class IrbisUserInfo
    {
        #region Properties

        /// <summary>
        /// Номер по порядку.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public string Number { get; set; }

        /// <summary>
        /// Логин.
        /// </summary>
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        [XmlAttribute("password")]
        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        /// Доступность АРМ Каталогизатор.
        /// </summary>
        [XmlAttribute("cataloguer")]
        [JsonProperty("cataloguer")]
        public string Cataloger { get; set; }

        /// <summary>
        /// АРМ Читатель.
        /// </summary>
        [XmlAttribute("reader")]
        [JsonProperty("reader")]
        public string Reader { get; set; }

        /// <summary>
        /// АРМ Книговыдача.
        /// </summary>
        [XmlAttribute("circulation")]
        [JsonProperty("circulation")]
        public string Circulation { get; set; }

        /// <summary>
        /// АРМ Комплектатор.
        /// </summary>
        [XmlAttribute("acquisitions")]
        [JsonProperty("acquisitions")]
        public string Acquisitions { get; set; }

        /// <summary>
        /// АРМ Книгообеспеченность.
        /// </summary>
        public string Provision { get; set; }

        /// <summary>
        /// АРМ Администратор.
        /// </summary>
        [XmlAttribute("administrator")]
        [JsonProperty("administrator")]
        public string Administrator { get; set; }

        #endregion

        #region Private members

        private string FormatPair
            (
                string prefix,
                string value,
                string defaultValue
            )
        {
            if (value.SameString(defaultValue))
            {
                return string.Empty;
            }
            return string.Format
                (
                    "{0}={1};",
                    prefix,
                    value
                );

        }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static IrbisUserInfo[] Parse
            (
                string[] text
            )
        {
            List<IrbisUserInfo> result = new List<IrbisUserInfo>();

            for (int index = 3; index < (text.Length - 9); index += 9)
            {
                IrbisUserInfo user = new IrbisUserInfo
                                         {
                                             Number = text[index],
                                             Name = text[index + 1],
                                             Password = text[index + 2],
                                             Cataloger = text[index + 3],
                                             Reader = text[index + 4],
                                             Circulation = text[index + 5],
                                             Acquisitions = text[index + 6],
                                             Provision = text[index + 7],
                                             Administrator = text[index + 8]
                                         };
                result.Add(user);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Encode the <see cref="IrbisUserInfo"/>.
        /// </summary>
        [NotNull]
        public string Encode()
        {
            return string.Format
                (
                    "{0}\r\n{1}\r\n{2}{3}{4}{5}{6}{7}",
                    Name,
                    Password,
                    FormatPair("C", Cataloger, "irbisc.ini"),
                    FormatPair("R", Reader, "irbisr.ini"),
                    FormatPair("B", Circulation, "irbisb.ini"),
                    FormatPair("M", Acquisitions, "irbisp.ini"),
                    FormatPair("K", Provision, "irbisk.ini"),
                    FormatPair("A", Administrator, "irbisa.ini")
                );
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return string.Format
                (
                    "Number: {0}, Name: {1}, Password: {2}, "
                  + "Cataloguer: {3}, Reader: {4}, Circulation: {5}, "
                  + "Acquisitions: {6}, Provision: {7}, Administrator: {8}",
                    Number,
                    Name,
                    Password,
                    Cataloger,
                    Reader,
                    Circulation,
                    Acquisitions,
                    Provision,
                    Administrator
                );
        }

        #endregion
    }
}
