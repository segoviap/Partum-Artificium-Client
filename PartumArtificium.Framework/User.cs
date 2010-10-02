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
	/// <summary> User object.  Used for all user types. </summary>
	public abstract class User : IUser
	{
		#region Private Variables
		private int _userId;
		private string _username;
		private string _password;
		private IEncryption _encryption;
		#endregion

		#region Constructor
		/// <summary> </summary>
		/// <param name="encryption"></param>
		public User(IEncryption encryption)
		{
			_encryption = encryption;
		}
		#endregion

		#region Public Methods
		/// <summary> </summary>
		/// <returns></returns>
		public string GetEncryptedPassword()
		{
			return _encryption.Encrypt(_password);
		}
		#endregion

		#region Abstract Methods
		/// <summary> Save user</summary>
		public abstract void Save();
		#endregion

		#region IUser Members

		public int UserId
		{
			get
			{
				return _userId;
			}
			set
			{
				_userId = value;
			}
		}

		public string Username
		{
			get
			{
				return _username;
			}
			set
			{
				_username = value;
			}
		}

		public string Password
		{
			get
			{
				return _password;
			}
			set
			{
				_password = value;
			}
		}

		#endregion
	}
}
