#region Copyright Partum Artificium 2010
/************************************************************************
*   Copyright Partum Artificium 2010
*   All rights reserved. Reproduction or transmission in whole or in part
*   in any form or by any means is prohibited without prior written 
*   consent of copyright owner.
*************************************************************************/
#endregion

namespace PartumArtificium.Framework
{
	/// <summary> Interface for string encryption</summary>
	public interface IEncryption
	{
		/// <summary> Encrypt string</summary>
		/// <param name="inputValue">string to encrypt</param>
		/// <returns>encrypted string</returns>
		string Encrypt(string inputValue);

		/// <summary> Decrypt string</summary>
		/// <param name="inputValue">string to decrypt</param>
		/// <returns>decrypted stirng</returns>
		string Decrypt(string inputValue);
	}
}
