/* ReaderInfo.cs -- информация о читателе.
 */

#region Using directives

using System;
using System.Linq;
using System.Xml.Serialization;
using ManagedClient.Fields;
using Newtonsoft.Json;

#endregion

namespace ManagedClient.Readers
{
    /// <summary>
    /// Информация о читателе.
    /// </summary>
    [Serializable]
    [XmlRoot("reader")]
    public sealed class ReaderInfo
    {
        #region Properties

        /// <summary>
        /// ФИО. Комбинируется из полей 10, 11 и 12.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public string Fio { get; set; }

        /// <summary>
        /// Фамилия. Поле 10.
        /// </summary>
        [XmlAttribute("family-name")]
        [JsonProperty("family-name")]
        public string FamilyName { get; set; }

        /// <summary>
        /// Имя. Поле 11.
        /// </summary>
        [XmlAttribute("first-name")]
        [JsonProperty("first-name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество. Поле 12.
        /// </summary>
        [XmlAttribute("patronym")]
        [JsonProperty("patronym")]
        public string Patronym { get; set; }

        /// <summary>
        /// Дата рождения. Поле 21.
        /// </summary>
        [XmlAttribute("birthdate")]
        [JsonProperty("birthdate")]
        public string Birthdate { get; set; }

        /// <summary>
        /// Номер читательского. Поле 30.
        /// </summary>
        [XmlAttribute("ticket")]
        [JsonProperty("ticket")]
        public string Ticket { get; set; }

        /// <summary>
        /// Пол. Поле 23.
        /// </summary>
        [XmlAttribute("sex")]
        [JsonProperty("sex")]
        public string Sex { get; set; }

        /// <summary>
        /// Категория. Поле 50.
        /// </summary>
        [XmlAttribute("category")]
        [JsonProperty("category")]        
        public string Category { get; set; }

        /// <summary>
        /// Домашний адрес. Поле 13.
        /// </summary>
        [XmlAttribute("address")]
        [JsonProperty("address")]
        public ReaderAddress Address { get; set; }

        /// <summary>
        /// Место работы. Поле 15.
        /// </summary>
        [XmlAttribute("work")]
        [JsonProperty("work")]
        public string Work { get; set; }

        /// <summary>
        /// Образование. Поле 20.
        /// </summary>
        [XmlAttribute("education")]
        [JsonProperty("education")]
        public string Education { get; set; }

        /// <summary>
        /// Электронная почта. Поле 32.
        /// </summary>
        [XmlAttribute("email")]
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Домашний телефон. Поле 17.
        /// </summary>
        [XmlAttribute("home-phone")]
        [JsonProperty("home-phone")]
        public string HomePhone { get; set; }

        /// <summary>
        /// Дата записи. Поле 51.
        /// </summary>
        [XmlAttribute("registration-date")]
        [JsonProperty("registration-date")]
        public string RegistrationDateString { get; set; }

        /// <summary>
        /// Дата регистрации
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public DateTime RegistrationDate
        {
            get
            {
                return RegistrationDateString
                    .ParseIrbisDate();
            }
        }

        /// <summary>
        /// Дата перерегистрации. Поле 52.
        /// </summary>
        public RegistrationInfo[] Registrations;

        /// <summary>
        /// Дата последней перерегистрации.
        /// </summary>
        public DateTime LastRegistrationDate
        {
            get
            {
                if ((Registrations == null) || (Registrations.Length == 0))
                {
                    return DateTime.MinValue;
                }
                return Registrations.Last().Date;
            }
        }

        /// <summary>
        /// Последнее место регистрации.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public string LastRegistrationPlace
        {
            get
            {
                if ((Registrations == null) || (Registrations.Length == 0))
                {
                    return null;
                }
                return Registrations.Last().Place;
            }
        }

        /// <summary>
        /// Разрешенные места получения литературы. Поле 56.
        /// </summary>
        [XmlAttribute("enabled-places")]
        [JsonProperty("enabled-places")]
        public string EnabledPlaces { get; set; }

        /// <summary>
        /// Запрещенные места получения литературы. Поле 57.
        /// </summary>
        [XmlAttribute("disabled-places")]
        [JsonProperty("disabled-places")]
        public string DisabledPlaces { get; set; }

        /// <summary>
        /// Право пользования библиотекой. Поле 29.
        /// </summary>
        [XmlAttribute("rights")]
        [JsonProperty("rights")]
        public string Rights { get; set; }

        /// <summary>
        /// Примечания. Поле 33.
        /// </summary>
        [XmlAttribute("remarks")]
        [JsonProperty("remarks")]
        public string Remarks { get; set; }

        /// <summary>
        /// Фотография читателя. Поле 950.
        /// </summary>
        [XmlAttribute("photo-file")]
        [JsonProperty("photo-file")]
        public string PhotoFile { get; set; }

        /// <summary>
        /// Информация о посещениях.
        /// </summary>
        [XmlArray("visits")]
        [JsonProperty("visits")]
        public VisitInfo[] Visits;

        /// <summary>
        /// Профили обслуживания ИРИ.
        /// </summary>
        [XmlArray("iri")]
        [JsonProperty("iri")]
        public IriProfile[] Profiles;

        /// <summary>
        /// Возраст, годы
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public int Age
        {
            get
            {
                if (string.IsNullOrEmpty(Birthdate))
                {
                    return 0;
                }
                string yearText = Birthdate;
                if (yearText.Length > 4)
                {
                    yearText = yearText.Substring(1, 4);
                }
#if PocketPC
                int year = int.Parse(yearText);
#else
                int year;
                if (!int.TryParse(yearText, out year))
                {
                    return 0;
                }
#endif
                return DateTime.Today.Year - year;
            }
        }

        /// <summary>
        /// Возрастная категория.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public string AgeCategory
        {
            get
            {
                int age = Age;
                if (age > 65) return "> 65";
                if (age >= 55) return "55-64";
                if (age >= 45) return "45-54";
                if (age >= 35) return "35-44";
                if (age >= 25) return "25-34";
                if (age >= 18) return "18-24";
                return "< 18";
            }
        }

        /// <summary>
        /// Произвольные данные, ассоциированные с читателем.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public object UserData
        {
            get { return _userData; }
            set { _userData = value; }
        }

        /// <summary>
        /// Дата первого посещения
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public DateTime FirstVisitDate
        {
            get
            {
                if ((Visits == null) || (Visits.Length == 0))
                {
                    return DateTime.MinValue;
                }
                return Visits.First().DateGiven;
            }
        }

        /// <summary>
        /// Дата последнего посещения.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public DateTime LastVisitDate
        {
            get
            {
                if ((Visits == null) || (Visits.Length == 0))
                {
                    return DateTime.MinValue;
                }
                return Visits.Last().DateGiven;
            }
        }

        /// <summary>
        /// Кафедра последнего посещения.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public string LastVisitPlace
        {
            get
            {
                if ((Visits == null) || (Visits.Length == 0))
                {
                    return null;
                }
                return Visits.Last().Department;
            }
        }

        /// <summary>
        /// Последний обслуживавший библиотекарь.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public string LastVisitResponsible
        {
            get
            {
                if ((Visits == null) || (Visits.Length == 0))
                {
                    return null;
                }
                return Visits.Last().Responsible;
            }
        }

        #endregion

        #region Private members

        [NonSerialized]
        private object _userData;

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the specified field.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static ReaderInfo Parse(IrbisRecord record)
        {
            ReaderInfo result = new ReaderInfo
                                    {
                                        FamilyName = record.FM("10"),
                                        FirstName = record.FM("11"),
                                        Patronym = record.FM("12"),
                                        Birthdate = record.FM("21"),
                                        Ticket = record.FM("30"),
                                        Sex = record.FM("23"),
                                        Category = record.FM("50"),
                                        Address = ReaderAddress.Parse
                                            (
                                                record.Fields
                                                .GetField("13")
                                                .FirstOrDefault()
                                            ),
                                        Work = record.FM("15"),
                                        Education = record.FM("20"),
                                        Email = record.FM("32"),
                                        HomePhone = record.FM("17"),
                                        RegistrationDateString = record.FM("51"),
                                        Registrations = record.Fields
                                            .GetField("52")
                                            .Select(field=>RegistrationInfo.Parse(field))
                                            .ToArray(),
                                        EnabledPlaces = record.FM("56"),
                                        DisabledPlaces = record.FM("57"),
                                        Rights = record.FM("29"),
                                        Remarks = record.FM("33"),
                                        PhotoFile = record.FM("950"),
                                        Visits = record.Fields
                                            .GetField("40")
                                            .Select(field=>VisitInfo.Parse(field))
                                            .ToArray(),
                                        Profiles = IriProfile.ParseRecord(record)
                                    };

            string fio = result.FamilyName;
            if (!string.IsNullOrEmpty(result.FirstName))
            {
                fio = fio + " " + result.FirstName;
            }
            if (!string.IsNullOrEmpty(result.Patronym))
            {
                fio = fio + " " + result.Patronym;
            }
            result.Fio = fio;

            return result;
        }

        /// <summary>
        /// Формирование записи по данным о читателе.
        /// </summary>
        /// <returns></returns>
        public IrbisRecord ToRecord()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format
                (
                    "{0} - {1}", 
                    Ticket,
                    Fio
                );
        }

        #endregion
    }
}
