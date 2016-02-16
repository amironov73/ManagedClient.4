﻿/* IrbisDatabaseInfo.cs
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
	/// Информация о базе данных Ирбис
	/// </summary>
	[Serializable]
	public sealed class IrbisDatabaseInfo
	{
		#region Constants

		public const char ItemDelimiter = (char)0x1E;

		#endregion

		#region Properties

		/// <summary>
		/// Имя базы данных.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание базы данных
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Максимальный MFN.
		/// </summary>
		public int MaxMfn { get; set; }

		/// <summary>
		/// Список логически удаленных записей.
		/// </summary>
		public int[] LogicallyDeletedRecords { get; set; }

		/// <summary>
		/// Список физически удаленных записей.
		/// </summary>
		public int[] PhysicallyDeletedRecords { get; set; }

		/// <summary>
		/// Список неактуализированных записей.
		/// </summary>
		public int[] NonActualizedRecords { get; set; }

		/// <summary>
		/// Список заблокированных записей.
		/// </summary>
		public int[] LockedRecords { get; set; }

		/// <summary>
		/// Флаг монопольной блокировки базы данных.
		/// </summary>
		public bool DatabaseLocked { get; set; }

        /// <summary>
        /// База данных только для чтения.
        /// </summary>
        public bool ReadOnly { get; set; }

		#endregion

		#region Private members

		private static int[] _ParseLine(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return new int[0];
			}

			string[] items = text.Split(ItemDelimiter);
			int[] result = items
				.Select(_ => int.Parse(_))
				.OrderBy(_=>_)
				.ToArray();

			return result;
		}

		#endregion

		#region Public methods

		public static IrbisDatabaseInfo ParseServerResponse(string[] text)
		{
			IrbisDatabaseInfo result = new IrbisDatabaseInfo
										   {
											   LogicallyDeletedRecords = _ParseLine(text[1]),
											   PhysicallyDeletedRecords = _ParseLine(text[2]),
											   NonActualizedRecords = _ParseLine(text[3]),
											   LockedRecords = _ParseLine(text[4]),
											   MaxMfn = int.Parse(text[5]),
											   DatabaseLocked = (int.Parse(text[6]) != 0)
										   };

			return result;
		}

		public static IrbisDatabaseInfo[] ParseMenu(string[] text)
		{
			List<IrbisDatabaseInfo> result = new List<IrbisDatabaseInfo>();

			for (int i = 0; i < text.Length; i+=2)
			{
				string name = text[i];
				if (string.IsNullOrEmpty(name)
					||name.StartsWith("*"))
				{
					break;
				}
			    bool readOnly = false;
				if (name.StartsWith("-"))
				{
					name = name.Substring(1);
				    readOnly = true;
				}
				string description = text[i + 1];
				IrbisDatabaseInfo oneBase = new IrbisDatabaseInfo
					                            {
						                            Name = name,
													Description = description,
                                                    ReadOnly = readOnly
					                            };
				result.Add(oneBase);
			}

			return result.ToArray();
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
			StringBuilder result = new StringBuilder();

			result.AppendFormat("Name: {0}", Name);
			result.AppendLine();

			result.AppendFormat("Description: {0}", Description);
			result.AppendLine();

			result.Append("Logically deleted records: ");
			result.AppendLine(Utilities.CompressRange(LogicallyDeletedRecords));

			result.Append("Physically deleted records: ");
			result.AppendLine(Utilities.CompressRange(PhysicallyDeletedRecords));

			result.Append("Nonactualized records: ");
			result.AppendLine(Utilities.CompressRange(NonActualizedRecords));

			result.Append("Locked records: ");
			result.AppendLine(Utilities.CompressRange(LockedRecords));

			result.AppendFormat("Max MFN: {0}", MaxMfn);
			result.AppendLine();

			result.AppendFormat("Database locked: {0}", DatabaseLocked);
			result.AppendLine();

			return result.ToString();
		}

		#endregion
	}
}
