#region Copyright Partum Artificium 2010
/************************************************************************
*   Copyright Partum Artificium 2010
*   All rights reserved. Reproduction or transmission in whole or in part
*   in any form or by any means is prohibited without prior written 
*   consent of copyright owner.
*************************************************************************/
#endregion

using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace PartumArtificium.Framework
{
	/// <summary> </summary>
	public class Encryption : IEncryption
	{
		#region Private Variables
		private byte[] _key;
		private byte[] _IV;
		#endregion

		#region Constructor
		/// <summary> </summary>
		/// <param name="key"></param>
		/// <param name="iv"></param>
		public Encryption(byte[] key, byte[] iv)
		{
			_key = key;
			_IV = iv;
		}
		#endregion

		#region Private Methods
		/// <summary> Transform array through encryption or decryption</summary>
		/// <param name="input">array to encrypt or decrypt</param>
		/// <param name="CryptoTransform">transform (encrypt or decrypt)</param>
		/// <returns>Encrypted or decrypted byte array</returns>
		private byte[] Transform(byte[] input, ICryptoTransform CryptoTransform)
		{
			byte[] result;

			using (MemoryStream memStream = new MemoryStream())
			{
				using (CryptoStream cryptStream = new CryptoStream(memStream, CryptoTransform, CryptoStreamMode.Write))
				{
					cryptStream.Write(input, 0, input.Length);
					cryptStream.FlushFinalBlock();

					memStream.Position = 0;
					result = memStream.ToArray();
				}
			}

			return result;
		}
		#endregion

		#region IEncryption Members
		/// <summary> Encrypt string </summary>
		/// <param name="text">String to encrypt</param>
		/// <returns>Encypted string</returns>
		public string Encrypt(string text)
		{
			using (TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider())
			{
				UTF8Encoding utf8 = new UTF8Encoding();

				byte[] input = utf8.GetBytes(text);
				byte[] output = Transform(input, des.CreateEncryptor(_key, _IV));
				return Convert.ToBase64String(output);
			}
		}

		/// <summary> Decrypt string </summary>
		/// <param name="text">String to decrypt</param>
		/// <returns>Decrypted string</returns>
		public string Decrypt(string text)
		{
			using (TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider())
			{
				UTF8Encoding utf8 = new UTF8Encoding();

				byte[] input = Convert.FromBase64String(text);
				byte[] output = Transform(input, des.CreateDecryptor(_key, _IV));
				return utf8.GetString(output);
			}
		}
		#endregion
	}
}
