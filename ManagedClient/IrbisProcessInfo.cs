/* IrbisProcessInfo.cs
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Информация о запущенном на сервере процессе.
    /// </summary>
    [Serializable]
    public sealed class IrbisProcessInfo
    {
        #region Properties

        /// <summary>
        /// Просто порядковый номер процесса.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// С каким клиентом взаимодействует.
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// Логин оператора.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Идентификатор клиента.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Тип АРМ.
        /// </summary>
        public string Workstation { get; set; }

        /// <summary>
        /// Время запуска.
        /// </summary>
        public string Started { get; set; }

        /// <summary>
        /// Последняя выполненная (или выполняемая) команда.
        /// </summary>
        public string LastCommand { get; set; }

        /// <summary>
        /// Порядковый номер последней команды.
        /// </summary>
        public string CommandNumber { get; set; }

        /// <summary>
        /// Идентификатор процесса.
        /// </summary>
        public string ProcID { get; set; }

        /// <summary>
        /// Состояние.
        /// </summary>
        public string State { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        public static IrbisProcessInfo[] Parse 
            ( 
                string[] text 
            )
        {
            List <IrbisProcessInfo> result = new List < IrbisProcessInfo > ();

            for ( int index = 3; index < (text.Length - 10); index += 10 )
            {
                IrbisProcessInfo info = new IrbisProcessInfo
                                            {
                                                Number = text[index],
                                                IPAddress = text[index+1],
                                                Name = text[index+2],
                                                ID=text[index+3],
                                                Workstation = text[index+4],
                                                Started = text[index+5],
                                                LastCommand = text[index+6],
                                                CommandNumber = text[index+7],
                                                ProcID = text[index+8],
                                                State = text[index+9]
                                            };
                result.Add ( info );
            }

            return result.ToArray ();
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" /> 
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> 
        /// that represents this instance.</returns>
        public override string ToString ()
        {
            return string.Format 
                ( 
                    "Number: {0}, IPAddress: {1}, " 
                  + "Name: {2}, ID: {3}, Workstation: {4}, "
                  + "Started: {5}, LastCommand: {6}, "
                  + "CommandNumber: {7}, ProcID: {8}, "
                  + "State: {9}", 
                    Number, 
                    IPAddress, 
                    Name, 
                    ID, 
                    Workstation, 
                    Started, 
                    LastCommand, 
                    CommandNumber, 
                    ProcID, 
                    State 
                );
        }

        #endregion
    }
}
