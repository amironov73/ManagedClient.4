﻿/* IrbisPath.cs -- path codes for Irbis64
   Ars Magna project, http://library.istu.edu/am */

namespace ManagedClient
{
    /// <summary>
    /// Задает путь к файлам Ирбис
    /// </summary>
    public enum IrbisPath
    {
        /// <summary>
        /// Общесистемный путь
        /// </summary>
        System = 0,

        /// <summary>
        /// путь размещения сведений о базах данных сервера ИРБИС64
        /// </summary>
        Data = 1,

        /// <summary>
        /// путь на мастер-файл базы данных
        /// </summary>
        MasterFile = 2,

        /// <summary>
        /// путь на словарь базы данных
        /// </summary>
        InvertedFile = 3,
        
        /// <summary>
        /// путь на параметрию базы данных
        /// </summary>
        ParameterFile = 10,

        /// <summary>
        /// Полный текст
        /// </summary>
        FullText = 11,
        
        /// <summary>
        /// Внутренний ресурс
        /// </summary>
        InternalResource = 12
    }
}
