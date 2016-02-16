/* IrbisIniFile.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient
{
    [Serializable]
    public sealed class IrbisIniFile
        : IniFile
    {
        #region Constants

        public const string Entry = "ENTRY";

        public const string Main = "MAIN";

        public const string Reader = "READER";

        public const string Request = "REQUEST";

        #endregion

        #region Properties

        /// <summary>
        /// Имя файла пакетного задания для АВТОВВОДА.
        /// </summary>
        public string AutoinFile
        {
            get { return GetString(Main, "AUTOINFILE", "autoin.gbl"); }
        }

        /// <summary>
        /// Определяет характер выполнения режима ВЫДАЧА БЕЗ ЗАКАЗА в АРМе 
        /// Книговыдача – в случае когда в поле КЛЮЧ вводят данные, 
        /// однозначно определяющие экземпляр (например, штрих-код), 
        /// и далее ENTER. Принимает два значения: 1 – автоматически 
        /// выполняется выдача; 0 – не выполняется.
        /// </summary>
        public bool AutoLand
        {
            get { return Get(Main, "AUTOLAND", true); }
        }

        /// <summary>
        /// Разрешает (значение 1) или запрещает (значение 0) 
        /// автоматическое слияние двух версий записи при корректировке 
        /// (при получении сообщения о несовпадении версий – в ситуации, 
        /// когда одну запись пытаются одновременно откорректировать 
        /// два и более пользователей) Автоматическое слияние проводится 
        /// по формальному алгоритму: неповторяющиеся поля заменяются, 
        /// а оригинальные значения повторяющихся полей суммируются
        /// </summary>
        public bool AutoMerge
        {
            get { return Get(Main, "AUTOMERGE", false); }
        }

        /// <summary>
        /// Имя краткого (однострокового) формата показа.
        /// </summary>
        public string BriefPft
        {
            get { return GetString(Main, "BRIEFPFT", "brief"); }
        }

        /// <summary>
        /// Интервал в мин., по истечении которого клиент посылает 
        /// на сервер уведомление о том, что он «жив».
        /// </summary>
        public int ClientTimeLive
        {
            get { return Get(Main, "CLIENT_TIME_LIVE", 15); }
        }

        /// <summary>
        /// Имя файла-справочника со списком ТВП переформатирования 
        /// для копирования.
        /// </summary>
        public string CopyMnu
        {
            get { return GetString(Main, "COPYMNU", "fst.mnu"); }
        }

        /// <summary>
        /// Имя формата для ФЛК документа в целом.
        /// </summary>
        public string DbnFlc
        {
            get { return GetString(Main, "DBNFLC", "dbnflc"); }
        }

        /// <summary>
        /// Имя файла-справочника со списком доступных баз данных.
        /// В списках БД доступных АРМам Каталогизатор и Читатель 
        /// именам конкретных БД может предшествовать символ “-“ (минус), 
        /// что означает: - для АРМа Каталогизатор – что соответствующая 
        /// БД не доступна для ввода; - для АРМа Читатель – что 
        /// соответствующая БД не доступна для заказа.
        /// </summary>
        public string DbnNameCat
        {
            get { return GetString(Main, "DBNNAMECAT", "dbnam1.mnu"); }
        }

        /// <summary>
        /// Префикс инверсии для шифра документа в БД ЭК.
        /// </summary>
        public string DbnPrefShifr
        {
            get { return GetString(Main, "DBNPREFSHIFR", "I="); }
        }

        /// <summary>
        /// Метка поля «экземпляры» в БД ЭК.
        /// </summary>
        public string DbnTagEkz
        {
            get { return GetString(Main, "DBNTAGEKZ", "910"); }
        }

        /// <summary>
        /// Метка поля «шифр документа» в БД ЭК.
        /// </summary>
        public string DbnTagShifr
        {
            get { return GetString(Main, "DBNTAGSHIFR", "903"); }
        }

        /// <summary>
        /// Метка поля «количество выдач» в БД ЭК.
        /// </summary>
        public string DbnTagSpros
        {
            get { return GetString(Main, "DBNTAGSPROS", "999"); }
        }

        /// <summary>
        /// Имя БД по умолчанию.
        /// </summary>
        public string DefaultDB
        {
            get { return GetString(Main, "DEFAULTDB", string.Empty); }
        }

        /// <summary>
        /// Имя шаблона для создания новой БД.
        /// </summary>
        public string EmptyDbn
        {
            get { return GetString(Main, "EMPTYDBN", "BLANK"); }
        }

        /// <summary>
        /// Имя эталонной БД Электронного каталога.
        /// </summary>
        public string EtalonDbn
        {
            get { return GetString(Main, "ETALONDBN", "IBIS"); }
        }

        /// <summary>
        /// Имя файла-справочника со списком ТВП переформатирования 
        /// для экспорта.
        /// </summary>
        public string ExportMnu
        {
            get { return GetString(Main, "EXPORTMNU", "export.mnu"); }
        }

        /// <summary>
        /// Имя файла-справочника со списком доступных РЛ.
        /// </summary>
        public string FmtMnu
        {
            get { return GetString(Main, "FMTMNU", "fmt.mnu"); }
        }

        /// <summary>
        /// Номер основного кодового набора.
        /// </summary>
        public int FontCharset
        {
            get { return Get(Main, "FONTCHARSET", 204); }
        }

        /// <summary>
        /// Имя шрифта для компонентов интерфейса.
        /// </summary>
        public string FontName
        {
            get { return GetString(Main, "FONTNAME", "Arial"); }
        }

        /// <summary>
        /// Размер шрифта на пользовательском интерфейсе. 
        /// Возможные значения: 0,1,2,3,4.
        /// </summary>
        public int FontSize
        {
            get { return Get(Main, "FONTSIZE", 0); }
        }

        /// <summary>
        /// Имя БД, содержащей тематический рубрикатор ГРНТИ.
        /// </summary>
        public string HelpDbn
        {
            get { return GetString(Main, "HELPDBN", "HELP"); }
        }

        /// <summary>
        /// Имя файла-справочника со списком ТВП переформатирования 
        /// для импорта.
        /// </summary>
        public string ImportMnu
        {
            get { return GetString(Main, "IMPORTMNU", "import.mnu"); }
        }

        /// <summary>
        /// Имя файла-справочника со списком постоянных запросов.
        /// </summary>
        public string IriMnu
        {
            get { return GetString(Main, "IRIMNU", "iri.mnu"); }
        }

        /// <summary>
        /// Доступность режима ЧИТАТЕЛИ-ВЫДАЧА БЕЗ ЗАКАЗА в АРМе 
        /// «Книговыдача» (1 – доступен, 0 – недоступен).
        /// </summary>
        public bool Landable
        {
            get { return Get(Main, "LANDABLE", true); }
        }

        /// <summary>
        /// Имя формата для показа документа ЭК при выдаче 
        /// без заказа в АРМе «Книговыдача».
        /// </summary>
        public string LandFormat
        {
            get { return GetString(Main, "LANDFORMAT", "brief"); }
        }

        /// <summary>
        /// Название отправителя почты для режимов печати в АРМах 
        /// «Читатель» и «Книговыдача».
        /// </summary>
        public string MailFrom
        {
            get { return GetString(Main, "MAILFROM", string.Empty); }
        }

        /// <summary>
        /// Адрес почтового сервера (отправителя) для режимов печати 
        /// в АРМах «Читатель» и «Книговыдача».
        /// </summary>
        public string MailHost
        {
            get { return GetString(Main, "MAILHOST", string.Empty); }
        }

        /// <summary>
        /// Маска штрих-кода для очереди заказов.
        /// </summary>
        public string MaskBar
        {
            get { return GetString(Request, "MASKBAR", "*"); }
        }

        /// <summary>
        /// Маска имени БД ЭК для очереди заказов.
        /// </summary>
        public string MaskDbn
        {
            get { return GetString(Request, "MASKDBN", "*"); }
        }

        /// <summary>
        /// Маска инв.номера для очереди заказов.
        /// </summary>
        public string MaskInv
        {
            get { return GetString(Request, "MASKINV", "*"); }
        }

        /// <summary>
        /// Маска идентификатора читателя для очереди заказов.
        /// </summary>
        public string MaskReader
        {
            get { return GetString(Request, "MASKREADER", "*"); }
        }

        /// <summary>
        /// Маска шифра документа для очереди заказов.
        /// </summary>
        public string MaskShifr
        {
            get { return GetString(Request, "MASKSHIFR", "*"); }
        }

        /// <summary>
        /// Маска места хранения для очереди заказов.
        /// </summary>
        public string MaskStore
        {
            get { return GetString(Request, "MASKSTORE", "*"); }
        }

        /// <summary>
        /// Макс. кол-во изданий на руках читателя.
        /// </summary>
        public int MaxBooks
        {
            get { return Get(Reader, "MAXBOOKS", 10); }
        }

        /// <summary>
        /// Макс. кол-во задолженных изданий на руках у читателя.
        /// </summary>
        public int MaxDolgBooks
        {
            get { return Get(Reader, "MAXDOLGBOOKS", 1); }
        }

        /// <summary>
        /// Определяет возможность фиксирования множественного 
        /// (больше одного) посещения одного читателя в течение 
        /// одного дня (не связанное с выдачей/возвратом). 
        /// Принимает два значения: 0  - нельзя; 1 – можно.
        /// </summary>
        public bool MultiVisit
        {
            get { return Get(Main, "MULTIVISIT", false); }
        }

        /// <summary>
        /// Имя файла-справочника со списком доступных форматов 
        /// показа документов.
        /// </summary>
        public string PftMnu
        {
            get { return GetString(Main, "PFTMNU", "pft.mnu"); }
        }

        /// <summary>
        /// Имя оптимизационного файла, который определяет принцип 
        /// формата ОПТИМИЗИРОВАННЫЙ (в АРМах Читатель и Каталогизатор).
        /// Для БД электронного каталога (IBIS) значение PFTW.OPT 
        /// определяет в качестве оптимизированных  RTF-форматы, 
        /// а значение PFTW_H.OPT – HTML-форматы
        /// </summary>
        public string PftOpt
        {
            get { return GetString(Main, "PFTOPT", "pft.opt"); }
        }

        /// <summary>
        /// Префикс инверсии для поиска записи в каталоге по инвентарному 
        /// номеру/штрих-коду при переносе в ЭК. 
        /// </summary>
        public string PrefInv
        {
            get { return GetString(Entry, "PREFINV", "IN="); }
        }

        /// <summary>
        /// Включение (значение 1) или выключение (значение 0) контроля 
        /// соответствия места выдачи и места хранения экземпляра 
        /// в АРМе «Книговыдача».
        /// </summary>
        public bool PrMhrKv
        {
            get { return Get(Main, "PRMHRKV", false); }
        }

        /// <summary>
        /// Имя формата краткого описания в БД читателей.
        /// </summary>
        public string RdrBriefFormat
        {
            get { return GetString(Reader, "REDRBRIEFFORMAT", "RDR0"); }
        }

        /// <summary>
        /// Имя БД читателей.
        /// </summary>
        public string RdrDbn
        {
            get { return GetString(Reader, "RDRDBN", "RDR"); }
        }

        /// <summary>
        /// Имя файла-справочника со списком форматов для должников.
        /// </summary>
        public string RdrPftMnu
        {
            get { return GetString(Reader, "RDRPFTMNU", "dolg.mnu"); }
        }

        /// <summary>
        /// Имя дополнительного INI-файла со сценарием поиска для БД. 
        /// Если соответствующий файл отсутствует, используется сценарий 
        /// из dbn.ini. Указывается только имя INI-файла. Сам файл должен 
        /// находиться в директории БД.
        /// </summary>
        public string SearchIni
        {
            get { return GetString(Main, "SEARCHINI", string.Empty); }
        }

        /// <summary>
        /// Имя справочника со списком Web-ресурсов для режима 
        /// ИМПОРТ ИЗ WEB-ИРБИС АРМа Каталогизатор.
        /// </summary>
        public string WebIrbisMnu
        {
            get { return GetString(Main, "WEBIRBISMNU", "webirbis.mnu"); }
        }

        /// <summary>
        /// Имя справочника со списком ресурсов Web-ИРБИС для передачи данных.
        /// </summary>
        public string WebTransferMnu
        {
            get { return GetString(Main, "WEBTRANSFERMNU", "webtransfer.mnu"); }
        }

        /// <summary>
        /// Директория для сохранения временных (выходных) данных.
        /// </summary>
        public string WorkDir
        {
            get { return GetString(Main, "WORKDIR", "C:\\irbiswrk"); }
        }

        /// <summary>
        /// Признак, разрешающий формирование протокола пакетных режимов.
        /// </summary>
        public bool WriteLog
        {
            get { return Get(Main, "WRITELOG", true); }
        }

        /// <summary>
        /// Имя файла оптимизации РЛ ввода.
        /// </summary>
        public string WsOpt
        {
            get { return GetString(Main, "WSOPT", "ws.opt"); }
        }

    #endregion
    }
}
