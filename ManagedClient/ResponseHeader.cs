/* ResponseHeader.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Заголовок ответа сервера
    /// </summary>
    [Serializable]
	public sealed class ResponseHeader
    {
        #region Properties

        /// <summary>
        /// Повторенный код команды
        /// </summary>
        public char Command { get; set; }

        /// <summary>
        /// Повторенный идентификатор клиента
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Повторенный идентификатор команды
        /// </summary>
        public int QueryID { get; set; }

        /// <summary>
        /// Собственно ответ сервера (данные).
        /// Часто первая строка содержит код возврата
        /// </summary>
        public List<string> Data { get; set; }

        /// <summary>
        /// Соединенные данные ответа сервера.
        /// </summary>
        public string JoinedData
        {
            get
            {
                return string.Join
                    (
                        Environment.NewLine,
                        Data.Skip(1).ToArray()
                    );
            }
        }

        /// <summary>
        /// Код возврата (код ошибки)
        /// </summary>
        public int ReturnCode { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        /// <param name="text">Текст ответа сервера</param>
        /// <returns></returns>
        public static ResponseHeader Parse(string text)
        {
            text = text.Replace("\x000D\x000A","\x000D");

            string[] lines = text.Split                
                (
                    '\x000D'
                );

            ResponseHeader result = new ResponseHeader();

            try
            {
                result.Command = lines[0][0];
                result.UserID = int.Parse(lines[1]);
                result.QueryID = int.Parse(lines[2]);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

			result.Data = new List<string>();
			if (lines.Length > 10)
            {
                for (int i = 10; i < lines.Length; i++)
                {
                    result.Data.Add(lines[i]);
                }
            }

            if (result.Data.Count != 0)
            {
                result.ReturnCode = result.Data[0].SafeParseInt32(0);
            }
            else
            {
	            result.ReturnCode = -1;
            }

            return result;
        }

        #endregion

        #region Object members

        public override string ToString ( )
        {
            return string.Format 
                ( 
                    "Command: {0}, UserID: {1}, QueryID: {2}, ReturnCode: {3}", 
                    Command, 
                    UserID, 
                    QueryID, 
                    ReturnCode 
                );
        }

        #endregion
    }
}