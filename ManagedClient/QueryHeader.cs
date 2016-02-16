/* QueryHeader.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient
{
	/// <summary>
	/// Заголовок запроса, уходящего на сервер.
	/// </summary>
	[Serializable]
	public sealed class QueryHeader
	{
		#region Properties

		/// <summary>
		/// Код команды.
		/// </summary>
		public char Command { get; set; }

		/// <summary>
		/// Код подкоманды.
		/// </summary>
		public char Subcommand { get; set; }

		/// <summary>
		/// Код АРМа.
		/// </summary>
		public char Workstation { get; set; }

		/// <summary>
		/// Идентификатор программы-клиента.
		/// </summary>
		public int ClientID { get; set; }

		/// <summary>
		/// Идентификатор запроса (последовательно возрастающее целое).
		/// </summary>
		public int QueryID { get; set; }

		/// <summary>
		/// Пароль.
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// Логин.
		/// </summary>
		public string UserName { get; set; }

		#endregion

		#region Private members

		private string _GlueCommand ()
		{
			StringBuilder result = new StringBuilder(Command.ToInvariantString());
			if (Subcommand != 0)
			{
				result.Append(Subcommand);
			}
			return result.ToString();
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Кодирование заголовка запроса в формат, 
		/// пригодный для отправки на сервер.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> Encode()
		{
			string glued = _GlueCommand();
			List<string> result = new List<string>
				                      {
					                      glued,
					                      Workstation.ToInvariantString(),
					                      glued,
					                      ClientID.ToInvariantString(),
										  QueryID.ToInvariantString()
				                      };

            // Документация на сервер неверно описывает формат 
            // клиентского пакета. На самом деле он таков (для команды А):
            // общая длина пакета
            // 'A'
            // АРМ, например 'C'
            // 'A'
            // ID клиента, например 523444
            // номер команды, для A всегда 1
            // пароль
            // имя пользователя
            // пустая строка
            // пустая строка
            // пустая строка

			if (!string.IsNullOrEmpty(Password))
			{
				result.Add(Password);
			}

			if (!string.IsNullOrEmpty(UserName))
			{
				result.Add(UserName);
			}

			while (result.Count < 10)
			{
				result.Add(string.Empty);
			}

			return result;
		}

		#endregion

		#region Object members

		public override string ToString()
		{
			return string.Join
				(
					"\r\n",
					Encode().ToArray()
				);
		}

		#endregion
	}
}
