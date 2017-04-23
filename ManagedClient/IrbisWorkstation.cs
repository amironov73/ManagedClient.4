// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisWorkstation.cs
 */

namespace ManagedClient
{
    /// <summary>
    /// Коды АРМов ИРБИС.
    /// </summary>
	public enum IrbisWorkstation : byte
	{
		Administrator = (byte) 'A',

		Cataloger = (byte)'C',

		Acquisitions = (byte)'M',

		Reader = (byte) 'R',

		Circulation = (byte) 'B',

		Bookland = (byte) 'B',

		Provision = (byte) 'K'

	}
}
