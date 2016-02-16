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
