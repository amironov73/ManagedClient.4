/* IrbisServerSettings.cs
 */

#region Using directives

using System.Text;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Серверные настройки (живущие в irbis_server.ini).
    /// </summary>
    public sealed class IrbisServerSettings
        : IniFile
    {
        #region Constants

        public const string IniFileName = "irbis_server.ini";

        public const string Main = "MAIN";

        #endregion

        #region Properties

        /// <summary>
        /// Путь на таблицу isisacw.
        /// </summary>
        public string ACTabPath
        {
            get { return Get(Main, "ACTABPATH", @"C:\IRBIS64\isisacw"); }
        }

        /// <summary>
        /// Файл автоввода.
        /// </summary>
        public string AutoInFile
        {
            get { return Get(Main, "AutoInFile", "autoin.gbl"); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string BriefPft
        {
            get { return Get(Main, "BriefPft", @"brief"); }
        }

        /// <summary>
        /// Проверять протокол на перенаправление запроса с другого сервера.
        /// </summary>
        public bool CheckRedirect
        {
            get { return Get(Main, "CHECK_REDIRECT", false); }
        }

        /// <summary>
        /// Имя файла со списком клиентов с паролями для доступа к серверу.
        /// </summary>
        public string ClientList
        {
            get { return Get(Main, "CLIENTLIST", @"client_m.mnu"); }
        }

        /// <summary>
        /// Время жизни клиента без подтверждения (в мин.) 
        /// По умочанию 0 – режим отключен.
        /// </summary>
        public int ClientTimeLive
        {
            get { return Get(Main, "CLIENT_TIME_LIVE", 0); }
        }

        /// <summary>
        /// Путь к системным меню и параметрическим файлам БД.
        /// </summary>
        public string DataPath
        {
            get { return Get ( Main,  "DATAPATH", @"C:\IRBIS64\DATAI\"); }
        }

        /// <summary>
        /// Файл со списком баз данных для администратора.
        /// </summary>
        public string DbNameCat
        {
            get { return Get(Main, "DBNAMECAT", "dbnam1.mnu"); }
        }

        /// <summary>
        /// Файл со списком баз данных для читателя.
        /// </summary>
        public string DbNameCatR
        {
            get { return Get(Main, "DBNAMECATR", "dbnam3.mnu"); }
        }

        /// <summary>
        /// Файл проверки на дублетность.
        /// </summary>
        public string DbnFlc
        {
            get { return Get(Main, "Dbnflc", @"dbnflc"); }
        }

        /// <summary>
        /// Применять отсечку при актуализации?
        /// </summary>
        public bool DeflexKW
        {
            get { return Get(Main, "DeflexKW", false); }
        }

        /// <summary>
        /// Включение многопроцессорного режима 
        /// (когда процесс обработки выполняет сетевое чтение-запись).
        /// </summary>
        public bool DuplicateSockets
        {
            get { return Get(Main, "DUPLICATE_SOCKETS", true); }
        }

        /// <summary>
        /// Обмен между процессами обработки и ядром сервера - через 
        /// системную память (1) или через временные файлы в рабочей 
        /// директории workdir (0).
        /// </summary>
        public bool DupMappingWorkFiles 
        {
            get { return Get(Main, "DUP_MAPING_WORK_FILES", true); }
        }

        /// <summary>
        /// Размер системной памяти, выделяемой процессу, Kb.
        /// </summary>
        public int DupMappingFileSize
        {
            get { return Get(Main, "Dup_MappingFileSize", 100); }
        }

        /// <summary>
        /// Число процессов обработки, стартуемых сервером при запуске.
        /// </summary>
        public int DupProcessCountPull
        {
            get { return Get(Main, "Dup_ProcessCountPull", 2); }
        }

        /// <summary>
        /// Эталонная (пустая) база данных.
        /// </summary>
        public string EmptyDbn
        {
            get { return Get(Main, "EmptyDBN", "BLANK"); }
        }

        /// <summary>
        /// Шифровать профили клиентов.
        /// </summary>
        public bool EncryptPasswords
        {
            get { return Get(Main, "ENCRYPT_PASSWORDS", false); }
        }

        /// <summary>
        /// Кеширование форматов.
        /// </summary>
        public bool FormatCacheable
        {
            get { return Get(Main, "FORMAT_CASHABLE", true); }
        }

        /// <summary>
        /// IP адрес сервера используется только для показа в таблице описателей.
        /// </summary>
        public string IPAddress
        {
            get { return Get(Main, "IP_ADDRESS", @"127.0.0.1"); }
        }

        /// <summary>
        /// IP порт сервера.
        /// </summary>
        public int IPPort
        {
            get { return Get(Main, "IP_PORT", 6666); }
        }

        /// <summary>
        /// Cигнал окончания процесса обработки посылается через 
        /// TCP на порт 7778, а не как сообщение windows. 
        /// В этом случае RegisterWindowMessage игнорируется.
        /// </summary>
        public int IPPortLocal
        {
            get { return Get(Main, "IP_PORT_LOCAL", 7778); }
        }

        /// <summary>
        /// Разрешает серверу использовать процесс обработки многократно.
        /// </summary>
        public bool KeepProcessAlive
        {
            get { return Get(Main, "KEEP_PROCESS_ALIVE", true); }
        }

        /// <summary>
        /// Cигнал окончания процесса обработки посылается через TCP 
        /// на порт 7778, а не как сообщение windows. 
        /// В этом случае RegisterWindowMessage игнорируется.
        /// </summary>
        public bool ListenResponse
        {
            get { return Get(Main, "LISTEN_RESPONSE", true); }
        }

        /// <summary>
        /// Размер системной памяти, выделяемой клиенту, Мб.
        /// </summary>
        public int MappingFileSize
        {
            get { return Get(Main, "MappingFileSize", 1); }
        }

        /// <summary>
        /// Обмен между процессами обработки и ядром сервера - через 
        /// системную память (1)  или через временные файлы (0) 
        /// в рабочей директории workdir. Если системной памяти не хватает, 
        /// происходит обмен через файл. При включении этого режима, 
        /// необходимо также включить проверку клиентов на подтверждение 
        /// - CLIENT_TIME_LIVE, чтобы за ними не оставалась выделенная 
        /// память.
        /// </summary>
        public bool MappingWorkFiles
        {
            get { return Get(Main, "MAPPING_WORK_FILES", true); }
        }

        /// <summary>
        /// Размер лог-файла, байты.
        /// </summary>
        public int MaxLogFileSize
        {
            get { return Get(Main, "MaxLogFileSize", 1000000); }
        }

        /// <summary>
        /// Максимально возможное число процессов обработки, 
        /// если превышено - возвращается ошибка SERVER_OVERLOAD. 
        /// По умолчанию = 20.
        /// </summary>
        public int MaxProcessCount
        {
            get { return Get(Main, "MAX_PROCESS_COUNT", 20); }
        }

        /// <summary>
        /// Максимально возможное число запросов к долгоживущему 
        /// процессу обработки, после чего процесс автоматически 
        /// прерывается. По умолчанию = 100.
        /// </summary>
        public int MaxProcessRequests
        {
            get { return Get(Main, "MAX_PROCESS_REQUESTS", 100); }
        }

        /// <summary>
        /// Максимальный размер буфера ответа в байтах  - если превышен 
        /// ответ разбивается на 2-ве части.
        /// Если 0 разбивки нет
        /// </summary>
        public int MaxResponseLength
        {
            get { return Get(Main, "MAX_RESPONSE_LENGTH", 0); }
        }

        /// <summary>
        /// Максимальное число процессов обработки, которые сервер использует 
        /// многократно (только если KEEP_PROCESS_ALIVE = 1).
        /// </summary>
        public int MaxServers
        {
            get { return Get(Main, "MAX_SERVERS", 20); }
        }

        /// <summary>
        /// Максимально возможное количество потоков.
        /// Если превышено – сервер переходит в режим последовательного
        /// чтения-записи.
        /// </summary>
        public int MaxThreadCount
        {
            get { return Get(Main, "MAX_THREADS_COUNT", 10); }
        }

        /// <summary>
        /// Минимальное количество потоков в очереди.
        /// </summary>
        public int MinThreadCount
        {
            get { return Get(Main, "MIN_THREADS_COUNT", 1); }
        }

        /// <summary>
        /// Файл оптимизации форматов.
        /// </summary>
        public string PftOpt
        {
            get { return Get(Main, "PftOpt", @"pftw.opt"); }
        }

        /// <summary>
        /// Повысить приоритет процесса до HIGH_PRIORITY_CLASS.
        /// </summary>
        public bool ProcessPriority
        {
            get { return Get(Main, "PROCESS_PRIORITY", false); }
        }

        /// <summary>
        /// Время мониторинга в сек. процессов и потоков на соответствие друг другу. 
        /// Если 0 – режим отключен. 10 сек по умолчанию.
        /// </summary>
        public int ProcessThreadsMonitor
        {
            get { return Get(Main, "PROCESS_THREADS_MONITOR", 10); }
        }

        /// <summary>
        /// Максимальное время обработки запроса (в мин.) 
        /// По умочанию 0 – режим отключен.
        /// </summary>
        public int ProcessTimeLive
        {
            get { return Get(Main, "PROCESS_TIME_LIVE", 0); }
        }

        /// <summary>
        /// Разрешать (определять) адрес машины клиента при регистрации.
        /// </summary>
        public bool RecognizeClientAddress
        {
            get { return Get(Main, "RECOGNIZE_CLIENT_ADDRESS", true); }
        }

        /// <summary>
        /// Сигнал обмена сообщениями между сервером и процессами обработки 
        /// регистрируется в системе WINDOWS и получает уникальный идентификатор.
        /// </summary>
        public bool RegisterWindowMessage
        {
            get { return Get(Main, "RegisterWindowMessage", true); }
        }

        /// <summary>
        /// Ожидание эксклюзивного доступа на запись.
        /// </summary>
        public int RelayRecTime
        {
            get { return Get(Main, "Relay_RecTime", 5); }
        }

        /// <summary>
        /// Префикс файла обмена - запрос.
        /// </summary>
        public string RequestFilePrefix
        {
            get { return Get(Main, "Prefix_file_request", "REQUEST_"); }
        }

        /// <summary>
        /// Префикс файла обмена - ответ.
        /// </summary>
        public string ResponseFilePrefix
        {
            get { return Get(Main, "Prefix_file_response", "RESPONSE_"); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ServerExecutable
        {
            get { return Get(Main, "Server_Exe", "server_64.exe"); }
        }

        /// <summary>
        /// Меню статистики.
        /// </summary>
        public string StatisticsMenu
        {
            get { return Get(Main, "STTMNU", @"stt.mnu"); }
        }

        /// <summary>
        /// Не выводить windows-сообщения о непредвиденных 
        /// ошибках в процессах обработки server_64.exe. 
        /// Этот параметр рекомендуется использовать, 
        /// если во время эксплуатации сервера выводятся сообщения 
        /// об ошибках в server_64.exe.
        /// </summary>
        public bool SuppressExceptions
        {
            get { return Get(Main, "SUPPRESS_EXCEPTIONS", true); }
        }

        /// <summary>
        /// Путь к системным (INI) файлам.
        /// </summary>
        public string SystemPath
        {
            get { return Get(Main, "SYSPATH", @"C:\IRBIS64"); }
        }

        /// <summary>
        /// Включение режима параллельной обработки чтения-записи
        /// запросов клиентов в многопотоковом режиме.
        /// </summary>
        public bool ThreadsAvailable
        {
            get { return Get(Main, "THREADS_AVAILABLE", true); }
        }

        /// <summary>
        /// Блокировка всех параллельных потоков, кроме текущего, 
        /// на время чтения-записи.
        /// </summary>
        public bool ThreadsLocking
        {
            get { return Get(Main, "THREADS_LOCKING", false); }
        }

        /// <summary>
        /// Этот параметр эффективен для терминальной работы клиентов.
        /// Время ожидания завершения передачи по сети в ms.
        /// Если 0 - нет ожидания. По умолчанию=1.
        /// </summary>
        public int TimeSleepOnClose
        {
            get { return Get(Main, "TimeSleepOnClose", 1); }
        }

        /// <summary>
        /// Путь на таблицу isisucw.
        /// </summary>
        public string UCTabPath
        {
            get { return Get(Main, "UCTABPATH", @"C:\IRBIS64\isisucw"); }
        }

        /// <summary>
        /// Распараллеливать процессы на несколько процессоров.
        /// </summary>
        public bool UseMultiProcessor
        {
            get { return Get(Main, "USE_MULTY_PROCESSOR", false); }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Utf8Representation
        {
            get { return Get(Main, "UTF8_REPRESANTATION", false); }
        }

        /// <summary>
        /// Директория для сохранения временных файлов, 
        /// используемых для межпроцессорного взаимодействия сервера 
        /// и процессов обработки.
        /// </summary>
        public string WorkingDirectory
        {
            get { return Get(Main, "WORKDIR", @"C:\IRBIS64\workdir"); }
        }

        #endregion

        #region Construction

        #endregion

        #region Public methods

        public static IrbisServerSettings FromLines
            (
                string[] lines
            )
        {
            return ParseLines<IrbisServerSettings>(lines);
        }

        public static IrbisServerSettings FromFile
            (
                string fileName
            )
        {
            return ParseFile<IrbisServerSettings>
                (
                    fileName, 
                    Encoding.Default
                );
        }

        public static IrbisServerSettings FromServer
            (
                ManagedClient64 client
            )
        {
            string text = client.ReadTextFile
                (
                    IrbisPath.System,
                    IniFileName
                );
            return ParseText<IrbisServerSettings>(text);
        }

        #endregion
    }
}
