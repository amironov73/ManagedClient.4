/* IrbisServerStat.cs
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
	/// Статистика работы Ирбис-сервера
	/// </summary>
	[Serializable]
	public sealed class IrbisServerStat
	{
		#region Properties

		public IrbisClientInfo[] Clients { get; set; }

        public int Unknown1 { get; set; }

        public int Unknown2 { get; set; }

		public int TotalCommandCount { get; set; }

		#endregion

		#region Public methods

		public static IrbisServerStat Parse ( string[] text )
		{
			IrbisServerStat result = new IrbisServerStat
			                             {
			                                 TotalCommandCount = int.Parse ( text [ 1 ] ),
                                             Unknown1 = int.Parse ( text[2] ),
                                             Unknown2 = int.Parse ( text[3] )
			                             };

		    List <IrbisClientInfo> clients = new List < IrbisClientInfo > ();

		    for ( int index = 4; index < (text.Length - 10); index += 10 )
		    {
		        IrbisClientInfo client = new IrbisClientInfo
		                                     {
		                                         Number = text[index],
                                                 IPAddress = text[index+1],
                                                 Port = text[index+2],
                                                 Name = text[index+3],
                                                 ID = text[index+4],
                                                 Workstation = text[index+5],
                                                 Registered = text[index+6],
                                                 Acknowledged = text[index+7],
                                                 LastCommand = text[index+8],
                                                 CommandNumber = text[index+9]
		                                     };
                clients.Add ( client );
		    }
		    result.Clients = clients.ToArray ();
			return result;
		}

		#endregion
	}
}
