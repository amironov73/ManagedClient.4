/* IrbisVersion.cs
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

namespace ManagedClient
{
	/// <summary>
	/// Информация о версии Ирбис-сервера.
	/// </summary>
	[Serializable]
	public sealed class IrbisVersion
	{
		#region Properties

		/// <summary>
		/// На кого приобретен.
		/// </summary>
		public string Organization { get; set; }

		/// <summary>
		/// Собственно версия.
		/// </summary>
		/// <remarks>Например, 64.2008.1</remarks>
		public string Version { get; set; }

		/// <summary>
		/// Максимальное количество подключений.
		/// </summary>
		public int MaxClients { get; set; }

		/// <summary>
		/// Текущее количество подключений.
		/// </summary>
		public int ConnectedClients { get; set; }

		#endregion

		#region Public methods

		/// <summary>
		/// Разбор ответа сервера.
		/// </summary>
		/// <param name="lines"></param>
		/// <returns></returns>
		public static IrbisVersion Parse ( List<string> lines )
		{
				IrbisVersion result = new IrbisVersion
					                      {
											  Organization = lines[1],
						                      Version = lines[2],
											  ConnectedClients = int.Parse(lines[3]),
											  MaxClients = int.Parse(lines[4])
										  };

				return result;
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
			return string.Format
				(
					"Version: {0}, MaxClients: {1}, ConnectedClients: {2}, Organization: {3}", 
					Version, 
					MaxClients, 
					ConnectedClients,
					Organization
				);
		}

		#endregion
	}
}
